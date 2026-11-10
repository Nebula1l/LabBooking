using LabBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace LabBooking.Data;

public class LabBookingContext : DbContext
{
    public LabBookingContext(
        DbContextOptions<LabBookingContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Equipment> Equipment => Set<Equipment>();
    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Equipment>()
            .HasIndex(e => e.Inventorynumber)
            .IsUnique();

        modelBuilder.Entity<Category>()
            .HasMany(c => c.Equipment)
            .WithOne(e => e.Category)
            .HasForeignKey(e => e.CategoryId);

        modelBuilder.Entity<Equipment>()
            .HasMany(e => e.Bookings)
            .WithOne(b => b.Equipment)
            .HasForeignKey(b => b.EquipmentId);


        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Вимірювальні прилади" },
            new Category { Id = 2, Name = "Комп'ютерна техніка" },
            new Category { Id = 3, Name = "Лабораторне обладнання" }
        );

        modelBuilder.Entity<Equipment>().HasData(
            new Equipment
            {
                Id = 1, Name = "Мультиметр",
                Inventorynumber = "LAB-001",
                Description = "Цифровий мультиметр",
                CategoryId = 1, IsOperational = true
            },
            new Equipment
            {
                Id = 2, Name = "Осцилограф",
                Inventorynumber = "LAB-002",
                Description = "Двоканальний осцилограф",
                CategoryId = 1, IsOperational = true
            },
            
            new Equipment
            {
                Id = 3, Name = "Вольтметр",
                Inventorynumber = "LAB-003",
                Description = "Цифровий вольтметр",
                CategoryId = 1, IsOperational = true
            },
            new Equipment
            {
                Id = 4, Name = "Амперметр",
                Inventorynumber = "LAB-004",
                Description = "Електронний амперметр",
                CategoryId = 1, IsOperational = true
            },
            new Equipment
            {
                Id = 5, Name = "Комп'ютер HP",
                Inventorynumber = "LAB-005",
                Description = "Робоча станція",
                CategoryId = 2, IsOperational = true
            },
            new Equipment
            {
                Id = 6, Name = "Ноутбук Lenovo",
                Inventorynumber = "LAB-006",
                Description = "Навчальний ноутбук",
                CategoryId = 2, IsOperational = true
            },
            new Equipment
            {
                Id = 7, Name = "Принтер",
                Inventorynumber = "LAB-007",
                Description = "Лазерний принтер",
                CategoryId = 2, IsOperational = false
            },

            new Equipment
            {
                Id = 8, Name = "Мікроскоп",
                Inventorynumber = "LAB-008",
                Description = "Оптичний мікроскоп",
                CategoryId = 3, IsOperational = true
            },
            new Equipment
            {
                Id = 9, Name = "Лабораторний блок живлення",
                Inventorynumber = "LAB-009",
                Description = "Регульований блок живлення",
                CategoryId = 3, IsOperational = true
            },
            new Equipment
            {
                Id = 10, Name = "Паяльна станція",
                Inventorynumber = "LAB-010",
                Description = "Регульована паяльна станція",
                CategoryId = 3, IsOperational = true
            }
        );
    }
}