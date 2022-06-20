using Microsoft.EntityFrameworkCore.Migrations;

namespace BoxOffice.Migrations
{
    public partial class UpdateSpectacle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PeopleCame",
                table: "Spectacles",
                type: "integer",
                nullable: true,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PeopleCame",
                table: "Spectacles");
        }
    }
}
