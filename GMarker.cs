using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;

namespace MapTechnique
{
    public class GMarker 
    {   
        /// <summary>
        /// Функция создающая маркер на основе объкта us.
        /// </summary>
        /// <param name="us"></param>
        /// <param name="gMarkerGoogleType"></param>
        /// <returns>GMarkerGoogle</returns>
        public static GMarkerGoogle GetMarker(Us us, GMarkerGoogleType gMarkerGoogleType = GMarkerGoogleType.red)
        {   
            
            var mapMarker = new GMarkerGoogle(new GMap.NET.PointLatLng(us.Coordinates.Lat, us.Coordinates.Lon), gMarkerGoogleType);//широта, долгота, тип маркера
            mapMarker.ToolTip = new GMap.NET.WindowsForms.ToolTips.GMapRoundedToolTip(mapMarker);//всплывающее окно с инфой к маркеру
            mapMarker.ToolTipText = us.Id; // текст внутри всплывающего окна
            mapMarker.ToolTipMode = MarkerTooltipMode.OnMouseOver; //отображение всплывающего окна (при наведении)
            return mapMarker;
        }

        /// <summary>
        /// Эта функция создает холст с маркерами.
        /// Принимает список объектов us, затем создает маркеры на его основе
        /// и добавляет их на холст.
        /// </summary>
        /// <param name="uss"></param>
        /// <param name="name"></param>
        /// <param name="gMarkerGoogleType"></param>
        /// <returns>GetOverlayMarkers</returns>
        public static GMapOverlay GetOverlayMarkers(List<Us> uss, string name, GMarkerGoogleType gMarkerGoogleType = GMarkerGoogleType.red)
        {
            var gMapMarkers = new GMapOverlay(name);// создание именованного слоя 
            foreach (var us in uss)
            {
                gMapMarkers.Markers.Add(GetMarker(us, gMarkerGoogleType));// добавление маркеров на слой
            }
            return gMapMarkers;
        }

    }
}
