using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeCostAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddRecipeCostCostPerServing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CostPerServing",
                table: "Recipes",
                type: "numeric(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalCost",
                table: "Recipes",
                type: "numeric(18,4)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostPerServing",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "TotalCost",
                table: "Recipes");
        }
    }
}
