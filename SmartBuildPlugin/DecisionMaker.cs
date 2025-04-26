using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBuildPlugin
{
    public static class DecisionMaker
    {
        public static string MakeDecision(Dictionary<string, int> scores)
        {
            double avg = scores.Values.Average();

            if (avg >= 4)
                return "Recommended: Offsite Manufacturing";
            else if (avg <= 2)
                return "Recommended: Traditional Methods";
            else
                return "Recommendation: Neutral - Further Analysis Needed";
        }
    }
}
