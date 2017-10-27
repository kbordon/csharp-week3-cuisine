using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestaurantCuisine.Models;

namespace RestaurantCuisine.Models.Tests
{
  [TestClass]
  public class RestaurantTests : IDisposable
  {
    public RestaurantTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=3306;database=restaurantcuisine_test;";
    }
    public void Dispose()
    {
      Restaurant.ClearAll();
    }

    [TestMethod]
    public void GetAll_RestaurantEmptyAtFirst_0()
    {
      int result = Restaurant.GetAll().Count;
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Equals_ReturnsTrueIfNamesAreTheSame_Restaurant()
    {
      Restaurant firstRestaurant = new Restaurant("Marios", "$", "pineapple pizza", 1);
      Restaurant secondRestaurant = new Restaurant("Marios", "$", "pineapple pizza", 1);

      Assert.AreEqual(firstRestaurant, secondRestaurant);
    }

    [TestMethod]
    public void Save_SavesToDatabase_RestaurantList()
    {
      Restaurant testRestaurant = new Restaurant("Marios", "$", "pineapple pizza", 1);

      testRestaurant.Save();
      List<Restaurant> result = Restaurant.GetAll();
      List<Restaurant> testList = new List<Restaurant>{testRestaurant};

      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Find_FindsRestaurantInDatabase_Restaurant()
    {
      Restaurant testRestaurant = new Restaurant("Marios", "$", "pineapple pizza", 1);
      testRestaurant.Save();

      Restaurant foundRestaurant = Restaurant.Find(testRestaurant.Id);

      Assert.AreEqual(testRestaurant, foundRestaurant);
    }

    [TestMethod]
    public void UpdateRestaurant_UpdateRestaurantInDatabase_Restaurant()
    {
      Restaurant testRestaurant = new Restaurant("Marios", "$", "pineapple pizza", 1);
      testRestaurant.Save();
      string newName = "Ginos";
      string newCost = "$$";
      string newDish = "bacon pizza";
      Restaurant newRestaurant = new Restaurant(newName, newCost, newDish, 1);
      testRestaurant.UpdateRestaurant(newName, newCost, newDish);

      Assert.AreEqual(newRestaurant.Name, testRestaurant.Name);
      Assert.AreEqual(newRestaurant.Cost, testRestaurant.Cost);
      Assert.AreEqual(newRestaurant.FavoriteDish, testRestaurant.FavoriteDish);
    }

    [TestMethod]
    public void Delete_DeletesRestaurantInDatabase_Restaurant()
    {
      Restaurant testRestaurant = new Restaurant("Marios", "$", "pineapple pizza", 1);
      testRestaurant.Save();
      Restaurant testRestaurant2 = new Restaurant("Ginos", "$$", "bacon pizza", 2);
      testRestaurant2.Save();

      List<Restaurant> testList = new List<Restaurant>{testRestaurant2};
      testRestaurant.Delete();

      List<Restaurant> result = Restaurant.GetAll();

      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Delete_DeleteRestaurantsByCuisineId_Restaurant()
    {
      Restaurant testRestaurant = new Restaurant("Marios", "$", "pineapple pizza", 1);
      testRestaurant.Save();
      Restaurant testRestaurant2 = new Restaurant("Sauced", "$$", "alcohol", 2);
      testRestaurant2.Save();
      Restaurant testRestaurant3 = new Restaurant("Luigis", "$", "not pineapple pizza", 1);
      testRestaurant3.Save();

      Restaurant.DeleteRestaurantsByCuisine(2);

      List<Restaurant> testList = Restaurant.GetAll();

      List<Restaurant> expectedList = new List<Restaurant> {testRestaurant, testRestaurant3};

      CollectionAssert.AreEqual(testList, expectedList);
    }

    [TestMethod]
    public void GetAllRestaurantsByCuisine_GetsRestaurantsInDatabaseByCuisineType_List()
    {
      Restaurant testRestaurant = new Restaurant("Marios", "$", "pineapple pizza", 1);
      testRestaurant.Save();
      Restaurant testRestaurant2 = new Restaurant("Sauced", "$$", "alcohol", 2);
      testRestaurant2.Save();
      Restaurant testRestaurant3 = new Restaurant("Luigis", "$", "not pineapple pizza", 1);
      testRestaurant3.Save();

      List<Restaurant> testList = Restaurant.GetAllRestaurantsByCuisine(1);
      List<Restaurant> expectedList = new List<Restaurant>{testRestaurant, testRestaurant3};

      CollectionAssert.AreEqual(testList, expectedList);

    }
  }
}
