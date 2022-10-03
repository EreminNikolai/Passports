using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Passports.Api.Migrations
{
    public partial class AddColumn_UserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                table: "passports",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "user_id",
                table: "passports");
        }
    }
}
