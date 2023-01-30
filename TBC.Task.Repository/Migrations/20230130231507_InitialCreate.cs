using System;
using Microsoft.EntityFrameworkCore.Migrations;
using TBC.Task.Domain;
using TBC.Task.Domain.ComplexTypes;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TBC.Task.Repository.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
					table.CheckConstraint("CK_City_Name", $"LEN({nameof(City.Name)}) >= 3");
				});

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Gender = table.Column<byte>(type: "tinyint", nullable: true),
                    PersonalNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContactInfoMobilePhone = table.Column<string>(name: "ContactInfo_MobilePhone", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContactInfoWorkPhone = table.Column<string>(name: "ContactInfo_WorkPhone", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContactInfoHomePhone = table.Column<string>(name: "ContactInfo_HomePhone", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Photo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Persons_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id");
                    table.CheckConstraint("CK_Person_FirstName", $"LEN({nameof(Person.FirstName)}) >= 2");
                    table.CheckConstraint("CK_Person_LastName", $"LEN({nameof(Person.LastName)}) >= 2");
                    table.CheckConstraint("CK_Person_PersonalNumber", $"LEN({nameof(Person.PersonalNumber)}) = 11");
                    table.CheckConstraint("CK_Person_ContactInfo_MobilePhone", $"LEN({nameof(ContactInfo)}_{nameof(ContactInfo.MobilePhone)}) >= 4");
                    table.CheckConstraint("CK_Person_ContactInfo_WorkPhone", $"LEN({nameof(ContactInfo)}_{nameof(ContactInfo.WorkPhone)}) >= 4");
                    table.CheckConstraint("CK_Person_ContactInfo_HomePhone", $"LEN({nameof(ContactInfo)}_{nameof(ContactInfo.HomePhone)}) >= 4");
                });

            migrationBuilder.CreateTable(
                name: "RelatedPersons",
                columns: table => new
                {
                    FromId = table.Column<int>(type: "int", nullable: false),
                    ToId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatedPersons", x => new { x.FromId, x.ToId });
                    table.ForeignKey(
                        name: "FK_RelatedPersons_Persons_FromId",
                        column: x => x.FromId,
                        principalTable: "Persons",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RelatedPersons_Persons_ToId",
                        column: x => x.ToId,
                        principalTable: "Persons",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Tbilisi" },
                    { 2, "Batumi" },
                    { 3, "Kutaisi" },
                    { 4, "Rustavi" },
                    { 5, "Gori" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Persons_CityId",
                table: "Persons",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatedPersons_ToId",
                table: "RelatedPersons",
                column: "ToId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RelatedPersons");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
