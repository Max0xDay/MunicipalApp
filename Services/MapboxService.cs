using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace MunicipalApp.Services
{
    public class MapboxService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://api.mapbox.com/geocoding/v5/mapbox.places/";
        private const string AccessToken = "pk.eyJ1IjoibWF4MHhkYXl2YyIsImEiOiJjbWVncTFkOXMxNmUwMmxzNGVnenRwaHRjIn0.h6CK_o-YpmYsimTqV1S2_Q";

        public MapboxService()
        {
            _httpClient = new HttpClient();
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
                var url = $"{BaseUrl}{encodedQuery}.json?access_token={AccessToken}&limit=5&country=ZA&types=place,locality,neighborhood,address,poi";

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
        public string Id { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public MapboxGeometry Geometry { get; set; } = new();
        public MapboxProperties Properties { get; set; } = new();
        public string Text { get; set; } = string.Empty;
        public string Place_Name { get; set; } = string.Empty;
        public string[] Context { get; set; } = Array.Empty<string>();
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