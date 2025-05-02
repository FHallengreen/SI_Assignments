using DataParsingServer;

namespace DataParsing;

public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: dotnet run <format>");
            Console.WriteLine("Supported formats: json, xml, yaml, csv, text");
            return;
        }

        var format = args[0].ToLowerInvariant();

        var filePath = $"../../../datasets/product.{format}";

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"File not found: {filePath}");
            return;
        }

        var products = format switch
        {
            "json" => DataParsingService.ParseJson(filePath),
            "xml" => DataParsingService.ParseXml(filePath),
            "yaml" => DataParsingService.ParseYaml(filePath),
            "csv" => DataParsingService.ParseCsv(filePath),
            "text" => DataParsingService.ParseText(filePath),
            _ => throw new InvalidOperationException($"Unexpected format: {format}")
        };

        DataParsingService.LogProducts(format, products);
    }
}
