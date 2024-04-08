using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HeartmadeCandles.Migrations.UserAndAuthDatabase
{
    /// <inheritdoc />
    public partial class ReplacedTokenWithSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Token");

            migrationBuilder.CreateTable(
                name: "Session",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    userId = table.Column<int>(type: "integer", nullable: false),
                    refreshToken = table.Column<string>(type: "text", nullable: false),
                    expireAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Session", x => x.id);
                    table.ForeignKey(
                        name: "FK_Session_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Session_userId",
                table: "Session",
                column: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Session");

            migrationBuilder.CreateTable(
                name: "Token",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    accessToken = table.Column<string>(type: "text", nullable: false),
                    expireTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    refreshToken = table.Column<string>(type: "text", nullable: false),
                    userId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Token", x => x.id);
                });
        }
    }
}
