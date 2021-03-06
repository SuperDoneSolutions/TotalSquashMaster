//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TotalSquashNext.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Country
    {
        public Country()
        {
            this.Provinces = new HashSet<Province>();
            this.Users = new HashSet<User>();
        }
    
        [Display(Name = "Country ID")]
        public int countryId { get; set; }
        [Display(Name = "Country Name")]
        public string countryName { get; set; }
    
        public virtual ICollection<Province> Provinces { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
