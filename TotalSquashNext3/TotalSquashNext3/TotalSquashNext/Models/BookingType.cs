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
    
    public partial class BookingType
    {
        public BookingType()
        {
            this.Bookings = new HashSet<Booking>();
        }
    
        public int bookingCode { get; set; }
        public string description { get; set; }
    
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
