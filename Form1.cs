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
            _uss.Add(new Us("Новосибирск", new Coordinates(82.9346, 82.9346)));
            GMapProvider.WebProxy = WebRequest.GetSystemWebProxy();
            GMapProvider.WebProxy.Credentials = CredentialCache.DefaultCredentials;

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

        private void CreateMarker_Click(object sender, EventArgs e)
        {
            gMapControl1.Overlays.Add(GMarker.GetOverlayMarkers(_uss, "GroupMarkers", GMarkerGoogleType.red));
            gMapControl1.Update();
        }

        private void Owerlay_Click(object sender, EventArgs e)
        {
            gMapControl1.Overlays[0].IsVisibile = true;
        }

       

        private void btnCreateDb_Click(object sender, EventArgs e)
        {
            var conStr = ConfigurationManager.ConnectionStrings["GMarkers"].ConnectionString;

            using (var myConn =
                   new SqlConnection(conStr))
            {

                var str = "";


                using (SqlCommand myCommand = new SqlCommand(str, myConn))
                {
                    try
                    {
                        myConn.Open();
                        myCommand.ExecuteNonQuery();
                        MessageBox.Show("DataBase is Created Successfully", "MyProgram", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "MyProgram", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    finally
                    {
                        if (myConn.State == ConnectionState.Open)
                        {
                            myConn.Close();
                        }
                    }
                }
            }
        }

    }
}
