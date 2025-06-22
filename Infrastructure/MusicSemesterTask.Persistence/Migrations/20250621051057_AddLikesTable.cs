using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Market.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLikesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFollows_Artists_FollowedArtistsId",
                table: "UserFollows");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFollows_AspNetUsers_FollowersId",
                table: "UserFollows");

            migrationBuilder.DropTable(
                name: "UserLikedSongs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFollows",
                table: "UserFollows");

            migrationBuilder.RenameTable(
                name: "UserFollows",
                newName: "ApplicationUserArtist");

            migrationBuilder.RenameIndex(
                name: "IX_UserFollows_FollowersId",
                table: "ApplicationUserArtist",
                newName: "IX_ApplicationUserArtist_FollowersId");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Songs",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUserArtist",
                table: "ApplicationUserArtist",
                columns: new[] { "FollowedArtistsId", "FollowersId" });

            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    SongId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Likes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Likes_Songs_SongId",
                        column: x => x.SongId,
                        principalTable: "Songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Songs_ApplicationUserId",
                table: "Songs",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_SongId",
                table: "Likes",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_UserId_SongId",
                table: "Likes",
                columns: new[] { "UserId", "SongId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserArtist_Artists_FollowedArtistsId",
                table: "ApplicationUserArtist",
                column: "FollowedArtistsId",
                principalTable: "Artists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserArtist_AspNetUsers_FollowersId",
                table: "ApplicationUserArtist",
                column: "FollowersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_AspNetUsers_ApplicationUserId",
                table: "Songs",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserArtist_Artists_FollowedArtistsId",
                table: "ApplicationUserArtist");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserArtist_AspNetUsers_FollowersId",
                table: "ApplicationUserArtist");

            migrationBuilder.DropForeignKey(
                name: "FK_Songs_AspNetUsers_ApplicationUserId",
                table: "Songs");

            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Songs_ApplicationUserId",
                table: "Songs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUserArtist",
                table: "ApplicationUserArtist");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Songs");

            migrationBuilder.RenameTable(
                name: "ApplicationUserArtist",
                newName: "UserFollows");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUserArtist_FollowersId",
                table: "UserFollows",
                newName: "IX_UserFollows_FollowersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFollows",
                table: "UserFollows",
                columns: new[] { "FollowedArtistsId", "FollowersId" });

            migrationBuilder.CreateTable(
                name: "UserLikedSongs",
                columns: table => new
                {
                    LikedByUsersId = table.Column<string>(type: "text", nullable: false),
                    LikedSongsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLikedSongs", x => new { x.LikedByUsersId, x.LikedSongsId });
                    table.ForeignKey(
                        name: "FK_UserLikedSongs_AspNetUsers_LikedByUsersId",
                        column: x => x.LikedByUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLikedSongs_Songs_LikedSongsId",
                        column: x => x.LikedSongsId,
                        principalTable: "Songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserLikedSongs_LikedSongsId",
                table: "UserLikedSongs",
                column: "LikedSongsId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollows_Artists_FollowedArtistsId",
                table: "UserFollows",
                column: "FollowedArtistsId",
                principalTable: "Artists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollows_AspNetUsers_FollowersId",
                table: "UserFollows",
                column: "FollowersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
