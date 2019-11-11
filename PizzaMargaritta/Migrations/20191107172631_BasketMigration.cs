using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaMargaritta.Migrations
{
    public partial class BasketMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "BasketPizza");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "BasketPizza");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "BasketPizza");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "BasketPizza");

            migrationBuilder.AddColumn<int>(
                name: "Pizza_id",
                table: "BasketPizza",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BasketPizza_Pizza_id",
                table: "BasketPizza",
                column: "Pizza_id");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketPizza_tblPizzas_Pizza_id",
                table: "BasketPizza",
                column: "Pizza_id",
                principalTable: "tblPizzas",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketPizza_tblPizzas_Pizza_id",
                table: "BasketPizza");

            migrationBuilder.DropIndex(
                name: "IX_BasketPizza_Pizza_id",
                table: "BasketPizza");

            migrationBuilder.DropColumn(
                name: "Pizza_id",
                table: "BasketPizza");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "BasketPizza",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "BasketPizza",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BasketPizza",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "BasketPizza",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
