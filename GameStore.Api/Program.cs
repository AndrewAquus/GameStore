using GameStore.Api.Entities;

const string GetGameEndpont = "GetName";

List<Game> games = new()
{
    new Game()
    {
        Id = 1,
        Name = "Street Flighter II",
        Genre = "Fighting",
        Price = 19.99m,
        ReleaseDate = new DateTime(1991, 2, 1),
        ImageUri = "https://placehold.co/128",
    },
    new Game()
    {
        Id = 2,
        Name = "Final Fantasy XIV",
        Genre = "Roleplaying",
        Price = 59.99m,
        ReleaseDate = new DateTime(2010, 9, 30),
        ImageUri = "https://placehold.co/128",
    },
    new Game()
    {
        Id = 3,
        Name = "Fifa 23",
        Genre = "Sports",
        Price = 69.99m,
        ReleaseDate = new DateTime(2022, 9, 27),
        ImageUri = "https://placehold.co/128",
    },
};

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var group = app.MapGroup("/games").WithParameterValidation();

group.MapGet("/", () => games);

group.MapGet("/{id}", (int id) =>
{
    Game? game = games.Find(game => game.Id == id);
    return game == null ? Results.NotFound() : Results.Ok(game);
})
.WithName(GetGameEndpont);

group.MapPost("/", (Game game) =>
{
    game.Id = games.Max(game => game.Id) + 1;
    games.Add(game);
    return Results.CreatedAtRoute(GetGameEndpont, new { id = game.Id }, game);
});

group.MapPut("/{id}", (int id, Game updatedGame) =>
{
    Game? existingGame = games.Find(game => game.Id == id);
    if (existingGame == null)
    {
        return Results.NotFound();
    }

    existingGame.Name = updatedGame.Name;
    existingGame.Genre = updatedGame.Genre;
    existingGame.Price = updatedGame.Price;
    existingGame.ReleaseDate = updatedGame.ReleaseDate;
    existingGame.ImageUri = updatedGame.ImageUri;

    return Results.NoContent();
});

group.MapDelete("/{id}", (int id) =>
{
    Game? existingGame = games.Find(game => game.Id == id);
    if (existingGame != null)
    {
        games.Remove(existingGame);
    }

    return Results.NoContent();

});

app.Run();
