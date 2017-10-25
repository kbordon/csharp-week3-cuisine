using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestaurantCuisine.Models;

namespace RestaurantCuisine.Models.Tests
{
  [TestClass]
  public class CuisineTests : IDisposable
  {

    public CuisineTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=restaurantcuisine_test;";
    }

    public void Dispose()
    {
      Cuisine.ClearAll();
    }

    [TestMethod]
    public void GetAll_CuisinesEmptyAtFirst_0()
    {
      int result = Cuisine.GetAll().Count;

      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Equals_ReturnsTrueForSameName_Cuisine()
    {
      Cuisine firstCuisine = new Cuisine("Chinese");
      Cuisine secondCuisine = new Cuisine("Chinese");

      Assert.AreEqual(firstCuisine, secondCuisine);
    }

    [TestMethod]
    public void Save_SavesCuisineToDatabase_CuisineList()
    {
      Cuisine testCuisine = new Cuisine("Thai");
      testCuisine.Save();

      List<Cuisine> result = Cuisine.GetAll();
      List<Cuisine> testList = new List<Cuisine>{testCuisine};

      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Find_FindsCuisineInDatabase_Cuisine()
    {
      Cuisine testCuisine = new Cuisine("Filipino");
      testCuisine.Save();

      Cuisine foundCuisine = Cuisine.Find(testCuisine.Id);

      Assert.AreEqual(testCuisine, foundCuisine);
    }

    [TestMethod]
    public void UpdateName_UpdateCategoryInDatabase_String()
    {
      string name = "American";
      Cuisine testCuisine = new Cuisine(name);
      testCuisine.Save();
      string updatedName = "Breakfast";

      testCuisine.UpdateName(updatedName);

      string result = Cuisine.Find(testCuisine.Id).Name;

      Assert.AreEqual(updatedName, result);
    }

    [TestMethod]
    public void Delete_DeletesCategoryInDatabase_CuisineList()
    {
      Cuisine testCuisine = new Cuisine("Thai");
      testCuisine.Save();
      Cuisine testCuisine2 = new Cuisine("Georgian");
      testCuisine2.Save();

      List<Cuisine> testList = new List<Cuisine>{testCuisine2};
      testCuisine.Delete();

      List<Cuisine> result = Cuisine.GetAll();

      CollectionAssert.AreEqual(testList, result);

    }
  }
}
