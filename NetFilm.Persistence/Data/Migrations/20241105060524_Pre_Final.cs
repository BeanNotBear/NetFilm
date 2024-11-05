using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetFilm.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class Pre_Final : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "Participants");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MyProperty",
                table: "Participants",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
