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
        public virtual DbSet<ShowroomCar> ShowroomCars { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }
        public virtual DbSet<Ward> Wards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(local);Uid=sa;Pwd=1234567890;Database=OldCarShowroomNetwork");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => new { e.Username, e.CarId, e.PickupHour })
                    .HasName("PK__Bookings__16102DD7E47A6B04");

                entity.Property(e => e.Username)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.CarId).HasColumnName("CarID");

                entity.Property(e => e.PickupHour).HasColumnType("datetime");

                entity.Property(e => e.CreationDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ShowroomId).HasColumnName("ShowroomID");

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.CarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Bookings__CarID__7E37BEF6");

                entity.HasOne(d => d.Showroom)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.ShowroomId)
                    .HasConstraintName("FK__Bookings__Showro__00200768");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Bookings__Userna__7D439ABD");
            });

            modelBuilder.Entity<Car>(entity =>
            {
                entity.Property(e => e.CarId).HasColumnName("CarID");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.FuelIntakeSystem).HasMaxLength(1000);

                entity.Property(e => e.Note).HasMaxLength(2000);

                entity.Property(e => e.Username)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Version).HasMaxLength(150);

                entity.HasOne(d => d.CarModelYearNavigation)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.CarModelYear)
                    .HasConstraintName("FK__Cars__CarModelYe__6FE99F9F");

                entity.HasOne(d => d.CarNameNavigation)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.CarName)
                    .HasConstraintName("FK__Cars__CarName__6EF57B66");

                entity.HasOne(d => d.ColorNavigation)
                    .WithMany(p => p.CarColorNavigations)
                    .HasForeignKey(d => d.Color)
                    .HasConstraintName("FK__Cars__Color__71D1E811");

                entity.HasOne(d => d.ColorInsideNavigation)
                    .WithMany(p => p.CarColorInsideNavigations)
                    .HasForeignKey(d => d.ColorInside)
                    .HasConstraintName("FK__Cars__ColorInsid__72C60C4A");

                entity.HasOne(d => d.DriveNavigation)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.Drive)
                    .HasConstraintName("FK__Cars__Drive__73BA3083");

                entity.HasOne(d => d.FuelNavigation)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.Fuel)
                    .HasConstraintName("FK__Cars__Fuel__74AE54BC");

                entity.HasOne(d => d.ImageCarNavigation)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.ImageCar)
                    .HasConstraintName("FK__Cars__ImageCar__75A278F5");

                entity.HasOne(d => d.ManufactoryNavigation)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.Manufactory)
                    .HasConstraintName("FK__Cars__Manufactor__6E01572D");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.Username)
                    .HasConstraintName("FK__Cars__Username__76969D2E");

                entity.HasOne(d => d.VehiclesNavigation)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.Vehicles)
                    .HasConstraintName("FK__Cars__Vehicles__70DDC3D8");
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
                    .HasConstraintName("FK__CarNames__Manufa__5070F446");
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
                    .HasName("PK__Drives__9610CA3823769493");

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
                    .HasName("PK__ImageCar__7516F4EC6DE99447");

                entity.Property(e => e.ImageId).HasColumnName("ImageID");

                entity.Property(e => e.Url)
                    .HasMaxLength(1000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ImageShowroom>(entity =>
            {
                entity.HasKey(e => e.ImageId)
                    .HasName("PK__ImageSho__7516F4EC1FB36600");

                entity.Property(e => e.ImageId).HasColumnName("ImageID");

                entity.Property(e => e.Url)
                    .HasMaxLength(1000)
                    .IsUnicode(false);
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

                entity.Property(e => e.ImageId).HasColumnName("ImageID");

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

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.Showrooms)
                    .HasForeignKey(d => d.ImageId)
                    .HasConstraintName("FK__Showrooms__Image__6B24EA82");

                entity.HasOne(d => d.WardsNavigation)
                    .WithMany(p => p.Showrooms)
                    .HasForeignKey(d => d.Wards)
                    .HasConstraintName("FK__Showrooms__Wards__6A30C649");
            });

            modelBuilder.Entity<ShowroomCar>(entity =>
            {
                entity.HasKey(e => new { e.CarId, e.ShowroomId })
                    .HasName("PK__Showroom__C2D712C745CC136B");

                entity.Property(e => e.CarId).HasColumnName("CarID");

                entity.Property(e => e.ShowroomId).HasColumnName("ShowroomID");

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.ShowroomCars)
                    .HasForeignKey(d => d.CarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ShowroomC__CarID__797309D9");

                entity.HasOne(d => d.Showroom)
                    .WithMany(p => p.ShowroomCars)
                    .HasForeignKey(d => d.ShowroomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ShowroomC__Showr__7A672E12");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Username)
                    .HasName("PK__Users__536C85E5CBE380A3");

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
                    .HasConstraintName("FK__Users__RoleID__4BAC3F29");
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasKey(e => e.VehiclesId)
                    .HasName("PK__Vehicles__C683EFD2F51107DE");

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
