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
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=restaurantcuisine_test;";
    }
    public void Dispose()
    {
      Restaurant.ClearAll();
    }

    [TestMethod]
    public void GetAll_RestaurantEmptyAtFirst_0()
    {
      int result = Restaurant.GetAll().Count;
      Console.WriteLine("before getall");
      Console.WriteLine("after getall");
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
  }
}
