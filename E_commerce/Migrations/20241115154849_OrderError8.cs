using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_commerce.Migrations
{
    /// <inheritdoc />
    public partial class OrderError8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "productId",
                table: "OrderTracking",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderTracking_productId",
                table: "OrderTracking",
                column: "productId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTracking_products_productId",
                table: "OrderTracking",
                column: "productId",
                principalTable: "products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderTracking_products_productId",
                table: "OrderTracking");

            migrationBuilder.DropIndex(
                name: "IX_OrderTracking_productId",
                table: "OrderTracking");

            migrationBuilder.DropColumn(
                name: "productId",
                table: "OrderTracking");
        }
    }
}
