using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkLink.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployerLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "EmployerLatitude",
                table: "Jobs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "EmployerLongitude",
                table: "Jobs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployerLatitude",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "EmployerLongitude",
                table: "Jobs");
        }
    }
}
