using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PodTracker.Api.Data;
using PodTracker.Api.Dtos;
using PodTracker.Api.Models;
using Xunit;


public class DeckEndpointsTests : IDisposable
{
    private readonly PodTrackerWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public DeckEndpointsTests()
    {
        _factory = new PodTrackerWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    private Player SeedPlayer(string name = "Gui")
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<PodTrackerDbContext>();
        var player = new Player { Name = name };
        db.Players.Add(player);
        db.SaveChanges();
        return player;
    }

    private static CreateDeckRequest SampleDeck(int playerId) => new(
        Name: "Atraxa Counters",
        CommanderName: "Atraxa, Praetors' Voice",
        PlayerId: playerId
    );

    [Fact]
    public async Task GetDecks_WhenEmpty_ReturnsEmptyList()
    {
        var response = await _client.GetAsync("/decks");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var decks = await response.Content.ReadFromJsonAsync<List<DeckResponse>>();

        decks.Should().NotBeNull();
        decks.Should().BeEmpty();
    }

    [Fact]
    public async Task PostDeck_CreatesDeckSuccessfully()
    {
        var player = SeedPlayer();

        var response = await _client.PostAsJsonAsync("/decks", SampleDeck(player.Id));

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task DeleteDeck_DeletesDeckSuccessfully()
    {
        var player = SeedPlayer();
        await _client.PostAsJsonAsync("/decks", SampleDeck(player.Id));

        var decks = await _client.GetFromJsonAsync<List<DeckResponse>>("/decks");
        var responseDelete = await _client.DeleteAsync($"/decks/{decks!.First().Id}");

        responseDelete.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task PutDeck_UpdatesDeckSuccessfully()
    {
        var player = SeedPlayer();
        await _client.PostAsJsonAsync("/decks", SampleDeck(player.Id));
        var decks = await _client.GetFromJsonAsync<List<DeckResponse>>("/decks");
        var deck = decks!.First();

        var updateRequest = new CreateDeckRequest(
            Name: "Atraxa Superfriends",
            CommanderName: "Atraxa, Grand Unifier",
            PlayerId: deck.PlayerId
        );

        var responsePut = await _client.PutAsJsonAsync($"/decks/{deck.Id}", updateRequest);
        responsePut.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var updatedDeck = await _client.GetFromJsonAsync<DeckResponse>($"/decks/{deck.Id}");
        updatedDeck.Should().NotBeNull();
        updatedDeck!.Name.Should().Be("Atraxa Superfriends");
        updatedDeck.CommanderName.Should().Be("Atraxa, Grand Unifier");
        updatedDeck.PlayerId.Should().Be(deck.PlayerId);
    }

    [Fact]
    public async Task GetDeck_ReturnsDeckSuccessfully()
    {
        var player = SeedPlayer();
        await _client.PostAsJsonAsync("/decks", SampleDeck(player.Id));
        var decks = await _client.GetFromJsonAsync<List<DeckResponse>>("/decks");
        var deck = decks!.First();

        var responseGet = await _client.GetAsync($"/decks/{deck.Id}");
        responseGet.StatusCode.Should().Be(HttpStatusCode.OK);

        var retrievedDeck = await responseGet.Content.ReadFromJsonAsync<DeckResponse>();
        retrievedDeck.Should().NotBeNull();
        retrievedDeck!.Name.Should().Be("Atraxa Counters");
        retrievedDeck.CommanderName.Should().Be("Atraxa, Praetors' Voice");
        retrievedDeck.PlayerId.Should().Be(deck.PlayerId);
    }
}