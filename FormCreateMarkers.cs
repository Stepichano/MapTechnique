using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Avalonia.Controls.Converters;
using GMap.NET;
using GMap.NET.MapProviders;
using System.Net;
using System.Globalization;
using Newtonsoft.Json.Linq;


namespace MapTechnique
{
    public partial class FormCreateMarkers : Form
    {
        public GMarker gmarkerBuffer { get; set; }

        public FormCreateMarkers()
        {
            InitializeComponent();
        }

        private void FormCreateMarkers_Load(object sender, EventArgs e)
        {   
            this.BackColor = Color.FromArgb(43,43,43);

        }


        private void gMapControl1_Load(object sender, EventArgs e)
        {
            
            // settings
            GMapProvider.WebProxy = WebRequest.GetSystemWebProxy();
            GMapProvider.WebProxy.Credentials = CredentialCache.DefaultCredentials;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            gMapControl1.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            gMapControl1.MinZoom = 2;
            gMapControl1.MaxZoom = 16;
            gMapControl1.Zoom = 4;
            gMapControl1.DragButton = MouseButtons.Left;
            gMapControl1.ShowCenter = false;
        }

        private void textBoxMarkerLat_Validated(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;
            // If all conditions have been met, clear the ErrorProvider of errors.
            errorProviderCoordinate.SetError(textBox, "");
        }

        private void textBoxMarkerLat_Validating(object sender, CancelEventArgs e)
        {
            var textBox = sender as TextBox;

            string errorMsg;

            if (!ValidLatValue(textBox.Text, out errorMsg))
            {
                // Cancel the event and select the text to be corrected by the user.
                e.Cancel = true;
                textBox.Select(0, textBox.Text.Length);

                // Set the ErrorProvider error with the text to display. 
                this.errorProviderCoordinate.SetError(textBox, errorMsg);
            }
        }

        public bool ValidLatValue(string latitude, out string errorMessage)
        {   
            errorMessage = "";
            if (latitude != string.Empty)
            {
                try
                {   // Set a dot as a delimiter and trying to read the value.
                    float value = Convert.ToSingle(latitude, CultureInfo.InvariantCulture.NumberFormat);
                    
                    if (value < 0 || value > 90)
                    {
                        errorMessage = "value is out of range.";
                        return false;
                    }
                }
                catch
                {
                    // Show validation error text
                    errorMessage = "The entered data is not in the correct format," +
                                   "please enter a valid format for the latitude. ";
                    return false;
                }
            }


            return true;
        }

        private void textBoxMarkerLng_Validated(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;
            // If all conditions have been met, clear the ErrorProvider of errors.
            errorProviderCoordinate.SetError(textBox, "");
        }

        private void textBoxMarkerLng_Validating(object sender, CancelEventArgs e)
        {
            var textBox = sender as TextBox;

            string errorMsg;

            if (!ValidLngValue(textBox.Text, out errorMsg))
            {
                // Cancel the event and select the text to be corrected by the user.
                e.Cancel = true;
                textBox.Select(0, textBox.Text.Length);

                // Set the ErrorProvider error with the text to display. 
                this.errorProviderCoordinate.SetError(textBox, errorMsg);
            }
        }

        public bool ValidLngValue(string longitude, out string errorMessage)
        {
            errorMessage = "";
            if (longitude != string.Empty)
            {
                try
                {
                    // Set a dot as a delimiter and trying to read the value.
                    float value = Convert.ToSingle(longitude, CultureInfo.InvariantCulture.NumberFormat);

                    if ((value < 0) || (value > 180))
                    {
                        errorMessage = "value is out of range.";
                        return false;
                    }
                }
                catch
                {
                    if (longitude != string.Empty)
                    {
                        // Show validation error text
                        errorMessage = "The entered data is not in the correct format," +
                                       "please enter a valid format for the longitude. ";
                        return false;
                    }
                }
            }


            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {   // Check if the fields are filled.
            if (textBoxMarkerLat.Text == string.Empty
                || textBoxMarkerLng.Text == string.Empty
                || textBoxMarkerName.Text == string.Empty)
                MessageBox.Show("Please fill in the empty fields");
            else
            {   // We create a marker and enter it into the database, and also pass it to form1.
                gmarkerBuffer = new GMarker(textBoxMarkerName.Text,
                    new Coordinates(Convert.ToSingle(textBoxMarkerLat.Text, CultureInfo.InvariantCulture.NumberFormat), Convert.ToSingle(textBoxMarkerLng.Text, CultureInfo.InvariantCulture.NumberFormat)));
                gmarkerBuffer.Id = SqlConnectionExtensions.InsertMarkerDataIntoDb(gmarkerBuffer.Name, gmarkerBuffer.Coordinates);

                this.DialogResult = DialogResult.OK;
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
