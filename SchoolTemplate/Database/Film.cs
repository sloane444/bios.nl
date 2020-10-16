using System;

namespace SchoolTemplate.Database
{
  public class Film
  {
    public int Id { get; set; }

    [Required]
    public string Naam { get; set; }
    [Required]
    [EmailAddress]
    public string Beschrijving { get; set; }    

    public DateTime Datum { get; set; }

  }
}
