using System.Text;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using UserManagement.Configuration;
using UserManagement.Controllers.v1.Users;
using UserManagement.Services.Users;

namespace UserManagement.IntegrationTests;

public abstract class BaseSetup
{
    private const string BaseUrl = "http://localhost:5000";

    public CustomWebApplicationFactory Factory { get; private set; }

    protected HttpClient Client { get; private set; }

    private IOptions<DatabaseConfiguration> Options;

    [OneTimeSetUp]
    public void Setup()
    {
        Factory = new CustomWebApplicationFactory();
        Client = Factory.CreateClient();

        Options = Factory.Server.Services.GetRequiredService<IOptions<DatabaseConfiguration>>();
    }

    [SetUp]
    public async Task BeforeEach()
    {
        using var connection = new SqlConnection(Options.Value.ConnectionString);

        await connection.ExecuteAsync(@"DELETE FROM Users;");
    }

    [OneTimeTearDown]
    public void Dispose()
    {
        Factory?.Dispose();
        Client?.Dispose();
    }

    protected async Task<User> CreateUser(CreateUserApiRequest request)
    {
        var result = await Post<CreateUserApiRequest, User>($"{BaseUrl}/api/v1/users", request);

        if (!result.Message.IsSuccessStatusCode)
        {
            var reason = await result.Message.Content.ReadAsStringAsync();
            throw new Exception(result.Message.StatusCode.ToString(), new Exception(reason));
        }

        return result.Response!;
    }

    protected async Task DeleteUser(Guid id)
    {
        var result = await Delete($"{BaseUrl}/api/v1/users/{id}");

        if (!result.IsSuccessStatusCode)
        {
            var reason = await result.Content.ReadAsStringAsync();
            throw new Exception(result.StatusCode.ToString(), new Exception(reason));
        }
    }

    protected async Task<User> GetUserById(Guid id)
    {
        var result = await Get<User>($"{BaseUrl}/api/v1/users/{id}");

        if (!result.Message.IsSuccessStatusCode)
        {
            var reason = await result.Message.Content.ReadAsStringAsync();
            throw new Exception(result.Message.StatusCode.ToString(), new Exception(reason));
        }

        return result.Response!;
    }

    protected async Task UpdateUser(Guid id, UpdateUserApiRequest request)
    {
        var result = await Put<UpdateUserApiRequest>($"{BaseUrl}/api/v1/users/{id}", request);

        if (!result.IsSuccessStatusCode)
        {
            var reason = await result.Content.ReadAsStringAsync();
            throw new Exception(result.StatusCode.ToString(), new Exception(reason));
        }
    }



    private async Task<(TResponse? Response, HttpResponseMessage Message)> Get<TResponse>(string endpoint) where TResponse : class
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, new Uri(endpoint));

        var response = await Client.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            return (null, response);
        }

        var content = await response.Content.ReadAsStringAsync();

        return (JsonConvert.DeserializeObject<TResponse>(content), response);
    }

    private async Task<(TResponse? Response, HttpResponseMessage Message)> Post<TRequest, TResponse>(
        string endpoint,
        TRequest request)
        where TRequest : class
        where TResponse : class
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(endpoint));

        httpRequest.Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        var response = await Client.SendAsync(httpRequest);

        if (!response.IsSuccessStatusCode)
        {
            return (null, response);
        }

        var content = await response.Content.ReadAsStringAsync();

        return (JsonConvert.DeserializeObject<TResponse>(content), response);
    }

    private async Task<HttpResponseMessage> Put<TRequest>(string endpoint, TRequest request)
        where TRequest : class
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Put, new Uri(endpoint));

        httpRequest.Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        var response = await Client.SendAsync(httpRequest);

        return response;
    }

    private async Task<HttpResponseMessage> Delete(string endpoint)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Delete, new Uri(endpoint));

        return await Client.SendAsync(httpRequest);
    }
}