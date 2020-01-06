using KeePark.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KeePark.Models
{
    public class ParkingSpot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ParkingSpotID { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Parking Name")]
        public string SpotName { get; set; }
        public string OwnerID { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Parking Address")]
        public string Address { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Price")]
        public int Price { get; set; }
        public int NunOfOrders { get; set; }
        public string filePath { get; set; }
        [Display(Name = "Describe Your Parking")]
        public string SpotDescription { get; set; }

        public ICollection<ReserveSpot> SpotReservations { get; set; }
        public int SiteType { get; set; }
    }
}