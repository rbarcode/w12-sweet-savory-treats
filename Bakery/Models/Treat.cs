using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bakery.Models
{
  public class Treat
  {
    public int TreatId { get; set; }
    [Required]
    public string Type { get; set; }
    public List<FlavorTreat> JoinEntity { get; }
    public ApplicationUser User { get; }
    public readonly string[] availableTypes = { "Bagel", "Cheesecake", "Cookie", "Croissant", "Cupcake", "Muffin", "Roll"};
  }
}