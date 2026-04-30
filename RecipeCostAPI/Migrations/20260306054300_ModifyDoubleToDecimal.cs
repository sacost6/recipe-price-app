using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeCostAPI.Migrations
{
    /// <inheritdoc />
    public partial class ModifyDoubleToDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "RecipeIngredients");

            migrationBuilder.AddColumn<decimal>(
                name: "Quantity",
                table: "RecipeIngredients",
                type: "numeric(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "RecipeIngredients");

            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "RecipeIngredients",
                type: "double precision",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
