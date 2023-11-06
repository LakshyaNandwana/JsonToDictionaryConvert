using System;
using System.Collections.Generic;
using System.Text.Json;

class Program
{
    static void Main()
    {
        string json = @"{""id"":""10d1df3d-0654-46b9-9fa3-1f9362344257"",""type"":""ExampleCloudEvent"",""time"":""2023-11-05T16:31:05.2603311+05:30"",""datacontenttype"":""application/json"",""source"":""urn:example.helloWorld"",""specVersion"":""1.0"",""data"":{""Temperature"":41,""Time"":""2023-11-05T16:31:05+05:30""}}";
        var keyValuePairs = ParseJsonToKeyValuePairs(json);

        Console.WriteLine("Parsed value= " + keyValuePairs);

        foreach (var kvp in keyValuePairs)
        {
            Console.WriteLine($"{ kvp.Key}: {kvp.Value}");
        }
    }

    static Dictionary<string, string> ParseJsonToKeyValuePairs(string json)
    {
        var keyValuePairs = new Dictionary<string, string>();

        using (JsonDocument document = JsonDocument.Parse(json))
        {
            ParseJsonObject(document.RootElement, keyValuePairs);
        }

        return keyValuePairs;
    }

    static void ParseJsonObject(JsonElement jsonElement, Dictionary<string, string> keyValuePairs, string parentKey = "")
    {
        foreach (var property in jsonElement.EnumerateObject())
        {
            string key = string.IsNullOrEmpty(parentKey) ? property.Name : $"{parentKey}.{property.Name}";

            if (property.Value.ValueKind == JsonValueKind.Object)
            {
                ParseJsonObject(property.Value, keyValuePairs, key);
            } else if (property.Value.ValueKind == JsonValueKind.Array)
            {
                // Handle arrays if needed
            } else
            {
                keyValuePairs[key] = property.Value.ToString();
            }
        }
    }
}
