using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        string url = "https://fantasydata.com/NFL_FantasyStats/PlayerSeasonStats?season=2024&scope=1&position=WR&scoring=1";
        string sheetId = "1GZesYn5QZQHMYdxecpzPRhJ1eFNot2Dgds0dZi7XFSE"; // Replace with your actual Google Sheet ID
        string serviceAccountFile = "C:\\Users\\ptige\\OneDrive\\Documents\\Gunnar Notes\\fantasy-data-scraping-ef15cc5a444d.json"; // Replace with your JSON key path

        var tableData = await FetchJsonTableAsync(url);
        await WriteToGoogleSheetsAsync(tableData, sheetId, serviceAccountFile);
    }

    static async Task<List<IList<object>>> FetchJsonTableAsync(string url)
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");

        var json = await client.GetStringAsync(url);
        var players = JArray.Parse(json);

        var data = new List<IList<object>>();

        // Add headers (optional)
        data.Add(new List<object> {
            "Name", "Team", "Position", "Games", "Targets", "Receptions", "Yards", "Touchdowns", "Fantasy Points (PPR)"
        });

        foreach (var player in players)
        {
            data.Add(new List<object>
            {
                player["Name"],
                player["Team"],
                player["Position"],
                player["Played"],
                player["Targets"],
                player["Receptions"],
                player["ReceivingYards"],
                player["ReceivingTouchdowns"],
                player["FantasyPointsPPR"]
            });
        }

        return data;
    }

    static async Task WriteToGoogleSheetsAsync(List<IList<object>> values, string spreadsheetId, string serviceAccountFile)
    {
        var credential = GoogleCredential.FromFile(serviceAccountFile).CreateScoped(SheetsService.Scope.Spreadsheets);

        var service = new SheetsService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "FantasyDataScraper",
        });

        var range = "Sheet1!A1";
        var valueRange = new ValueRange
        {
            Values = values
        };

        var updateRequest = service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
        updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;

        await updateRequest.ExecuteAsync();
        Console.WriteLine("✅ Data written to Google Sheets.");
    }
}
