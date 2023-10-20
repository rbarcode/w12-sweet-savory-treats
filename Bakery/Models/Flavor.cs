using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bakery.Models
{
  public class Flavor
  {
    public int FlavorId { get; set; }
    [Required]
    public string Type { get; set; }
    public List<FlavorTreat> JoinEntity { get; }
    public readonly string[] availableTypes = { "Bitter", "Salty", "Savory", "Sour", "Sweet"};
  }
}