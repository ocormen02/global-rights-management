using GlobalRightsManagement.Core;
using System.Globalization;

// Define file paths
var contractsPath = Path.Combine(AppContext.BaseDirectory, "music_contract.txt");
var partnersPath = Path.Combine(AppContext.BaseDirectory, "partner_contracts.txt");

Console.Write("Enter partner name (ITunes, YouTube): ");
var partner = Console.ReadLine()?.Trim();

Console.Write("Enter date (MM-dd-yyyy): ");
var dateInput = Console.ReadLine()?.Trim();

if (string.IsNullOrWhiteSpace(partner) || string.IsNullOrWhiteSpace(dateInput))
{
    Console.WriteLine("Partner and Date are required");
    return;
}

if (!DateOnly.TryParseExact(dateInput, "MM-dd-yyyy", CultureInfo.InvariantCulture,
            DateTimeStyles.None, out var date))
{
    Console.WriteLine("Invalid format. Use MM-dd-yyyy (eg: 03-01-2012).");
    return;
}

// Query the contracts service
var rows = GRMQueryService.Query(contractsPath, partnersPath, partner, date);

// Display results
Console.WriteLine();
Console.WriteLine("| Artist       | Title                   | Usages            | StartDate   | EndDate |");
foreach (var r in rows)
{
    var usages = string.Join(", ", r.Usages);
    Console.WriteLine($"| {r.Artist,-12} | {r.Title,-23} | {usages,-16} | {r.StartDate:MM-dd-yyyy,-11} | {r.EndDate:MM-dd-yyyy} |");
}