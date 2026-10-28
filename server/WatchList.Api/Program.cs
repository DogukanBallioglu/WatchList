using WatchList.Api.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using WatchList.Api.Data;

var builder = WebApplication.CreateBuilder(args);

List<MediaItem> movies = new List<MediaItem>();
var yeniFilm = new MediaItem
{
    Id = 1,
    Title = "Inception",
    Type = "Movies",
    IsWatched = false,
    Rating = 7,
};

var yeniFilm2 = new MediaItem
{
    Id = 2,
    Title = "Interstaller",
    Type = "Movies",
    IsWatched = true,
    Rating = 9,
};

movies.Add(yeniFilm);
movies.Add(yeniFilm2);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddDbContext<WatchListContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Filmleri döndürür.
app.MapGet("/api/movies", () =>
{
    return movies;
});

// IsWatched = True olan filmleri döndürür.
app.MapGet("/api/movies/watched", () =>
{

    var watchedMovies = movies.Where(movies => movies.IsWatched);
    return watchedMovies;
});

// Yeni film ekler.
app.MapPost("/api/movies", (MediaItem newItem) =>
{

    var validationContext = new ValidationContext(newItem);
    var validationResults = new List<ValidationResult>();

    bool isValid = Validator.TryValidateObject(
        newItem,
        validationContext,
        validationResults,
        validateAllProperties: true
    );

    if (!isValid)
    {
        return Results.ValidationProblem(
            validationResults
            .GroupBy(result => result.MemberNames.FirstOrDefault() ?? "")
            .ToDictionary(
                group => group.Key,
                group => group.Select(result => result.ErrorMessage ?? "Geçersiz değer").ToArray()
            )
        );
    }


    movies.Add(newItem);
    return Results.Ok(newItem);
});

// Filmlerin ID olarak detayını döndürür.
app.MapGet("/api/movies/{id}", (int id) =>
{

    var eslesenFilm = movies.FirstOrDefault(movie => movie.Id == id);

    if (eslesenFilm == null) return Results.NotFound();

    return Results.Ok(eslesenFilm);
});

// Seçilen ID'li filmi günceller.
app.MapPut("/api/movies/{id}", (int id, MediaItem updatedItem) =>
{

    var selectedMovie = movies.FirstOrDefault(movie => movie.Id == id);

    if (selectedMovie == null) return Results.NotFound();

    selectedMovie.Title = updatedItem.Title;
    selectedMovie.Type = updatedItem.Type;
    selectedMovie.IsWatched = updatedItem.IsWatched;
    selectedMovie.Rating = updatedItem.Rating;

    return Results.Ok(selectedMovie);

});

// Seçilen ID'li filmi siler.
app.MapDelete("/api/movies/{id}", (int id) =>
{

    var selectedMovie = movies.FirstOrDefault(movie => movie.Id == id);

    if (selectedMovie == null) return Results.NotFound();

    movies.Remove(selectedMovie);

    return Results.NoContent();
});


app.Run();


record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
