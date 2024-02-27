namespace MvcMovie.Models;

public class Studio
{
    public int StudioId { get; set; }
    public string? Name { get; set;}
    public string? Coutry {get;set;}
    public ICollection<Movie>? Movies { get; set;}
}