using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineGameStore.DAL.Migrations
{
    /// <inheritdoc />
    public partial class GameEntityIdTypeIntToGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "id_new",
                table: "games",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()");

            migrationBuilder.AddColumn<Guid>(
                name: "publisher_id_new",
                table: "games",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "license_id_new",
                table: "games",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "genre_id_new",
                table: "games",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.DropPrimaryKey(name: "PK_games", table: "games");

            migrationBuilder.DropColumn(name: "id", table: "games");
            migrationBuilder.DropColumn(name: "publisher_id", table: "games");
            migrationBuilder.DropColumn(name: "license_id", table: "games");
            migrationBuilder.DropColumn(name: "genre_id", table: "games");

            migrationBuilder.RenameColumn(name: "id_new", table: "games", newName: "id");
            migrationBuilder.RenameColumn(name: "publisher_id_new", table: "games", newName: "publisher_id");
            migrationBuilder.RenameColumn(name: "license_id_new", table: "games", newName: "license_id");
            migrationBuilder.RenameColumn(name: "genre_id_new", table: "games", newName: "genre_id");

            migrationBuilder.AddPrimaryKey(name: "PK_games", table: "games", column: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "id_old",
                table: "games",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "publisher_id_old",
                table: "games",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "license_id_old",
                table: "games",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "genre_id_old",
                table: "games",
                type: "int",
                nullable: true);

            migrationBuilder.DropPrimaryKey(name: "PK_games", table: "games");

            migrationBuilder.DropColumn(name: "id", table: "games");
            migrationBuilder.DropColumn(name: "publisher_id", table: "games");
            migrationBuilder.DropColumn(name: "license_id", table: "games");
            migrationBuilder.DropColumn(name: "genre_id", table: "games");

            migrationBuilder.RenameColumn(name: "id_old", table: "games", newName: "id");
            migrationBuilder.RenameColumn(name: "publisher_id_old", table: "games", newName: "publisher_id");
            migrationBuilder.RenameColumn(name: "license_id_old", table: "games", newName: "license_id");
            migrationBuilder.RenameColumn(name: "genre_id_old", table: "games", newName: "genre_id");

            migrationBuilder.AddPrimaryKey(name: "PK_games", table: "games", column: "id");
        }
    }
}
