using Microsoft.EntityFrameworkCore.Migrations;

namespace KhanhHoaTravel.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblPlaceDeltail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Author = table.Column<string>(nullable: true),
                    TimeOpen = table.Column<string>(nullable: true),
                    TimeClose = table.Column<string>(nullable: true),
                    Rate = table.Column<double>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Genre = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ImagePath = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPlaceDeltail", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblPlaceDeltail");
        }
    }
}
