using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.DATA.EF
{
    class ReservationSystemMetadata
    {
        [MetadataType(typeof(UserFamilyMemberMetadata))]
        public partial class UserFamilyMember { }
        public class UserFamilyMemberMetadata
        {
            [Required(ErrorMessage = "* Family Member Name is required")]
            [Display(Name = "Family Member Name")]
            public string MemberName { get; set; }

            public string Photo { get; set; }

            [Display(Name = "Notes")]
            [UIHint("MultilineText")]
            public string SpecialNotes { get; set; }

        }


        [MetadataType(typeof(RoomMetadata))]
        public partial class Room { }
        public class RoomMetadata
        {
            [Required(ErrorMessage = "* Room Type is required")]
            [Display(Name = "Room Type")]
            public string RoomType { get; set; }

            [Required(ErrorMessage = "* Room Description is required")]
            [Display(Name = "Room Description")]
            public string RoomDescription { get; set; }

            [Required(ErrorMessage = "* Price per night is required")]
            [Display(Name = "Price Per Night")]
            public double Price { get; set; }

            [Required(ErrorMessage = "* Max Occupancy is required")]
            [Display(Name = "Max Occupancy")]
            public byte MaxOccupancy { get; set; }

            [Display(Name = "Is Available")]
            public bool IsAvailable { get; set; }

            [Required(ErrorMessage = "* Room Photo is required")]
            public string Photo { get; set; }

            [Required(ErrorMessage = "* Rooms Available is required")]
            [Display(Name = "Number of Rooms Available")]
            public bool RoomsAvailable { get; set; }

            [Required(ErrorMessage = "* Rooms Taken is required")]
            [Display(Name = "Number of Rooms Taken")]
            public bool RoomsTaken { get; set; }
        }

        [MetadataType(typeof(ReservationMetadata))]
        public partial class Reservation{}

        public class ReservationMetadata
        {
            [DisplayFormat(DataFormatString = "{0:d}")]
            [Display(Name = "Check In")]
            [Required(ErrorMessage = "* Check In is required")]
            public System.DateTime ReservationStartDate { get; set; }

            [DisplayFormat(DataFormatString = "{0:d}")]
            [Display(Name = "Check Out")]
            [Required(ErrorMessage = "* Check Out is required")]
            public System.DateTime ReservationEndDate { get; set; }

            [Display(Name = "Room")]
            [Required(ErrorMessage = "* Room is required")]
            public int RoomId { get; set; }

            [Display(Name = "Number Of Guests")]
            [Required(ErrorMessage = "* Number of Guests is required")]
            public byte NumberOfGuests { get; set; }

            [DisplayFormat(DataFormatString = "{0:d}")]
            [Display(Name = "Data Added")]
            public System.DateTime DataAdded { get; set; }

            [Display(Name = "Is Active")]
            public bool IsActive { get; set; }

            [Display(Name = "Special Requests")]
            [StringLength(200, ErrorMessage = "* Maximum of 250 Characters")]
            public string SpecialRequests { get; set; }
        }

        [MetadataType(typeof(LocationMetadata))]
        public partial class Location { }

        public class LocationMetadata
        {
            [Display(Name = "Location")]
            public int LocationID { get; set; }

            [Display(Name = "Location")]
            [Required(ErrorMessage = "* Location name is required")]
            public string LocationName { get; set; }

            [Required(ErrorMessage = "* Address is required")]
            public string Address { get; set; }

            [Required(ErrorMessage = "* City is required")]
            public string City { get; set; }

            [Required(ErrorMessage = "* State is required")]
            public string State { get; set; }
        }
    }
}
