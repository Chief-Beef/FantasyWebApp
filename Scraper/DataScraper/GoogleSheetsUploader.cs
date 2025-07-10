using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System.IO;
using System.Collections.Generic;

class GoogleSheetsUploader
{
    static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
    static readonly string SpreadsheetId = "1GZesYn5QZQHMYdxecpzPRhJ1eFNot2Dgds0dZi7XFSE";
    static readonly string SheetName = "Sheet1"; // Modify if needed
    static readonly string FilePath = @"C:\Users\ptige\Documents\FantasyWebApp\Scraper\WR Data.txt";

    static void Main()
    {
        var credential = GoogleCredential.FromFile("C:\\Users\\ptige\\OneDrive\\Documents\\Gunnar Notes\\fantasy-data-scraping-ef15cc5a444d.json")
            .CreateScoped(Scopes);

        var service = new SheetsService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "FantasyDataUploader"
        });

        var lines = File.ReadAllLines(FilePath);

        // Insert header row first
        var headers = new List<object>
        {
            "RK", "Name", "Team", "Pos", "GP", "TGTS", "Rec", "Catch%", "YDs", "TDS", "TD", "Long",
            "YDs/TGT", "YDs/Rec", "ATT", "YDs", "AVG", "TD", "FUM", "LOST", "FPTS/G", "FPTS"
        };

        var headerRange = new ValueRange { Values = new List<IList<object>> { headers } };

        var headerRequest = service.Spreadsheets.Values.Update(headerRange, SpreadsheetId, $"{SheetName}!A1");
        headerRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
        headerRequest.Execute();


        var rows = new List<IList<object>>();
        var currentRow = new List<object>();

        foreach (var line in File.ReadLines(FilePath))
        {
            var trimmed = line.Trim();
            if (string.IsNullOrWhiteSpace(trimmed)) continue;

            var fields = System.Text.RegularExpressions.Regex.Split(trimmed, @"\t"); // Split on multiple spaces

            // If line begins with a rank number, it's the start of a new player row
            if (int.TryParse(fields[0], out _))
            {
                if (currentRow.Count > 0)
                {
                    rows.Add(currentRow);
                    currentRow = new List<object>();
                }
            }

            foreach (var field in fields)
            {
                var value = field.Trim();
                currentRow.Add(string.IsNullOrEmpty(value) ? "0" : value);
            }
        }

        // Add the final row
        if (currentRow.Count > 0)
        {
            rows.Add(currentRow);
        }

        var dataRange = new ValueRange { Values = rows };

        var dataRequest = service.Spreadsheets.Values.Update(dataRange, SpreadsheetId, $"{SheetName}!A2");
        dataRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
        dataRequest.Execute();
    }
}

