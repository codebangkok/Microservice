using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json;

namespace Frontend.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        IHttpClientFactory _clientFactory;
        public List<WeatherForecast> Forecasts { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public async Task OnGet()
        {
            var option = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var client = _clientFactory.CreateClient("backend");

            var response = await client.GetAsync("/weatherforecast");
            if (response.IsSuccessStatusCode) {
                var stream = await response.Content.ReadAsStreamAsync();
                Forecasts = await JsonSerializer.DeserializeAsync<List<WeatherForecast>>(stream, option);
            }
        }
    }
}
