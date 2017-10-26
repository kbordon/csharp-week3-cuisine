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
        cmd.CommandText = @"DELETE FROM restaurants; ALTER TABLE restaurants AUTO_INCREMENT = 1";
        cmd.ExecuteNonQuery();

        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
      }

      public static List<Restaurant> GetAll()
      {
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
        conn.Open();

        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM restaurants WHERE id = @thisId;";

        MySqlParameter thisId = new MySqlParameter();
        thisId.ParameterName = "@thisId";
        thisId.Value = searchId;

        cmd.Parameters.Add(thisId);

        int restaurantId = 0;
        string restaurantName = "";
        string restaurantCost = "";
        string restaurantDish = "";
        int restaurantCuisineId = 0;

        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        while(rdr.Read())
        {
          restaurantId = rdr.GetInt32(0);
          restaurantName = rdr.GetString(1);
          restaurantCost = rdr.GetString(2);
          restaurantDish = rdr.GetString(3);
          restaurantCuisineId = rdr.GetInt32(4);
        }

        Restaurant foundRestaurant = new Restaurant(restaurantName, restaurantCost, restaurantDish, restaurantCuisineId, restaurantId);

        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
        return foundRestaurant;
      }

      public void UpdateRestaurant(string newName, string newCost, string newDish)
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"UPDATE restaurants SET name = @newName, cost = @newCost, favorite_dish = @newDish WHERE id = @searchId;";

        MySqlParameter searchId = new MySqlParameter();
        searchId.ParameterName = "@searchId";
        searchId.Value = Id;
        cmd.Parameters.Add(searchId);

        //name
        MySqlParameter name = new MySqlParameter();
        name.ParameterName = "@newName";
        name.Value = newName;
        cmd.Parameters.Add(name);
        //cost
        MySqlParameter cost = new MySqlParameter();
        cost.ParameterName = "@newCost";
        cost.Value = newCost;
        cmd.Parameters.Add(cost);
        //favorite dish
        MySqlParameter restaurantDish = new MySqlParameter();
        restaurantDish.ParameterName = "@newDish";
        restaurantDish.Value = newDish;
        cmd.Parameters.Add(restaurantDish);

        cmd.ExecuteNonQuery();
        Name = newName;
        Cost = newCost;
        FavoriteDish = newDish;

        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
      }

      public void Delete()
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();

        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = "DELETE FROM restaurants WHERE id = @searchId;";

        MySqlParameter searchId = new MySqlParameter();
        searchId.ParameterName = "@searchId";
        searchId.Value = this.Id;
        cmd.Parameters.Add(searchId);

        cmd.ExecuteNonQuery();
        conn.Close();

        if (conn != null)
        {
          conn.Dispose();
        }
      }

      public static void DeleteRestaurantsByCuisine(int inputId)
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();

        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"DELETE FROM restaurants where cuisine_id = @cuisineId;";

        MySqlParameter cuisineId = new MySqlParameter();
        cuisineId.ParameterName = "@cuisineId";
        cuisineId.Value = inputId;
        cmd.Parameters.Add(cuisineId);

        cmd.ExecuteNonQuery();

        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
      }

      public static List<Restaurant> GetAllRestaurantsByCuisine(int inputId)
      {
        List<Restaurant> cuisineRestaurants = new List<Restaurant>{};
        MySqlConnection conn = DB.Connection();
        conn.Open();

        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM restaurants WHERE cuisine_id = @cuisineId;";

        MySqlParameter cuisineId = new MySqlParameter();
        cuisineId.ParameterName = "@cuisineId";
        cuisineId.Value = inputId;
        cmd.Parameters.Add(cuisineId);

        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        while(rdr.Read())
        {
          int returnId = rdr.GetInt32(0);
          string returnName = rdr.GetString(1);
          string returnCost = rdr.GetString(2);
          string returnDish = rdr.GetString(3);
          int returnCuisineId = rdr.GetInt32(4);
          Restaurant returnRestaurant = new Restaurant(returnName, returnCost, returnDish, returnCuisineId, returnId);
          cuisineRestaurants.Add(returnRestaurant);
        }
        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }

        return cuisineRestaurants;
      }
  }
}
