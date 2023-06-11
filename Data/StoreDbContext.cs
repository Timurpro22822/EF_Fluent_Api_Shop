using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Course_2
{
    public class Position
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
    public class Country
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
    public class City
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [ForeignKey("Country")]
        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
    public class Shop
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [ForeignKey("City")]
        public int CityId { get; set; }
        public City City { get; set; }

        public int? ParkingArea { get; set; }
    }
    public class Worker
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        public decimal Salary { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [ForeignKey("Position")]
        public int PositionId { get; set; }
        public Position Position { get; set; }

        [ForeignKey("Shop")]
        public int ShopId { get; set; }
        public Shop Shop { get; set; }
    }
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public decimal Price { get; set; }

        public float Discount { get; set; }

        [ForeignKey("Category")]
        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        public int Quantity { get; set; }

        public bool IsInStock { get; set; }
    }

    // Контекст бази даних
    public class StoreContext : DbContext
    {
        public DbSet<Position> Positions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StoreDbCourse;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>()
                .HasOne(c => c.Country)
                .WithMany()
                .HasForeignKey(c => c.CountryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Shop>()
                .HasOne(s => s.City)
                .WithMany()
                .HasForeignKey(s => s.CityId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Worker>()
                .HasOne(w => w.Position)
                .WithMany()
                .HasForeignKey(w => w.PositionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Worker>()
                .HasOne(w => w.Shop)
                .WithMany()
                .HasForeignKey(w => w.ShopId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    // Ініціалізатор (seeder) для початкових даних
    public class DbInitializer
    {
        public static void Initialize(StoreContext context)
        {
            context.Database.EnsureCreated();

            if (context.Countries.Any())
            {
                return;   // База даних ініціалізована
            }
            var countries = new List<Country>
        {
            new Country { Name = "Country 1" },
            new Country { Name = "Country 2" },
        };
            context.Countries.AddRange(countries);
            context.SaveChanges();

            var cities = new List<City>
        {
            new City { Name = "City 1", CountryId = 1 },
            new City { Name = "City 2", CountryId = 1 },
        };
            context.Cities.AddRange(cities);
            context.SaveChanges();

            var categories = new List<Category>
        {
            new Category { Name = "Category 1" },
            new Category { Name = "Category 2" },
        };
            context.Categories.AddRange(categories);
            context.SaveChanges();

            var positions = new List<Position>
        {
            new Position { Name = "Position 1" },
            new Position { Name = "Position 2" },
        };
            context.Positions.AddRange(positions);
            context.SaveChanges();
        }
    }
}