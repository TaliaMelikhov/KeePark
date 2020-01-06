using KeePark.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KeePark.Data
{
    public class GeneralUser : IdentityUser
    {
        

        //Must added the key cause its the primary key which conect the IdentityUser to this General user
        [Key]
        [PersonalData]
        public string UID { get; set; }
        [PersonalData]
        public string FirstName { get; set; }
        [PersonalData]
        public string LastName { get; set; }
        [PersonalData]
        public string CreditCard { get; set; }
        [PersonalData]
        public string CarNumber { get; set; }
        [PersonalData]
        public string CarType { get; set; }
        [PersonalData]
        public string Address { get; set; }

        public double Balance { get; set; }

        public string History { get; set; }
    }
}
