using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tmds.DBus;
using System.Configuration;
using System.IO;
using System.Reflection.Metadata;
using System.Security.RightsManagement;
using JetBrains.Annotations;
using System.Globalization;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;

namespace MapTechnique
{
    public static class SqlConnectionExtensions
    {
        /// <summary>
        /// This method is called on the form load event and creates the database if it doesn't exist.
        /// </summary>
        public static void CreateDatabase()
        {
            var databaseName = ConfigurationManager.ConnectionStrings["MyDatabase"].Name;

            if (!DatabaseExists(databaseName))
            {
                
                using (var connection =
                       new SqlConnection(ConfigurationManager.ConnectionStrings["MyServerConnection"].ConnectionString))
                {


                    var createDbQuery = $"CREATE DATABASE {databaseName}";

                    using (var createDbCommand = new SqlCommand(createDbQuery, connection))
                    {
                        try
                        {
                            connection.Open();

                            createDbCommand.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"{ex.Message}");
                        }
                        finally
                        {
                            connection.Close();
                            EnsureCreateTable();
                            InitialDataFillingScript();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method checks the database connection.
        /// </summary>
        /// <param name="databaseName"></param>
        /// <returns>If the connection was successful it will return true,
        /// otherwise it will return false.
        /// </returns>
        public static bool DatabaseExists(string databaseName)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyServerConnection"].ConnectionString))
            {
                var query = $"SELECT COUNT(*) FROM sys.databases WHERE name='{databaseName}'";

                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    var count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        /// <summary>
        /// This method creates a table of markers in the database
        /// </summary>
        private static void EnsureCreateTable()
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDatabase"].ConnectionString))
            {
                var tableName = "Markers";
                // Create table Query
                var createTableQuery = $"CREATE TABLE [dbo].[{tableName}] (" +
                                       "[ID]    INT IDENTITY(1, 1) NOT NULL," +
                                       "[Name]  NVARCHAR(50) NULL," +
                                       "[Latitude] FLOAT NULL," +
                                       "[Longitude] FLOAT NULL," +
                                       "PRIMARY KEY CLUSTERED([ID] ASC)" +
                                       ");";

                using (var createTableCommand = new SqlCommand(createTableQuery, connection))
                {
                    try
                    {
                        connection.Open();

                        var transaction = connection.BeginTransaction();

                        createTableCommand.Transaction = transaction;

                        createTableCommand.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}");
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// This method adds the data of one marker to the database.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="coordinates"></param>
        /// <returns>record Id</returns>
        public static int InsertMarkerDataIntoDb(string name, PointLatLng coordinates)
        {   
            // In this variable, save the ID of the inserted record. 
            var id = -1;

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDatabase"].ConnectionString))
            {
                var tableName = "Markers";

                // Table creation query.
                var commandText = $"INSERT INTO [dbo].[{tableName}] (Name, Latitude, Longitude) VALUES (@name, @lat, @lng)" +
                                   "SELECT SCOPE_IDENTITY();";

                using (var command = new SqlCommand(commandText, connection))
                {
                    try
                    {   
                        connection.Open();
                        // create transaction and started
                        var transaction = connection.BeginTransaction();
                        
                        command.Transaction = transaction;

                        command.Parameters.AddWithValue("@name", $"{name}");
                        command.Parameters.AddWithValue("@lat", coordinates.Lat);
                        command.Parameters.AddWithValue("@lng", coordinates.Lng);

                        id = Convert.ToInt32(command.ExecuteScalar());
                        // ending transaction
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}");
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return id;
        }

        /// <summary>
        /// This method populates the Marker table with initial data
        /// </summary>
        public static void InitialDataFillingScript()
        {

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDatabase"].ConnectionString))
            {

                // Create table Query
                var commandText = "INSERT INTO [dbo].[Markers] (Name, Latitude, Longitude)" +
                                  "VALUES ('Москва', 55.7558, 37.6173)," +
                                  "('Санкт-Петербург', 59.9343, 30.3351)," +
                                  "('Новосибирск', 55.0084, 82.9357)," +
                                  "('Екатеринбург', 56.8389, 60.6057)," +
                                  "('Нижний Новгород', 56.2965, 43.9361)," +
                                  "('Казань', 55.8304, 49.0661)," +
                                  "('Челябинск', 55.1644, 61.4368)," +
                                  "('Омск', 54.9885, 73.3242)," +
                                  "('Самара', 53.1959, 50.1003)," +
                                  "('Ростов-на-Дону', 47.2357, 39.7015)," +
                                  "('Уфа', 54.7388, 55.9721)," +
                                  "('Красноярск', 56.0106, 92.8526)," +
                                  "('Пермь', 58.0104, 56.2294)," +
                                  "('Воронеж', 51.6606, 39.2006)," +
                                  "('Волгоград', 48.7071, 44.5170)," +
                                  "('Краснодар', 45.0355, 38.9750)," +
                                  "('Саратов', 51.5331, 46.0342)," +
                                  "('Тюмень', 57.1613, 65.5250)," +
                                  "('Тольятти', 53.5078, 49.4204)," +
                                  "('Ижевск', 56.8526, 53.2049); ";


                using (var command = new SqlCommand(commandText, connection))
                {
                    try
                    {
                        connection.Open();
                        // create transaction and started
                        var transaction = connection.BeginTransaction();

                        command.Transaction = transaction;

                        command.ExecuteNonQuery();

                        // ending trancsaction
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}");
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// This method processes the marker data and adds it to the list of markers.
        /// </summary>
        /// <param name="gMarkers"></param>
        public static void MarkerDataSelectionQuery(List<GMarker> gMarkers)
        {
            using (var connection =
                   new SqlConnection(ConfigurationManager.ConnectionStrings["MyDatabase"].ConnectionString))
            {
                var commandText = "SELECT ID, Name, Latitude, Longitude FROM [dbo].[Markers] ";
                
                SqlDataReader dataReader = null; 
                    
                using (var command = new SqlCommand(commandText, connection))
                {
                    try
                    {   
                        connection.Open();

                        dataReader = command.ExecuteReader();

                        while (dataReader.Read())
                        {   
                            var gMarker = new GMarker(new PointLatLng(Convert.ToDouble(dataReader["Latitude"], CultureInfo.InvariantCulture.NumberFormat), Convert.ToDouble(dataReader["Longitude"], CultureInfo.InvariantCulture.NumberFormat)))
                            {
                                Id = Convert.ToInt32(dataReader["ID"]),
                                Name = Convert.ToString(dataReader["Name"]),

                            };
                            gMarkers.Add(gMarker);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {   
                        if (dataReader != null && !dataReader.IsClosed)
                        {
                            dataReader.Close();
                        }

                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// The function writes changes to the database from the list.
        /// </summary>
        /// <param name="gMarkers"></param>
        public static void UpdateDataIntoDb(List<GMarker> gMarkers)
        {
            using (var connection =
                   new SqlConnection(ConfigurationManager.ConnectionStrings["MyDatabase"].ConnectionString))
            {
                try
                {
                    connection.Open();


                    foreach (var gMarker in gMarkers)
                    {
                        if (gMarker.IsMarkerMoved == true)
                        {
                            var commandText = "UPDATE [dbo].[Markers]" +
                                              $" SET Latitude = {gMarker.Position.Lat.ToString(CultureInfo.InvariantCulture)}," +
                                              $" Longitude = {gMarker.Position.Lng.ToString(CultureInfo.InvariantCulture)} " +
                                              $"WHERE ID = {gMarker.Id};" 
                                              ;
                            using (var command = new SqlCommand(commandText, connection))
                            {
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                    
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                
            }
        }
    }
}
