using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KhumaloCraft.Data.Migrations
{
    /// <inheritdoc />
    public partial class tuningupdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ProductInformation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "ProductInformation",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "OrderDate",
                table: "Cart",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductInformation_UserID",
                table: "ProductInformation",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInformation_AspNetUsers_UserID",
                table: "ProductInformation",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductInformation_AspNetUsers_UserID",
                table: "ProductInformation");

            migrationBuilder.DropIndex(
                name: "IX_ProductInformation_UserID",
                table: "ProductInformation");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ProductInformation");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "ProductInformation");

            migrationBuilder.DropColumn(
                name: "OrderDate",
                table: "Cart");
        }
    }
}
