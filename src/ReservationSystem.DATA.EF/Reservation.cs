//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ReservationSystem.DATA.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class Reservation
    {
        public int ReservationId { get; set; }
        public string CustomerId { get; set; }
        public System.DateTime ReservationDate { get; set; }
        public int ReservationLength { get; set; }
        public int RoomId { get; set; }
        public byte NumberOfGuests { get; set; }
        public System.DateTime DateAdded { get; set; }
        public int IsActive { get; set; }
        public string SpecialRequests { get; set; }
    
        public virtual Room Room { get; set; }
        public virtual UserDetail UserDetail { get; set; }
    }
}