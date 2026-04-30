using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeCostAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnitCost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostPerUnit",
                table: "Ingredients");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CostPerUnit",
                table: "Ingredients",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
