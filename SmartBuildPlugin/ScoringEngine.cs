using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SmartBuildPlugin
{
    public static class ScoringEngine
    {
        public static Dictionary<string, int> EvaluateProject(ModelAnalyzer data)
        {
            var scores = new Dictionary<string, int>();

            // Total area (converted from ft² to m²)
            double areaM2 = UnitUtils.ConvertFromInternalUnits(data.TotalFloorArea, UnitTypeId.SquareMeters);
            scores["Project Size"] = areaM2 > 1000 ? 5 : areaM2 > 500 ? 3 : 1;

            scores["Design Standardization"] = data.RepeatedWallTypes > 3 ? 5 : 3;

            scores["Complexity"] = data.WallCount > 100 ? 2 : 4;

            // Placeholder scores for now
            scores["Site Constraints"] = 4;
            scores["Timeline"] = 5;
            scores["Sustainability"] = 3;

            return scores;
        }
    }
}
