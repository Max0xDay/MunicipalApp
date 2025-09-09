using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace MunicipalApp.Services
{
    public class MapboxService
    {
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://api.mapbox.com/geocoding/v5/mapbox.places/";
    private readonly MapboxOptions _options;

        public MapboxService(): this(null) {}

        public MapboxService(MapboxOptions? options)
        {
            _httpClient = new HttpClient();
            _options = options ?? LoadOptions();
        }

        private static MapboxOptions LoadOptions()
        {
            var config = ConfigurationHolder.Configuration;
            var opts = new MapboxOptions();
            if (config is not null)
            {
                var section = config.GetSection("Mapbox");
                if (section.Exists())
                {
                    opts.AccessToken = section["AccessToken"] ?? opts.AccessToken;
                    opts.CountryFilter = section["CountryFilter"] ?? opts.CountryFilter;
                    if (int.TryParse(section["ResultLimit"], out var rl)) opts.ResultLimit = rl;
                    opts.Types = section["Types"] ?? opts.Types;
                }
            }
            return opts;
        }

        public async Task<List<MapboxSuggestion>> GetLocationSuggestions(string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
                {
                    return new List<MapboxSuggestion>();
                }

                var encodedQuery = Uri.EscapeDataString(query);
                var country = string.IsNullOrWhiteSpace(_options.CountryFilter) ? "ZA" : _options.CountryFilter;
                var types = string.IsNullOrWhiteSpace(_options.Types) ? "place,locality,neighborhood,address,poi" : _options.Types;
                var limit = _options.ResultLimit <= 0 ? 5 : _options.ResultLimit;
                var token = string.IsNullOrWhiteSpace(_options.AccessToken) ? "MISSING_TOKEN" : _options.AccessToken;
                var url = $"{BaseUrl}{encodedQuery}.json?access_token={token}&limit={limit}&country={country}&types={types}";

                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<MapboxResponse>();
                    return result?.Features ?? new List<MapboxSuggestion>();
                }
            }
            catch (Exception)
            {
                // Log error or handle gracefully
            }

            return new List<MapboxSuggestion>();
        }
    }

    public class MapboxResponse
    {
        public List<MapboxSuggestion> Features { get; set; } = new();
    }

    public class MapboxSuggestion
    {
        [JsonPropertyName("id")] public string Id { get; set; } = string.Empty;
        [JsonPropertyName("type")] public string Type { get; set; } = string.Empty;
        [JsonPropertyName("geometry")] public MapboxGeometry Geometry { get; set; } = new();
        [JsonPropertyName("properties")] public MapboxProperties Properties { get; set; } = new();
        [JsonPropertyName("text")] public string Text { get; set; } = string.Empty;
        [JsonPropertyName("place_name")] public string PlaceName { get; set; } = string.Empty;
        [JsonPropertyName("context")] public object? Context { get; set; } // simplified
    }

    public class MapboxGeometry
    {
        public double[] Coordinates { get; set; } = Array.Empty<double>();
        public string Type { get; set; } = string.Empty;
    }

    public class MapboxProperties
    {
        public string? Address { get; set; }
        public string? Category { get; set; }
    }
}