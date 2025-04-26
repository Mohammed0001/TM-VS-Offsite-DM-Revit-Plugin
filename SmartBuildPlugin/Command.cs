using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;
using System.Text;
using Microsoft.VisualBasic; // Add this namespace

namespace SmartBuildPlugin { 

[Transaction(TransactionMode.ReadOnly)]
public class Command : IExternalCommand
{
    public Result Execute(
        ExternalCommandData commandData,
        ref string message,
        ElementSet elements)
    {
        // Collect user inputs
        int siteConstraints = GetValidatedInput("Site Constraints (1-5):\n Where: \n1 = Unconstrained and \n5 = Extreme ", "Site Constraints");
        int timeline = GetValidatedInput("Timeline (1-5): \n Where: \n1 = Very relaxed and \n5 = Critical", "Timeline");
        int sustainability = GetValidatedInput("Sustainability (1-5):\n Where: \n1 = No sustainability goals and \n5 = Ultra‑high", "Sustainability");

        // Analyze model
        UIDocument uidoc = commandData.Application.ActiveUIDocument;
        Document doc = uidoc.Document;
        var analyzer = new ModelAnalyzer(doc);

        // Generate scores
        var scores = ScoringEngine.EvaluateProject(analyzer);
        scores["Site Constraints"] = siteConstraints; // Override placeholders
        scores["Timeline"] = timeline;
        scores["Sustainability"] = sustainability;

        // Generate recommendation
        var result = DecisionMaker.MakeDecision(scores);

        // Display results
        TaskDialog.Show("Recommendation",
            $"{result}\n\nScores:\n{string.Join("\n", scores.Select(kvp => $"{kvp.Key}: {kvp.Value}"))}");

        return Result.Succeeded;
    }

    private int GetValidatedInput(string prompt, string title)
    {
        string input;
        int value;
        do
        {
            input = Interaction.InputBox(prompt, title, "3", -1, -1);
            if (string.IsNullOrEmpty(input)) // Handle cancellation
                return 3; // Default value
        }
        while (!int.TryParse(input, out value) || value < 1 || value > 5);

        return value;
    }
}

}

//using Autodesk.Revit.DB;
//using Autodesk.Revit.UI;
//using static System.Runtime.InteropServices.JavaScript.JSType;
//using System.Text;
//using Autodesk.Revit.Attributes;

//namespace SmartBuildPlugin
//{
//    [Transaction(TransactionMode.ReadOnly)]
//    public class Command : IExternalCommand
//    {
//        public Result Execute(
//        ExternalCommandData commandData,
//        ref string message,
//        ElementSet elements)
//        {
//            UIDocument uidoc = commandData.Application.ActiveUIDocument;
//            Document doc = uidoc.Document;

//            var analyzer = new ModelAnalyzer(doc);
//            var scores = ScoringEngine.EvaluateProject(analyzer);
//            var result = DecisionMaker.MakeDecision(scores);

//            // Build display message
//            StringBuilder messageBuilder = new StringBuilder();
//            messageBuilder.AppendLine(result);
//            messageBuilder.AppendLine("\nFactor Scores:");
//            foreach (var score in scores)
//            {
//                messageBuilder.AppendLine($"{score.Key}: {score.Value}");
//            }

//            // Show in Revit dialog
//            TaskDialog.Show("SmartBuild Recommendation", messageBuilder.ToString());

//            return Result.Succeeded;
//        }
//    }
//}
