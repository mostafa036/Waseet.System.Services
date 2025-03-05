using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waseet.System.Services.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class v5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductReview_Customers_CustomerId",
                table: "ProductReview");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ServiceProviders_ServiceProviderId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_ServicesProviderReviews_ServiceProviders_ServiceProviderId",
                table: "ServicesProviderReviews");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "ServiceProviders");

            migrationBuilder.DropIndex(
                name: "IX_ServicesProviderReviews_ServiceProviderId",
                table: "ServicesProviderReviews");

            migrationBuilder.DropIndex(
                name: "IX_Products_ServiceProviderId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_ProductReview_CustomerId",
                table: "ProductReview");

            migrationBuilder.DropColumn(
                name: "ServiceProviderId",
                table: "ServicesProviderReviews");

            migrationBuilder.DropColumn(
                name: "ServiceProviderId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "ProductReview");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Category");

            migrationBuilder.RenameColumn(
                name: "Color",
                table: "Products",
                newName: "ServiceProviderEmail");

            migrationBuilder.AddColumn<string>(
                name: "CustomerEmail",
                table: "ServicesProviderReviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ServiceProviderEmail",
                table: "ServicesProviderReviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomerEamil",
                table: "ProductReview",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerEmail",
                table: "ServicesProviderReviews");

            migrationBuilder.DropColumn(
                name: "ServiceProviderEmail",
                table: "ServicesProviderReviews");

            migrationBuilder.DropColumn(
                name: "CustomerEamil",
                table: "ProductReview");

            migrationBuilder.RenameColumn(
                name: "ServiceProviderEmail",
                table: "Products",
                newName: "Color");

            migrationBuilder.AddColumn<int>(
                name: "ServiceProviderId",
                table: "ServicesProviderReviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ServiceProviderId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "ProductReview",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Category",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceProviders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ServiceProviderEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceProviderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceProviders", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServicesProviderReviews_ServiceProviderId",
                table: "ServicesProviderReviews",
                column: "ServiceProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ServiceProviderId",
                table: "Products",
                column: "ServiceProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReview_CustomerId",
                table: "ProductReview",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReview_Customers_CustomerId",
                table: "ProductReview",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ServiceProviders_ServiceProviderId",
                table: "Products",
                column: "ServiceProviderId",
                principalTable: "ServiceProviders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServicesProviderReviews_ServiceProviders_ServiceProviderId",
                table: "ServicesProviderReviews",
                column: "ServiceProviderId",
                principalTable: "ServiceProviders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
