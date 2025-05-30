using System.Globalization;
using System.Text.Json.Nodes;
using System.Xml.Linq;
using CsvHelper;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace DataParsingServer;

public static class DataParsingService
{
    public static List<Product> ParseJson(string filePath)
    {
        System.Console.WriteLine("Hello From C# server");
        var json = File.ReadAllText(filePath);
        var products = ((JsonArray)JsonNode.Parse(json)!)
            .Select(p => new Product
            {
                Id = int.TryParse(p["id"]?.ToString(), out var id) ? id : 0,
                Name = p["name"]?.ToString() ?? "Unknown",
                Price = double.TryParse(p["price"]?.ToString(), out var price) ? price : 0.0,
            })
            .ToList();

        return products;
    }

    public static List<Product> ParseXml(string filePath)
    {
        System.Console.WriteLine("Hello From C# server");
        var doc = XDocument.Load(filePath);
        var products = doc.Descendants("Product")
            .Select(p => new Product
            {
                Id = int.TryParse(p.Element("id")?.Value, out var id) ? id : 0,
                Name = p.Element("name")?.Value ?? "Unknown",
                Price = double.TryParse(p.Element("price")?.Value, out var price) ? price : 0.0,
            })
            .ToList();
        return products;
    }

    public static List<Product> ParseYaml(string filePath)
    {
        System.Console.WriteLine("Hello From C# server");
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var yamlContent = File.ReadAllText(filePath);
        var root = deserializer.Deserialize<Dictionary<string, List<Product>>>(yamlContent);
        var products = root["products"];
        return products;
    }

    public static List<Product> ParseCsv(string filePath)
    {
        System.Console.WriteLine("Hello From C# server");
        var lines = File.ReadAllLines(filePath);
        var products = lines.Skip(1)
            .Select(line =>
            {
                var parts = line.Split(',');
                return new Product
                {
                    Id = int.TryParse(parts[0], out var id) ? id : 0,
                    Name = parts[1],
                    Price = double.TryParse(parts[2], out var price) ? price : 0.0,
                };
            })
            .ToList();

        return products;
    }

    public static List<Product> ParseText(string filePath)
    {
        System.Console.WriteLine("Hello From C# server");
        var lines = File.ReadAllLines(filePath);
        var products = new List<Product>();
        Product currentProduct = null;

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                if (currentProduct != null)
                {
                    products.Add(currentProduct);
                    currentProduct = null;
                }
                continue;
            }

            if (currentProduct == null)
            {
                currentProduct = new Product();
            }

            var parts = line.Split('=');
            if (parts.Length != 2) continue;

            var key = parts[0].Trim().ToLower();
            var value = parts[1].Trim();

            switch (key)
            {
                case "id":
                    currentProduct.Id = int.TryParse(value, out var id) ? id : 0;
                    break;
                case "name":
                    currentProduct.Name = value;
                    break;
                case "price":
                    currentProduct.Price = double.TryParse(value, out var price) ? price : 0.0;
                    break;
            }
        }

        if (currentProduct != null)
        {
            products.Add(currentProduct);
        }

        return products;
    }

    public static void LogProducts(string format, List<Product> products)
    {
        Console.WriteLine($"\n--- {format.ToUpper()} Data ---");
        products.ForEach(p =>
            Console.WriteLine($"ID: {p.Id}, Name: {p.Name}, Price: {p.Price}")
        );
    }
}
