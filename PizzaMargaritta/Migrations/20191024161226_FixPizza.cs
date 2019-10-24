using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaMargaritta.Migrations
{
    public partial class FixPizza : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tblOrders_PizzaID",
                table: "tblOrders");

            migrationBuilder.DropColumn(
                name: "OrderID",
                table: "tblPizzas");

            migrationBuilder.CreateIndex(
                name: "IX_tblOrders_PizzaID",
                table: "tblOrders",
                column: "PizzaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tblOrders_PizzaID",
                table: "tblOrders");

            migrationBuilder.AddColumn<int>(
                name: "OrderID",
                table: "tblPizzas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tblOrders_PizzaID",
                table: "tblOrders",
                column: "PizzaID",
                unique: true);
        }
    }
}
