using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyXafSolution.Module.Migrations
{
    /// <inheritdoc />
    public partial class MyInitialMigrationNameXOTM4c : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AddressID",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CorespondenceAddressID",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StateProvince = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipPostal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_AddressID",
                table: "Employees",
                column: "AddressID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CorespondenceAddressID",
                table: "Employees",
                column: "CorespondenceAddressID");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Address_AddressID",
                table: "Employees",
                column: "AddressID",
                principalTable: "Address",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Address_CorespondenceAddressID",
                table: "Employees",
                column: "CorespondenceAddressID",
                principalTable: "Address",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Address_AddressID",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Address_CorespondenceAddressID",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropIndex(
                name: "IX_Employees_AddressID",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_CorespondenceAddressID",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "AddressID",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CorespondenceAddressID",
                table: "Employees");
        }
    }
}
