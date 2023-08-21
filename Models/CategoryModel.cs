using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CarvedRock.Admin.Models;

public class CategoryModel
{
  public int Id { get; set; }
  [Required]
  public string Name { get; set; }
}
