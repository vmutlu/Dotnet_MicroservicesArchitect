using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineAuctionApp.Infrastructure.Migrations
{
    public partial class editProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAdnin",
                table: "AspNetUsers",
                newName: "IsAdmin");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAdmin",
                table: "AspNetUsers",
                newName: "IsAdnin");
        }
    }
}
