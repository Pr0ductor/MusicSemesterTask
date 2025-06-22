using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicSemesterTask.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateArtistSubscriptionPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtistSubscriptions_ApplicationUsers_UserId",
                table: "ArtistSubscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_ApplicationUsers_UserId",
                table: "Likes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUsers",
                table: "ApplicationUsers");

            migrationBuilder.RenameTable(
                name: "ApplicationUsers",
                newName: "Users");

            migrationBuilder.AddColumn<int>(
                name: "ArtistId1",
                table: "ArtistSubscriptions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "ArtistSubscriptions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ArtistSubscriptions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ArtistSubscriptions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "ArtistSubscriptions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "ArtistSubscriptions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "ArtistSubscriptions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistSubscriptions_ArtistId1",
                table: "ArtistSubscriptions",
                column: "ArtistId1");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistSubscriptions_UserId1",
                table: "ArtistSubscriptions",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistSubscriptions_Artists_ArtistId1",
                table: "ArtistSubscriptions",
                column: "ArtistId1",
                principalTable: "Artists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistSubscriptions_Users_UserId",
                table: "ArtistSubscriptions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistSubscriptions_Users_UserId1",
                table: "ArtistSubscriptions",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Users_UserId",
                table: "Likes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtistSubscriptions_Artists_ArtistId1",
                table: "ArtistSubscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistSubscriptions_Users_UserId",
                table: "ArtistSubscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistSubscriptions_Users_UserId1",
                table: "ArtistSubscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Users_UserId",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_ArtistSubscriptions_ArtistId1",
                table: "ArtistSubscriptions");

            migrationBuilder.DropIndex(
                name: "IX_ArtistSubscriptions_UserId1",
                table: "ArtistSubscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ArtistId1",
                table: "ArtistSubscriptions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ArtistSubscriptions");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ArtistSubscriptions");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ArtistSubscriptions");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ArtistSubscriptions");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "ArtistSubscriptions");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "ArtistSubscriptions");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "ApplicationUsers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUsers",
                table: "ApplicationUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistSubscriptions_ApplicationUsers_UserId",
                table: "ArtistSubscriptions",
                column: "UserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_ApplicationUsers_UserId",
                table: "Likes",
                column: "UserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
