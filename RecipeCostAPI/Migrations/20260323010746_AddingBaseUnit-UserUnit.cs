using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeCostAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddingBaseUnitUserUnit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CostPerUserUnit",
                table: "Ingredients",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Ingredients",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserUnit",
                table: "Ingredients",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostPerUserUnit",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "UserUnit",
                table: "Ingredients");
        }
    }
}
