using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KeePark.Data.Enums;

namespace KeePark.ViewModels
{
    public class ParkingSpotCreate
    {
        [Key]
        public Guid ParkingSpotID { get; set; }
        [Required]
        [Display(Name = "Parking Name")]
        public string SpotName { get; set; }
        [Required]
        [Display(Name = "Parking Address")]
        public string Address { get; set; }
        [Required]
        [Display(Name = "Price")]
        [Range(0, 9999)]
        public int Price { get; set; }
        [Display(Name = "Describe Your Parking")]
        public string SpotDescription { get; set; }
        [Display(Name = "Parking Photo")]
        public IFormFile parkingPhoto { get; set; }
        [Display(Name = "Site Type")]
        public int SiteType { get; set; }
    }
}