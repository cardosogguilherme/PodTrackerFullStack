using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PodTracker.Api.Data;
using PodTracker.Api.Dtos;
using PodTracker.Api.Models;

public class PlayerEndpointTests : IDisposable
{
    private readonly PodTrackerWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public PlayerEndpointTests()
    {
        _factory = new PodTrackerWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    private Player SeedPlayer(string name = "Gui")
    {
        using var scope = _factory.Services.CreateScope();
        var player = new Player { Name = name };
        return player;
    }

    [Fact]
    public async Task GetPlayers_WhenEmpty_ReturnsEmptyList()
    {
        var response = await _client.GetAsync("/players");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var players = await response.Content.ReadFromJsonAsync<List<PlayerResponse>>();

        players.Should().NotBeNull();
        players.Should().BeEmpty();
    }

    [Fact]
    public async Task PostPlayer_CreatesPlayerSuccessfully()
    {
        var player = SeedPlayer();
        var response = await _client.PostAsJsonAsync("/players", player);
        var created = await response.Content.ReadFromJsonAsync<PlayerResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        created!.Name.Should().Be("Gui");
    }

    [Fact]
    public async Task DeletePlayer_DeletesDeckSuccessfully()
    {
        var player = SeedPlayer();
        var response = await _client.PostAsJsonAsync("/players", player);
        var created = await response.Content.ReadFromJsonAsync<PlayerResponse>();

        var responseDelete = await _client.DeleteAsync($"/players/{created!.Id}");

        responseDelete.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task PutPlayer_UpdatesDeckSuccessfully()
    {
        var response = await _client.PostAsJsonAsync("/players", new CreatePlayerRequest
        (
            Name: "Gui Test"
        ));
        var created = await response.Content.ReadFromJsonAsync<PlayerResponse>();
        created!.Name.Should().Be("Gui Test");

        var updateRequest = new UpdatePlayerRequest(
            Id: created!.Id,
            Name: "Updated player"
        );

        var responsePut = await _client.PutAsJsonAsync($"/players/{created.Id}", updateRequest);
        responsePut.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var updatedPlayer = await _client.GetFromJsonAsync<PlayerResponse>($"/players/{created.Id}");
        updatedPlayer.Should().NotBeNull();
        updatedPlayer!.Name.Should().Be("Updated player");
        updatedPlayer.Id.Should().Be(created.Id);
    }

    [Fact]
    public async Task GetPlayer_WhenNotFound_Returns404()
    {
        var response = await _client.GetAsync("/players/999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PutPlayer_WhenNotFound_Returns404()
    {
        var response = await _client.PutAsJsonAsync("/players/999", SeedPlayer());
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeletePlayer_WhenNotFound_Returns404()
    {
        var response = await _client.DeleteAsync("/players/999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
    }
}