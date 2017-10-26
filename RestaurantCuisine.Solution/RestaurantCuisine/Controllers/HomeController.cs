using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using RestaurantCuisine.Models;

namespace RestaurantCuisine.Controllers
{
    public class HomeController : Controller
    {
      [HttpGet("/")]
      public ActionResult Index()
      {
        return View();
      }

      [HttpGet("/cuisines/new")]
      public ActionResult CuisineForm()
      {
        return View();
      }

      [HttpGet("/cuisines")]
      public ActionResult ViewCuisines()
      {
        List<Cuisine> allCuisines = Cuisine.GetAll();
        return View("Cuisines", allCuisines);
      }

      [HttpPost("/cuisines/new")]
      public ActionResult Cuisines()
      {
        Cuisine newCuisine = new Cuisine(Request.Form["cuisine-name"]);
        newCuisine.Save();
        List<Cuisine> allCuisines = Cuisine.GetAll();
        return View(allCuisines);
      }

      [HttpPost("/cuisines/clear")]
      public ActionResult ClearCuisines()
      {
        Cuisine.ClearAll();
        Restaurant.ClearAll();
        List<Cuisine> model = Cuisine.GetAll();
        return View("Cuisines", model);
      }

      [HttpGet("/cuisines/{id}")]
      public ActionResult CuisineDetail(int id)
      {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        model.Add("selected-restaurant", null);
        Cuisine selectedCuisine = Cuisine.Find(id);
        model.Add("this-cuisine", selectedCuisine);
        List<Restaurant> cuisineRestaurants = Restaurant.GetAllRestaurantsByCuisine(selectedCuisine.Id);
        model.Add("cuisine-restaurants", cuisineRestaurants);
        return View(model);
      }

      [HttpGet("/cuisines/{id}/restaurants/new")]
      public ActionResult RestaurantForm(int id)
      {
        // is this necessary? go back later and check it out.
        Cuisine selectedCuisine = Cuisine.Find(id);
        return View(selectedCuisine);
      }

      [HttpPost("/cuisines/{id}/restaurants/new")]
      public ActionResult AddRestaurant(int id)
      {
        Restaurant newRestaurant = new Restaurant(Request.Form["name"], Request.Form["cost"], Request.Form["dish"], id);
        newRestaurant.Save();

        Dictionary<string, object> model = new Dictionary<string, object> {};
        model.Add("selected-restaurant", null);
        Cuisine selectedCuisine = Cuisine.Find(id);
        model.Add("this-cuisine", selectedCuisine);
        List<Restaurant> allRestaurants = Restaurant.GetAllRestaurantsByCuisine(id);
        model.Add("cuisine-restaurants", allRestaurants);
        return View("CuisineDetail", model);
      }

      [HttpGet("/cuisines/{id}/restaurants/{restaurantId}")]
      public ActionResult RestaurantDetails(int id, int restaurantId)
      {
        Dictionary<string, object> model = new Dictionary<string, object> {};
        List<Restaurant> allRestaurants = Restaurant.GetAllRestaurantsByCuisine(id);
        Cuisine selectedCuisine = Cuisine.Find(id);
        Restaurant selectedRestaurant = Restaurant.Find(restaurantId);
        model.Add("cuisine-restaurants", allRestaurants);
        model.Add("this-cuisine", selectedCuisine);
        model.Add("selected-restaurant", selectedRestaurant);
        return View("CuisineDetail", model);
      }

      [HttpPost("/cuisines/{id}/restaurants/clear")]
      public ActionResult ClearRestaurantsInCuisine(int id)
      {
        Restaurant.DeleteRestaurantsByCuisine(id);
        Dictionary<string, object> model = new Dictionary<string, object> {};
        model.Add("selected-restaurant", null);
        Cuisine selectedCuisine = Cuisine.Find(id);
        model.Add("this-cuisine", selectedCuisine);
        model.Add("cuisine-restaurants", null);
        return View("CuisineDetail", model);
      }

      [HttpPost("/cuisines/{id}/delete")]
      public ActionResult DeleteCuisine(int id)
      {
        Cuisine selectedCuisine = Cuisine.Find(id);
        selectedCuisine.Delete();
        Restaurant.DeleteRestaurantsByCuisine(id);
        List<Cuisine> allCuisines = Cuisine.GetAll();
        return View("Cuisines", allCuisines);

      }

      [HttpPost("/cuisines/{id}/restaurants/{restaurantId}/delete")]
      public ActionResult DeleteRestaurant(int id, int restaurantId)
      {
        Restaurant selectedRestaurant = Restaurant.Find(restaurantId);
        selectedRestaurant.Delete();
        Dictionary<string, object> model = new Dictionary<string, object> {};
        model.Add("selected-restaurant", null);
        Cuisine selectedCuisine = Cuisine.Find(id);
        model.Add("this-cuisine", selectedCuisine);
        List<Restaurant> allRestaurants = Restaurant.GetAllRestaurantsByCuisine(id);
        model.Add("cuisine-restaurants", allRestaurants);
        return View("CuisineDetail", model);
      }

    }
}
