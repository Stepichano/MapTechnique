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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Tmds.DBus;
using GMapMarker = GMap.NET.WindowsForms.GMapMarker;


namespace MapTechnique
{
    public partial class Form1 : Form
    {   
        public List<GMarker> gMarkers = new List<GMarker>();

        private GMapMarker _selectedMarker;
        public GMapMarker AddGMapMarker { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ///
            /// Form1
            ///
            this.BackColor = Color.FromArgb(43, 43, 43);
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
            /// Database
            ///
            SqlConnectionExtensions.CreateDatabase(); // If DataBase dos not exist, will be created
            ///
            /// gMapControl1
            /// 
            SqlConnectionExtensions.MarkerDataSelectionQuery(gMarkers);
            gMapControl1.Overlays.Add(GMarker.Get_GMapOverlayGMarkers(gMarkers, "GroupMarkers", GMarkerGoogleType.red)); //create an overlay
            gMapControl1.Overlays[0].IsVisibile = false; //make the overlay invisible
            gMapControl1.Update();
            ///
            /// btns close, collapse, reextarnal
            ///
            btnClose.Image = Properties.Resources.close;
            btnReExtarnal.Image = Properties.Resources.reexternal;
            btnCollapse.Image = Properties.Resources.collapse;
        }

        private void gMapControl1_Load_1(object sender, EventArgs e)
        {
            // settings
            GMapProvider.WebProxy = WebRequest.GetSystemWebProxy();
            GMapProvider.WebProxy.Credentials = CredentialCache.DefaultCredentials;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly; //set the mode of loading maps only from online resources
            gMapControl1.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance; //which map provider is used (in our case, Google) 
            gMapControl1.MinZoom = 2; //minimum zoom
            gMapControl1.MaxZoom = 16; //maximum zoom
            gMapControl1.Zoom = 6; //what zoom is used when opening
            gMapControl1.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter; //how it zooms in (just to the center of the map or by mouse position)
            gMapControl1.Position = new GMap.NET.PointLatLng(55.0415, 82.9346); //initial position of the map
            // Specify that all edges of the control
            // pinned to the edges of its containing element
            // controls (of the main form) and their sizes change
            // respectively.
            //gMapControl1.Dock = DockStyle.Fill; 
            gMapControl1.ShowCenter = false; //show or hide the red cross in the center
            gMapControl1.ShowTileGridLines = false; //show or hide tiles
            // Events
            gMapControl1.CanDragMap = true; // перетаскивание карты мышью
            gMapControl1.DragButton = MouseButtons.Left; // какой кнопкой осуществляется перетаскивание.
            gMapControl1.MouseDown += gMapControl1_MouseDown;
            gMapControl1.MouseUp += gMapControl1_MouseUp;

        }

        private void btnCreateMarker_Click(object sender, EventArgs e)
        {
            // Create a new marker constructor form.
            var formCreateMarkers = new FormCreateMarkers();
            formCreateMarkers.ShowDialog();

            // If the add button was clicked, handle this event. 
            if (formCreateMarkers.DialogResult == DialogResult.OK)
            {
                gMapControl1.Overlays[0].Markers.Add(GMarker.GetMarker(formCreateMarkers.gmarkerBuffer));
            }

        }

        private void btnMarkersOnOFf_Click(object sender, EventArgs e)
        {
            // Onn/Off markers.
            gMapControl1.Overlays[0].IsVisibile = !gMapControl1.Overlays[0].IsVisibile;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void gMapControl1_MouseDown(object sender, MouseEventArgs e)
        {
            // Find the marker over which the mouse button was pressed.
            _selectedMarker = gMapControl1.Overlays
                .SelectMany(o => o.Markers)
                .FirstOrDefault(m => m.IsMouseOver == true);
            
        }


        private void gMapControl1_MouseUp(object sender, MouseEventArgs e)
        {
            if(_selectedMarker is null)
                return;

            // transfer mouse cursor coordinates to longitude and latitude on the map.
            var latlng = gMapControl1.FromLocalToLatLng(e.X, e.Y);
            // Assign a new position for the marker.
            _selectedMarker.Position = latlng;
            foreach (var gMarker in gMarkers)
            {
                if (_selectedMarker.ToolTipText == gMarker.Name)
                {
                    gMarker.Position = _selectedMarker.Position;
                    gMarker.IsMarkerMoved = true;
                }
            }
            // deselect marker.
            _selectedMarker = null;

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите выйти из приложения?", "Выход", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                // Save the changed coordinates.
                SqlConnectionExtensions.UpdateDataIntoDb(gMarkers);
            }
            else
            {
                e.Cancel = true; // отменить закрытие формы
            }
        }

        private void btnReExtarnal_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }


        }

        private void btnCollapse_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
