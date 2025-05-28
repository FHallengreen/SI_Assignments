using Microsoft.AspNetCore.Mvc;

namespace DataParsingServer
{
    [ApiController]
    [Route("api/data")]
    public class DataController : ControllerBase
    {
        private const string BasePath = "../../../datasets/product.";
        private const string PYTHON_SERVER_URL = "http://localhost:8000";
        private static readonly HttpClient HttpClient = new();

        private async Task<IActionResult> FetchFromPythonServer(string format)
        {
            var response = await HttpClient.GetAsync($"{PYTHON_SERVER_URL}/local/{format}");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Failed to fetch data from Python server.");
            }
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        [HttpGet("json")]
        public async Task<IActionResult> GetJsonData() => await FetchFromPythonServer("json");

        [HttpGet("xml")]
        public async Task<IActionResult> GetXmlData() => await FetchFromPythonServer("xml");

        [HttpGet("yaml")]
        public async Task<IActionResult> GetYamlData() => await FetchFromPythonServer("yaml");

        [HttpGet("csv")]
        public async Task<IActionResult> GetCsvData() => await FetchFromPythonServer("csv");

        [HttpGet("txt")]
        public async Task<IActionResult> GetTextData() => await FetchFromPythonServer("txt");

        [HttpGet("local/json")]
        public IActionResult GetLocalJsonData()
        {
            var products = DataParsingService.ParseJson(BasePath + "json");
            return Ok(products);
        }

        [HttpGet("local/xml")]
        public IActionResult GetLocalXmlData()
        {
            var products = DataParsingService.ParseXml(BasePath + "xml");
            return Ok(products);
        }

        [HttpGet("local/yaml")]
        public IActionResult GetLocalYamlData()
        {
            var products = DataParsingService.ParseYaml(BasePath + "yaml");
            return Ok(products);
        }

        [HttpGet("local/csv")]
        public IActionResult GetLocalCsvData()
        {
            var products = DataParsingService.ParseCsv(BasePath + "csv");
            return Ok(products);
        }

        [HttpGet("local/txt")]
        public IActionResult GetLocalTextData()
        {
            var products = DataParsingService.ParseText(BasePath + "txt");
            return Ok(products);
        }
    }
}
