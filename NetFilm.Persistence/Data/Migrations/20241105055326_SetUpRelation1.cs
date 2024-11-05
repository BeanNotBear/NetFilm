using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetFilm.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class SetUpRelation1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Advertises",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Advertises",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Advertises_UserId",
                table: "Advertises",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertises_Users_UserId",
                table: "Advertises",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertises_Users_UserId",
                table: "Advertises");

            migrationBuilder.DropIndex(
                name: "IX_Advertises_UserId",
                table: "Advertises");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Advertises");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Advertises");
        }
    }
}
