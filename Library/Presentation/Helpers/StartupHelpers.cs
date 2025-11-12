namespace Presentation.Helpers;

using System.Text.Json;

public static class StartupHelpers
{
    public static async Task<string> AskForJsonFileAsync()
    {
        while (true)
        {
            Console.Write("Enter path to a JSON file (existing or new): ");
            var input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                MenuHelpers.Warn("You must specify a JSON file path.");
                continue;
            }

            if (!input.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            {
                MenuHelpers.Warn("File must have '.json' extension. Try again.");
                continue;
            }

            if (File.Exists(input))
            {
                Console.WriteLine("File found — checking JSON validity...");

                try
                {
                    await using var stream = File.OpenRead(input);
                    await JsonSerializer.DeserializeAsync<object>(stream);

                    MenuHelpers.Success($"Existing valid JSON file '{input}' opened.");
                    MenuHelpers.ContinuePrompt();
                }
                catch (JsonException)
                {
                    MenuHelpers.Error($"File '{input}' contains invalid JSON.");
                    continue;
                }
            }
            else
            {
                MenuHelpers.Warn($"File '{input}' not found — will be created as new.");
                MenuHelpers.ContinuePrompt();
            }

            return input;
        }
    }
}