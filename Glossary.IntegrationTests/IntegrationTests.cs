using Auth0.AuthenticationApi.Models;
using Auth0.AuthenticationApi;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http;

namespace Glossary.IntegrationTests;

public class IntegrationTests : IClassFixture<WebApplicationFactory<Glossary.Startup>>
{

    private readonly HttpClient _httpClient;
    private readonly IConfigurationSection auth0Settings;

    public IntegrationTests(WebApplicationFactory<Glossary.Startup> factory)
    {
        _httpClient = factory.CreateClient();
        auth0Settings = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettings.json")
         .Build()
         .GetSection("Auth0");
    }

    [Fact]
    public async Task GetGlossaryList()
    {
        // Act
        var response = await _httpClient.GetAsync("api/glossary");

        // Assert
        response.EnsureSuccessStatusCode();
        var stringResponse = await response.Content.ReadAsStringAsync();
        var terms = JsonSerializer.Deserialize<List<GlossaryItem>>(stringResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

       
        Assert.Equal(3, terms.Count);
        Assert.Contains(terms, t => t.Term == "Access Token");
        Assert.Contains(terms, t => t.Term == "JWT");
        Assert.Contains(terms, t => t.Term == "OpenID");
    }

    [Fact]
    public async Task AddTermWithoutAuthorization()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Post, "api/glossary");

        request.Content = new StringContent(JsonSerializer.Serialize(new
        {
            term = "MFA",
            definition = "An authentication process that considers multiple factors."
        }), Encoding.UTF8, "application/json");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "This is an invalid token");


        // Act
        var response = await _httpClient.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task AddTermWithAuthorization()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Post, "api/glossary");

        request.Content = new StringContent(JsonSerializer.Serialize(new
        {
            term = "MFA",
            definition = "An authentication process that considers multiple factors."
        }), Encoding.UTF8, "application/json");

        var accessToken = await GetAccessToken();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        // Act
        var response = await _httpClient.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal("/api/glossary\\MFA", Uri.UnescapeDataString(response.Headers.GetValues("Location").FirstOrDefault()));
    }


    private async Task<string> GetAccessToken()
    {
        var auth0Client = new AuthenticationApiClient(auth0Settings["Domain"]);
        var tokenRequest = new ClientCredentialsTokenRequest()
        {
            ClientId = auth0Settings["ClientId"],
            ClientSecret = auth0Settings["ClientSecret"],
            Audience = auth0Settings["Audience"]
        };
        var tokenResponse = await auth0Client.GetTokenAsync(tokenRequest);

        return tokenResponse.AccessToken;
    }


}