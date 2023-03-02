using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace MapTechnique
{
    [DefaultMember("coordinates")]
    public class Us
    {

        public string Id { get; set; }
        public Coordinates Coordinates { get; set; }
        public Us(string id, Coordinates coordinates)
        {
            Id = id;
            Coordinates = coordinates;
        }

    }
}
