using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;

namespace MapTechnique
{
    public class GMarker : GMapMarker
    {
        public bool IsMarkerMoved = false;
        public int Id { get; set; }
        public string Name { get; set; }

        public GMarker(PointLatLng pos) : base(pos)
        {

        }

        /// <summary>
        /// A function that creates a marker based on a GMarker object.
        /// </summary>
        /// <param name="gMarkeraram>
        /// <param name="gMarkerGoogleType"></param>
        /// <returns>GMarkerGoogle</returns>
        public static GMarkerGoogle GetMarker(GMarker gMarker, GMarkerGoogleType gMarkerGoogleType = GMarkerGoogleType.red)
        {   
            var mapMarker = new GMarkerGoogle(gMarker.Position, gMarkerGoogleType);//позиция, тип маркера
            mapMarker.ToolTip = new GMap.NET.WindowsForms.ToolTips.GMapRoundedToolTip(mapMarker); //marker info popup
            mapMarker.ToolTipText = gMarker.Name; //text inside the popup
            mapMarker.ToolTipMode = MarkerTooltipMode.OnMouseOver; //popup display (on hover)
            return mapMarker;
        }
        /// <summary>
        /// This function creates a canvas with markers.
        /// Takes a list of gMarker objects, then creates markers based on it
        /// and adds them to the canvas.
        /// </summary>
        /// <param name="gMarkers"></param>
        /// <param name="name"></param>
        /// <param name="gMarkerGoogleType"></param>
        /// <returns>GMapOverlay</returns>
        public static GMapOverlay Get_GMapOverlayGMarkers(List<GMarker> gMarkers, string name, GMarkerGoogleType gMarkerGoogleType = GMarkerGoogleType.red)
        {
            var gMapOverlayGmarkers = new GMapOverlay(name); //create new overlay
            foreach (var gMarker in gMarkers)
            {
                gMapOverlayGmarkers.Markers.Add(GetMarker(gMarker, gMarkerGoogleType)); //adding markers to the overlay
            }
            return gMapOverlayGmarkers;
        }
    }
}
