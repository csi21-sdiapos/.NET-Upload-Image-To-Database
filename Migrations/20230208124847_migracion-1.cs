using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UploadImageToDatabase.Migrations
{
    public partial class migracion1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "alumno",
                schema: "public",
                columns: table => new
                {
                    Alumno_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Md_uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Md_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Alumno_nombre = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Alumno_imagen = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alumno", x => x.Alumno_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alumno",
                schema: "public");
        }
    }
}
