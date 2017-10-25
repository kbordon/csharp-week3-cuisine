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
    }
}
