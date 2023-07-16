using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyXafSolution.Module.Migrations
{
    /// <inheritdoc />
    public partial class customer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtendedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VatID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyAddressID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CorrespondenceAddressID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WebPageAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", maxLength: 4096, nullable: true),
                    Segment = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Customers_Address_CompanyAddressID",
                        column: x => x.CompanyAddressID,
                        principalTable: "Address",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Customers_Address_CorrespondenceAddressID",
                        column: x => x.CorrespondenceAddressID,
                        principalTable: "Address",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CompanyAddressID",
                table: "Customers",
                column: "CompanyAddressID");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CorrespondenceAddressID",
                table: "Customers",
                column: "CorrespondenceAddressID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
