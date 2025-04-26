using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBuildPlugin
{
    public class ModelAnalyzer
    {
        private Document _doc;

        public double TotalFloorArea { get; private set; }
        public int WallCount { get; private set; }
        public int RepeatedWallTypes { get; private set; }

        public ModelAnalyzer(Document doc)
        {
            _doc = doc;
            Analyze();
        }

        private void Analyze()
        {
            var walls = new FilteredElementCollector(_doc)
                .OfClass(typeof(Wall))
                .Cast<Wall>()
                .ToList();

            WallCount = walls.Count;
            RepeatedWallTypes = walls
                .GroupBy(w => w.WallType.Id)
                .Count(g => g.Count() > 3); // Treat 3+ as repeated

            var rooms = new FilteredElementCollector(_doc)
                .OfCategory(BuiltInCategory.OST_Rooms)
                .Cast<SpatialElement>();

            TotalFloorArea = rooms
                .Sum(r => r.get_Parameter(BuiltInParameter.ROOM_AREA)?.AsDouble() ?? 0);
        }
    }
}
