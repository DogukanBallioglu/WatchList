
using System.ComponentModel.DataAnnotations;

namespace WatchList.Api.Models;

public class MediaItem
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = "";

    [Required]
    [RegularExpression("^(Movie|Series)$",
    ErrorMessage = "Type yalnızca Movie veya Series olabilir.")]
    public string Type { get; set; } = "Movie";

    public bool IsWatched { get; set; }

    [Range(1, 10, 
    ErrorMessage = "Rating 1 ile 10 arasında olmalıdır.")]
    public int? Rating { get; set; }
}
