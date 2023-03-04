using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Windows.Forms;
using GMap.NET.Avalonia;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms.Markers;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Xml.Linq;


namespace MapTechnique
{
    public partial class Form1 : Form
    {   
        private List<Us> _uss = new List<Us>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(43, 43, 43);
            ///
            /// Form1
            ///
            this.StartPosition = FormStartPosition.WindowsDefaultBounds;
            ///
            /// menuStrip1
            ///
            menuStrip1.BackColor = this.BackColor;
            menuStrip1.ShowItemToolTips = true;
            ///
            /// fileToolStripMenuItem
            ///  
            fileToolStripMenuItem.BackColor = menuStrip1.BackColor;
            foreach (ToolStripItem dropDownItem in fileToolStripMenuItem.DropDownItems)
            {
                dropDownItem.BackColor = menuStrip1.BackColor;
            }
            ///
            /// helpToolStripMenuItem
            ///
            helpToolStripMenuItem.BackColor = menuStrip1.BackColor;
            foreach (ToolStripItem dropDownItem in helpToolStripMenuItem.DropDownItems)
            {
                dropDownItem.BackColor =  menuStrip1.BackColor;
            }
            ///
            /// Gmap
            /// 
            GMapProvider.WebProxy = WebRequest.GetSystemWebProxy();
            GMapProvider.WebProxy.Credentials = CredentialCache.DefaultCredentials;
            _uss.Add(new Us("Новосибирск", new Coordinates(82.9346, 82.9346)));
            ///
            /// Database
            ///
            CreateDatabase(); 
            CreateAndPopulateTable();
        }

        private void gMapControl1_Load_1(object sender, EventArgs e)
        {
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly; //выбор подгрузки карты – онлайн или из ресурсов
            gMapControl1.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance; //какой провайдер карт используется (в нашем случае гугл) 
            gMapControl1.MinZoom = 2; //минимальный зум
            gMapControl1.MaxZoom = 16; //максимальный зум
            gMapControl1.Zoom = 4; // какой используется зум при открытии
            gMapControl1.Position = new GMap.NET.PointLatLng(66.4169575018027, 94.25025752215694);// точка в центре карты при открытии (центр России)
            gMapControl1.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter; // как приближает (просто в центр карты или по положению мыши)
            gMapControl1.CanDragMap = true; // перетаскивание карты мышью
            gMapControl1.DragButton = MouseButtons.Left; // какой кнопкой осуществляется перетаскивание
            gMapControl1.ShowCenter = false; //показывать или скрывать красный крестик в центре
            gMapControl1.ShowTileGridLines = false; //показывать или скрывать тайлы
        }

        private static void CreateDatabase()
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyServerConnection"].ConnectionString))
            {

                var databaseName = "MyDataBase";

                var dataFilePath =
                    $@"C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\Data\{databaseName}.mdf";

                var logFilePath =
                    $@"C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\Data\{databaseName}_log.ldf";

                var createDbQuery2 = $"CREATE DATABASE {databaseName}";
                    // $"ON (NAME = {databaseName}, FILENAME = '{dataFilePath}') LOG ON (NAME = '{databaseName}_log', FILENAME = '{logFilePath}')";


                using (var createDbCommand = new SqlCommand(createDbQuery2, connection))
                {
                    try
                    {
                        connection.Open();

                        createDbCommand.ExecuteNonQuery();

                    }
                    /*catch (Exception ex)
                    {
                    //    MessageBox.Show($"{ex.Message}");
                    }*/
                    finally
                    {
                        connection.Close();
                    }
                }
            }

        }

        private static void CreateAndPopulateTable()
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDatabaseConnection"].ConnectionString))
            {

                // Создание таблицы
                var createTableQuery = "CREATE TABLE Users (Id int PRIMARY KEY, Name nvarchar(50), Age int)";

                // Заполнение таблицы данными
                var insertDataQuery =
                    "INSERT INTO Users (Id, Name, Age) VALUES (1, 'John', 25), (2, 'Jane', 30), (3, 'Bob', 40)";

                using (var createTableCommand = new SqlCommand(createTableQuery, connection))
                {
                    try
                    {
                        connection.Open();

                        var insertDataCommand = new SqlCommand(insertDataQuery, connection);

                    }
                    /*catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}");
                    }*/
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        private void btnCreateMarker_Click(object sender, EventArgs e)
        {
            var formCreateMarkers = new FormCreateMarkers();
            formCreateMarkers.ShowDialog();
        }

        private void btnMarkersOnOFf_Click(object sender, EventArgs e)
        {
            gMapControl1.Overlays.Add(GMarker.GetOverlayMarkers(_uss, "GroupMarkers", GMarkerGoogleType.red));
            gMapControl1.Update();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

       
    }
}
