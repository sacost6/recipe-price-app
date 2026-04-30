using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeCostAPI.Migrations
{
    /// <inheritdoc />
    public partial class DensityToIngredient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DensityGramsPerMl",
                table: "Ingredients",
                type: "numeric",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DensityGramsPerMl",
                table: "Ingredients");
        }
    }
}
