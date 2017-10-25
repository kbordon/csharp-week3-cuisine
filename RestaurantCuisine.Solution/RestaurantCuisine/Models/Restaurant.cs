using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;


namespace RestaurantCuisine.Models
{
  public class Restaurant
  {
    public string Name {get; private set;}
    public string Cost {get; private set;}
    public string FavoriteDish {get; private set;}
    public int Id {get; private set;}
    public int CuisineId {get; private set;}

    public Restaurant(string name, string cost, string favoriteDish, int cuisineId, int id = 0)
    {
      Name = name;
      Cost = cost;
      FavoriteDish = favoriteDish;
      Id = id;
      CuisineId = cuisineId;
    }

    public static void ClearAll()
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();

        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"DELETE FROM restaurants;";
        cmd.ExecuteNonQuery();

        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
      }

      public static List<Restaurant> GetAll()
      {
        Console.WriteLine("in getall");
        List<Restaurant> allRestaurants = new List<Restaurant> {};
        MySqlConnection conn = DB.Connection();
        conn.Open();
        MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM restaurants;";
        MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
        while (rdr.Read())
        {
          int restaurantId = rdr.GetInt32(0);
          string restaurantName = rdr.GetString(1);
          string restaurantCost = rdr.GetString(2);
          string restaurantDish = rdr.GetString(3);
          int cuisineId = rdr.GetInt32(4);

          Restaurant newRestaurant = new Restaurant(restaurantName, restaurantCost, restaurantDish, cuisineId, restaurantId);
          allRestaurants.Add(newRestaurant);
        }
        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
        return allRestaurants;
      }

      public override bool Equals(System.Object otherRestaurant)
      {
        if (!(otherRestaurant is Restaurant))
        {
          return false;
        }
        else
        {
          Restaurant newRestaurant = (Restaurant) otherRestaurant;
          bool idEqaulity = (this.Id == newRestaurant.Id);
          bool nameEquality = (this.Name == newRestaurant.Name);
          bool costEquality = (this.Cost == newRestaurant.Cost);
          bool dishEquality = (this.FavoriteDish == newRestaurant.FavoriteDish);
          bool cuisineEquality = (this.CuisineId == newRestaurant.CuisineId);
          return (idEqaulity && nameEquality && costEquality && dishEquality && cuisineEquality);
        }
      }

      public void Save()
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();

        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO restaurants (name, cost, favorite_dish, cuisine_id) VALUES (@RestaurantName, @RestaurantCost, @FavoriteDish, @CuisineId);";
        //name
        MySqlParameter name = new MySqlParameter();
        name.ParameterName = "@RestaurantName";
        name.Value = this.Name;
        cmd.Parameters.Add(name);
        //cost
        MySqlParameter cost = new MySqlParameter();
        cost.ParameterName = "@RestaurantCost";
        cost.Value = this.Cost;
        cmd.Parameters.Add(cost);
        //favorite dish
        MySqlParameter restaurantDish = new MySqlParameter();
        restaurantDish.ParameterName = "@FavoriteDish";
        restaurantDish.Value = this.FavoriteDish;
        cmd.Parameters.Add(restaurantDish);
        //cuisine id
        MySqlParameter cuisineId = new MySqlParameter();
        cuisineId.ParameterName = "@CuisineId";
        cuisineId.Value = this.CuisineId;
        cmd.Parameters.Add(cuisineId);

        cmd.ExecuteNonQuery();
        Id = (int)cmd.LastInsertedId;

        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
      }

      public static Restaurant Find(int searchId)
      {
        MySqlConnection conn = DB.Connection();
      }
  }
}
