using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Avalonia.Controls.Converters;

namespace MapTechnique
{
    public partial class FormCreateMarkers : Form
    {
        public FormCreateMarkers()
        {
            InitializeComponent();
        }

        private void FormCreateMarkers_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(43,43,43);
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("hello");
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
