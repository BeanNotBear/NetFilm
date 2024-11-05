using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetFilm.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnStar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Star",
                table: "Votes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Star",
                table: "Votes");
        }
    }
}
