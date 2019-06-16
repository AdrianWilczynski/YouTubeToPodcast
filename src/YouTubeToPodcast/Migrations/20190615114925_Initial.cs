using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YouTubeToPodcast.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Podcasts",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    YouTubeId = table.Column<string>(nullable: true),
                    ChannelType = table.Column<int>(nullable: false),
                    Contains = table.Column<string>(nullable: true),
                    Duration = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_Podcasts", x => x.Id));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Podcasts");
        }
    }
}
