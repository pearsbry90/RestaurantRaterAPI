using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RestaurantRaterAPI.Models
{
    public class Restaurant{
        [Key]
        public int Id {get;set;}
        [Required]
        [MaxLength(100)]
        public string Name {get;set;}
        [Required]
        [MaxLength(100)]
        public string Location {get;set;}
        public virtual List<Rating> Ratings {get; set;} = new List<Rating>();

    }
}