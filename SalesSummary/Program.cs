using System.Text.Json;
using System.Text;

string storesPath = Path.Combine("..", "stores");
string[] storeDirectories = Directory.GetDirectories(storesPath);

decimal totalSales = 0;
StringBuilder report = new StringBuilder();

report.AppendLine("Sales Summary");
report.AppendLine("----------------------------");
report.AppendLine();

report.AppendLine("Details:");

foreach (string storeDirectory in storeDirectories)
{
    string storeNumber = Path.GetFileName(storeDirectory);
    string salesTotalsFile = Path.Combine(storeDirectory, "salestotals.json");

    if (File.Exists(salesTotalsFile))
    {
        string salesData = File.ReadAllText(salesTotalsFile);

        using JsonDocument document = JsonDocument.Parse(salesData);

        decimal storeTotal = 0;

        foreach (JsonProperty property in document.RootElement.EnumerateObject())
        {
            if (property.Value.ValueKind == JsonValueKind.Number)
            {
                storeTotal += property.Value.GetDecimal();
            }
        }

        totalSales += storeTotal;

        report.AppendLine($" {storeNumber}: {storeTotal:C}");
    }
}

report.Insert(
    report.ToString().IndexOf("Details:"),
    $" Total Sales: {totalSales:C}\n\n"
);

string reportFile = "sales-summary.txt";
File.WriteAllText(reportFile, report.ToString());

Console.WriteLine(report.ToString());
Console.WriteLine($"Sales summary saved to {reportFile}");