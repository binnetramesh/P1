using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using P1.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace P1.Common
{
    public class DbOperations
    {
        #region Private Variables
        private MySqlConnection connection;
        private string connString = string.Empty;
        #endregion

        #region Default Constructor
        public DbOperations()
        {
            try
            {
                var configurationBuilder = new ConfigurationBuilder();
                string path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
                configurationBuilder.AddJsonFile(path, false);
                connString = configurationBuilder.Build().GetSection("ConnectionStrings:DBConn").Value;
                //connString = ConfigurationManager.ConnectionStrings["AvesConn"].ConnectionString.ToString();
                connection = new MySqlConnection(connString);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region Manage Connections
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                return false;
            }
        }


        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                connection.Close();
                return false;
            }
        }
        #endregion

        #region Inventory
        public List<Inventory> GetAllInventoryList(string Id)
        {
            try
            {
                List<Inventory> lst = new List<Inventory>();
                string query = "sp_getallInventory";

                if (this.OpenConnection() == true)
                {

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new MySqlParameter("i_Id", Id));
                        using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            IEnumerable<DataRow> sequence = dt.AsEnumerable();
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                lst = (from DataRow row in dt.Rows
                                       select new Inventory
                                       {
                                           Name = Convert.ToString(row["Name"]),
                                           Country = Convert.ToString(row["Country"]),
                                           CountryId = Convert.ToString(row["CountryId"]),
                                           StartDate = Convert.ToString(row["StartDate"]),
                                           Id = Convert.ToString(row["Id"]),
                                       }).ToList();
                            }
                        }

                    }

                    this.CloseConnection();
                }

                return lst;
            }
            catch (Exception e)
            {
                //log.Error(e);
                throw e;
            }
        }
        public void AddInventory(Inventory opp)
        {
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_insert_Inventory", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new MySqlParameter("i_Name", opp.Name));
                    cmd.Parameters.Add(new MySqlParameter("i_CountryId", opp.CountryId));
                    cmd.Parameters.Add(new MySqlParameter("i_StartDate", opp.StartDate));
                    if (this.OpenConnection() == true)
                    {
                        cmd.ExecuteNonQuery();
                        this.CloseConnection();
                    }
                }
            }
            catch(Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region Country
        public List<Country> GetAllCountryList(string Id)
        {
            try
            {
                List<Country> lst = new List<Country>();
                if (this.OpenConnection() ==true)
                {
                    using (MySqlCommand cmd=new MySqlCommand("sp_getAllCountry", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add(new MySqlParameter("i_Id", Id));
                        using (MySqlDataAdapter sda=new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            lst = (from DataRow row in dt.Rows
                                   select new Country
                                   {
                                       Id=Convert.ToString(row["Id"]),
                                       CountryName = Convert.ToString(row["CountryName"]),
                                   }).ToList();
                        }
                    }
                    this.CloseConnection();
                }
                return lst;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}
