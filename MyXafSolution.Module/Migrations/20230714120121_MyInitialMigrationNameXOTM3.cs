using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyXafSolution.Module.Migrations
{
    /// <inheritdoc />
    public partial class MyInitialMigrationNameXOTM3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_PhoneNumbers_PhoneNumberID",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_PhoneNumberID",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PhoneNumberID",
                table: "Employees");

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeID",
                table: "PhoneNumbers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhoneNumbers_EmployeeID",
                table: "PhoneNumbers",
                column: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_PhoneNumbers_Employees_EmployeeID",
                table: "PhoneNumbers",
                column: "EmployeeID",
                principalTable: "Employees",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhoneNumbers_Employees_EmployeeID",
                table: "PhoneNumbers");

            migrationBuilder.DropIndex(
                name: "IX_PhoneNumbers_EmployeeID",
                table: "PhoneNumbers");

            migrationBuilder.DropColumn(
                name: "EmployeeID",
                table: "PhoneNumbers");

            migrationBuilder.AddColumn<Guid>(
                name: "PhoneNumberID",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PhoneNumberID",
                table: "Employees",
                column: "PhoneNumberID");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_PhoneNumbers_PhoneNumberID",
                table: "Employees",
                column: "PhoneNumberID",
                principalTable: "PhoneNumbers",
                principalColumn: "ID");
        }
    }
}
