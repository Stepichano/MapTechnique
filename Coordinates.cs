using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapTechnique
{
    public class Coordinates
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
        public Coordinates(double lat, double lon)
        {
            Lat = lat;
            Lon = lon;
        }
    }
}
