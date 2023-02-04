using System;
using Microsoft.EntityFrameworkCore.Migrations;
using TBC.Task.Domain;

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
                    PersonalNumber = table.Column<string>(type: "char(11)", maxLength: 11, nullable: false),
                    Gender = table.Column<byte>(type: "tinyint", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: false),
                    MobilePhone = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    WorkPhone = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    HomePhone = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    PhotoPath = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    PhotoUrl = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
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
                    table.CheckConstraint("CK_Person_MobilePhone", $"LEN({nameof(Person.MobilePhone)}) >= 4");
                    table.CheckConstraint("CK_Person_WorkPhone", $"LEN({nameof(Person.WorkPhone)}) >= 4");
                    table.CheckConstraint("CK_Person_HomePhone", $"LEN({nameof(Person.HomePhone)}) >= 4");
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
                    table.CheckConstraint("CK_RelatedPerson_SelfRelationCheck", $"{nameof(RelatedPerson.FromId)} != {nameof(RelatedPerson.ToId)}");
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
                name: "IX_Persons_FirstName_LastName_PersonalNumber",
                table: "Persons",
                columns: new[] { "FirstName", "LastName", "PersonalNumber" });

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
