using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace BOs.Models
{
    public partial class OldCarShowroomNetworkContext : DbContext
    {
        public string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", true, true).Build();
            var strConn = config["ConnectionStrings:DB"];
            return strConn;
        }
        public OldCarShowroomNetworkContext()
        {
        }

        public OldCarShowroomNetworkContext(DbContextOptions<OldCarShowroomNetworkContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<CarModelYear> CarModelYears { get; set; }
        public virtual DbSet<CarName> CarNames { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Color> Colors { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<Drife> Drives { get; set; }
        public virtual DbSet<Fuel> Fuels { get; set; }
        public virtual DbSet<ImageCar> ImageCars { get; set; }
        public virtual DbSet<ImageShowroom> ImageShowrooms { get; set; }
        public virtual DbSet<Manufactory> Manufactorys { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Showroom> Showrooms { get; set; }
        public virtual DbSet<Slot> Slots { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }
        public virtual DbSet<Ward> Wards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Vietnamese_CI_AS");

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => new { e.Username, e.CarId, e.Slot })
                    .HasName("PK__Bookings__E25AFD0C9FA82D61");

                entity.Property(e => e.Username)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.CarId).HasColumnName("CarID");

                entity.Property(e => e.DayBooking).HasColumnType("date");

                entity.Property(e => e.Note).HasMaxLength(2000);

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.CarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Bookings__CarID__5812160E");

                entity.HasOne(d => d.SlotNavigation)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.Slot)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Bookings__Slot__59FA5E80");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Bookings__Userna__59063A47");
            });

            modelBuilder.Entity<Car>(entity =>
            {
                entity.Property(e => e.CarId).HasColumnName("CarID");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.FuelIntakeSystem).HasMaxLength(1000);

                entity.Property(e => e.Note).HasMaxLength(2000);

                entity.Property(e => e.ShowroomId).HasColumnName("ShowroomID");

                entity.Property(e => e.Username)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Version).HasMaxLength(150);

                entity.HasOne(d => d.CarModelYearNavigation)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.CarModelYear)
                    .HasConstraintName("FK__Cars__CarModelYe__5BE2A6F2");

                entity.HasOne(d => d.CarNameNavigation)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.CarName)
                    .HasConstraintName("FK__Cars__CarName__5CD6CB2B");

                entity.HasOne(d => d.ColorNavigation)
                    .WithMany(p => p.CarColorNavigations)
                    .HasForeignKey(d => d.Color)
                    .HasConstraintName("FK__Cars__Color__5DCAEF64");

                entity.HasOne(d => d.ColorInsideNavigation)
                    .WithMany(p => p.CarColorInsideNavigations)
                    .HasForeignKey(d => d.ColorInside)
                    .HasConstraintName("FK__Cars__ColorInsid__5EBF139D");

                entity.HasOne(d => d.DriveNavigation)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.Drive)
                    .HasConstraintName("FK__Cars__Drive__5FB337D6");

                entity.HasOne(d => d.FuelNavigation)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.Fuel)
                    .HasConstraintName("FK__Cars__Fuel__60A75C0F");

                entity.HasOne(d => d.ManufactoryNavigation)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.Manufactory)
                    .HasConstraintName("FK__Cars__Manufactor__619B8048");

                entity.HasOne(d => d.Showroom)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.ShowroomId)
                    .HasConstraintName("FK_Cars_Showrooms");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.Username)
                    .HasConstraintName("FK__Cars__Username__628FA481");

                entity.HasOne(d => d.VehiclesNavigation)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.Vehicles)
                    .HasConstraintName("FK__Cars__Vehicles__6383C8BA");
            });

            modelBuilder.Entity<CarModelYear>(entity =>
            {
                entity.Property(e => e.CarModelYearId).HasColumnName("CarModelYearID");

                entity.Property(e => e.CarModelYear1).HasColumnName("CarModelYear");
            });

            modelBuilder.Entity<CarName>(entity =>
            {
                entity.Property(e => e.CarNameId).HasColumnName("CarNameID");

                entity.Property(e => e.CarName1)
                    .HasMaxLength(50)
                    .HasColumnName("CarName");

                entity.Property(e => e.ManufactoryId).HasColumnName("ManufactoryID");

                entity.HasOne(d => d.Manufactory)
                    .WithMany(p => p.CarNames)
                    .HasForeignKey(d => d.ManufactoryId)
                    .HasConstraintName("FK__CarNames__Manufa__5AEE82B9");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("city");

                entity.Property(e => e.CityId)
                    .HasMaxLength(20)
                    .HasColumnName("city_id");

                entity.Property(e => e.AdministrativeRegionId).HasColumnName("administrative_region_id");

                entity.Property(e => e.AdministrativeUnitId).HasColumnName("administrative_unit_id");

                entity.Property(e => e.CodeName)
                    .HasMaxLength(255)
                    .HasColumnName("code_name");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("full_name");

                entity.Property(e => e.FullNameEn)
                    .HasMaxLength(255)
                    .HasColumnName("full_name_en");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.NameEn)
                    .HasMaxLength(255)
                    .HasColumnName("name_en");
            });

            modelBuilder.Entity<Color>(entity =>
            {
                entity.ToTable("Color");

                entity.Property(e => e.ColorId).HasColumnName("ColorID");

                entity.Property(e => e.ColorName).HasMaxLength(50);
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.ToTable("district");

                entity.Property(e => e.DistrictId)
                    .HasMaxLength(20)
                    .HasColumnName("district_id");

                entity.Property(e => e.AdministrativeUnitId).HasColumnName("administrative_unit_id");

                entity.Property(e => e.CityId)
                    .HasMaxLength(20)
                    .HasColumnName("city_id");

                entity.Property(e => e.CodeName)
                    .HasMaxLength(255)
                    .HasColumnName("code_name");

                entity.Property(e => e.FullName)
                    .HasMaxLength(255)
                    .HasColumnName("full_name");

                entity.Property(e => e.FullNameEn)
                    .HasMaxLength(255)
                    .HasColumnName("full_name_en");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.NameEn)
                    .HasMaxLength(255)
                    .HasColumnName("name_en");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Districts)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_DISTRICT_RE_CITY");
            });

            modelBuilder.Entity<Drife>(entity =>
            {
                entity.HasKey(e => e.DriveId)
                    .HasName("PK__Drives__9610CA385E934F74");

                entity.Property(e => e.DriveId).HasColumnName("DriveID");

                entity.Property(e => e.DriveName).HasMaxLength(50);
            });

            modelBuilder.Entity<Fuel>(entity =>
            {
                entity.Property(e => e.FuelId).HasColumnName("FuelID");

                entity.Property(e => e.FuelName).HasMaxLength(50);
            });

            modelBuilder.Entity<ImageCar>(entity =>
            {
                entity.HasKey(e => e.ImageId)
                    .HasName("PK__ImageCar__7516F4ECFE7DE2BF");

                entity.Property(e => e.ImageId).HasColumnName("ImageID");

                entity.Property(e => e.CarId).HasColumnName("CarID");

                entity.Property(e => e.Url)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.ImageCars)
                    .HasForeignKey(d => d.CarId)
                    .HasConstraintName("FK__ImageCars__CarID__66603565");
            });

            modelBuilder.Entity<ImageShowroom>(entity =>
            {
                entity.HasKey(e => e.ImageId)
                    .HasName("PK__ImageSho__7516F4ECF31B0B0F");

                entity.Property(e => e.ImageId).HasColumnName("ImageID");

                entity.Property(e => e.ShowroomId).HasColumnName("ShowroomID");

                entity.Property(e => e.Url)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.Showroom)
                    .WithMany(p => p.ImageShowrooms)
                    .HasForeignKey(d => d.ShowroomId)
                    .HasConstraintName("FK__ImageShow__Showr__6754599E");
            });

            modelBuilder.Entity<Manufactory>(entity =>
            {
                entity.Property(e => e.ManufactoryId).HasColumnName("ManufactoryID");

                entity.Property(e => e.ManufactoryName).HasMaxLength(255);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleId)
                    .ValueGeneratedNever()
                    .HasColumnName("RoleID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Showroom>(entity =>
            {
                entity.Property(e => e.ShowroomId).HasColumnName("ShowroomID");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.CityId)
                    .HasMaxLength(20)
                    .HasColumnName("CityID");

                entity.Property(e => e.DistrictId)
                    .HasMaxLength(20)
                    .HasColumnName("DistrictID");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShowroomName).HasMaxLength(255);

                entity.Property(e => e.Wards).HasMaxLength(20);

                entity.Property(e => e.Website)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Showrooms)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK__Showrooms__CityI__68487DD7");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.Showrooms)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FK__Showrooms__Distr__693CA210");

                entity.HasOne(d => d.WardsNavigation)
                    .WithMany(p => p.Showrooms)
                    .HasForeignKey(d => d.Wards)
                    .HasConstraintName("FK__Showrooms__Wards__6A30C649");
            });

            modelBuilder.Entity<Slot>(entity =>
            {
                entity.Property(e => e.SlotId)
                    .ValueGeneratedNever()
                    .HasColumnName("SlotID");

                entity.Property(e => e.PickupDate).HasColumnType("datetime");

                entity.Property(e => e.ReturnDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Username)
                    .HasName("PK__Users__536C85E501388125");

                entity.Property(e => e.Username)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__Users__RoleID__6B24EA82");
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasKey(e => e.VehiclesId)
                    .HasName("PK__Vehicles__C683EFD273E7DDBD");

                entity.Property(e => e.VehiclesId).HasColumnName("VehiclesID");

                entity.Property(e => e.VehiclesName).HasMaxLength(50);
            });

            modelBuilder.Entity<Ward>(entity =>
            {
                entity.ToTable("ward");

                entity.Property(e => e.WardId)
                    .HasMaxLength(20)
                    .HasColumnName("ward_id");

                entity.Property(e => e.AdministrativeUnitId).HasColumnName("administrative_unit_id");

                entity.Property(e => e.CodeName)
                    .HasMaxLength(255)
                    .HasColumnName("code_name");

                entity.Property(e => e.DistrictId)
                    .HasMaxLength(20)
                    .HasColumnName("district_id");

                entity.Property(e => e.FullName)
                    .HasMaxLength(255)
                    .HasColumnName("full_name");

                entity.Property(e => e.FullNameEn)
                    .HasMaxLength(255)
                    .HasColumnName("full_name_en");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.NameEn)
                    .HasMaxLength(255)
                    .HasColumnName("name_en");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.Wards)
                    .HasForeignKey(d => d.DistrictId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_WARD_RE_DISTRICT");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
