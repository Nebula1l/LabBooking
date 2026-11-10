using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LabBooking.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Inventorynumber = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsOperational = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equipment_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EquipmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    StudentName = table.Column<string>(type: "TEXT", nullable: false),
                    StudentGroup = table.Column<string>(type: "TEXT", nullable: false),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsCancelled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Вимірювальні прилади" },
                    { 2, "Комп'ютерна техніка" },
                    { 3, "Лабораторне обладнання" }
                });

            migrationBuilder.InsertData(
                table: "Equipment",
                columns: new[] { "Id", "CategoryId", "Description", "Inventorynumber", "IsOperational", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Цифровий мультиметр", "LAB-001", true, "Мультиметр" },
                    { 2, 1, "Двоканальний осцилограф", "LAB-002", true, "Осцилограф" },
                    { 3, 1, "Цифровий вольтметр", "LAB-003", true, "Вольтметр" },
                    { 4, 1, "Електронний амперметр", "LAB-004", true, "Амперметр" },
                    { 5, 2, "Робоча станція", "LAB-005", true, "Комп'ютер HP" },
                    { 6, 2, "Навчальний ноутбук", "LAB-006", true, "Ноутбук Lenovo" },
                    { 7, 2, "Лазерний принтер", "LAB-007", false, "Принтер" },
                    { 8, 3, "Оптичний мікроскоп", "LAB-008", true, "Мікроскоп" },
                    { 9, 3, "Регульований блок живлення", "LAB-009", true, "Лабораторний блок живлення" },
                    { 10, 3, "Регульована паяльна станція", "LAB-010", true, "Паяльна станція" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_EquipmentId",
                table: "Bookings",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_CategoryId",
                table: "Equipment",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_Inventorynumber",
                table: "Equipment",
                column: "Inventorynumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
