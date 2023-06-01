using System;
using System.Collections.Generic;

#nullable disable

namespace BOs.Models
{
    public partial class Car
    {
        public Car()
        {
            Bookings = new HashSet<Booking>();
            ShowroomCars = new HashSet<ShowroomCar>();
        }

        public int CarId { get; set; }
        public int? Manufactory { get; set; }
        public int? CarName { get; set; }
        public string Version { get; set; }
        public int? CarModelYear { get; set; }
        public bool? Origin { get; set; }
        public int? NumberOfKilometersTraveled { get; set; }
        public int? Vehicles { get; set; }
        public long? Price { get; set; }
        public int? Color { get; set; }
        public int? ColorInside { get; set; }
        public byte? NumberOfDoors { get; set; }
        public byte? NumberOfSeats { get; set; }
        public bool? Gear { get; set; }
        public int? Drive { get; set; }
        public int? Fuel { get; set; }
        public bool? Notification { get; set; }
        public string Note { get; set; }
        public string FuelIntakeSystem { get; set; }
        public int? FuelConsumption { get; set; }
        public string Description { get; set; }
        public int? ImageCar { get; set; }
        public string Username { get; set; }
        public bool? Status { get; set; }

        public virtual CarModelYear CarModelYearNavigation { get; set; }
        public virtual CarName CarNameNavigation { get; set; }
        public virtual Color ColorInsideNavigation { get; set; }
        public virtual Color ColorNavigation { get; set; }
        public virtual Drife DriveNavigation { get; set; }
        public virtual Fuel FuelNavigation { get; set; }
        public virtual ImageCar ImageCarNavigation { get; set; }
        public virtual Manufactory ManufactoryNavigation { get; set; }
        public virtual User UsernameNavigation { get; set; }
        public virtual Vehicle VehiclesNavigation { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<ShowroomCar> ShowroomCars { get; set; }
    }
}
