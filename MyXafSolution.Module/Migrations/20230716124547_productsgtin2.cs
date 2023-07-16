using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyXafSolution.Module.Migrations
{
    /// <inheritdoc />
    public partial class productsgtin2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_VatRates_VatRateSymbol",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VatRates",
                table: "VatRates");

            migrationBuilder.DropIndex(
                name: "IX_Products_VatRateSymbol",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "VatRateSymbol",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "Symbol",
                table: "VatRates",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<Guid>(
                name: "ID",
                table: "VatRates",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "Products",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<Guid>(
                name: "VatRateID",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VatRates",
                table: "VatRates",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_VatRateID",
                table: "Products",
                column: "VatRateID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_VatRates_VatRateID",
                table: "Products",
                column: "VatRateID",
                principalTable: "VatRates",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_VatRates_VatRateID",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VatRates",
                table: "VatRates");

            migrationBuilder.DropIndex(
                name: "IX_Products_VatRateID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "VatRates");

            migrationBuilder.DropColumn(
                name: "VatRateID",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "Symbol",
                table: "VatRates",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldPrecision: 18,
                oldScale: 4);

            migrationBuilder.AddColumn<string>(
                name: "VatRateSymbol",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VatRates",
                table: "VatRates",
                column: "Symbol");

            migrationBuilder.CreateIndex(
                name: "IX_Products_VatRateSymbol",
                table: "Products",
                column: "VatRateSymbol");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_VatRates_VatRateSymbol",
                table: "Products",
                column: "VatRateSymbol",
                principalTable: "VatRates",
                principalColumn: "Symbol");
        }
    }
}
