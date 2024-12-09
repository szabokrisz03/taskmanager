using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Srv.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "project",
            columns: table => new
            {
                rowid = table.Column<long>(name: "row_id", type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                technicalname = table.Column<Guid>(name: "technical_name", type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()")
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_project", x => x.rowid);
            });

        migrationBuilder.CreateTable(
            name: "user",
            columns: table => new
            {
                rowid = table.Column<long>(name: "row_id", type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                username = table.Column<string>(name: "user_name", type: "nvarchar(450)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_user", x => x.rowid);
            });

        migrationBuilder.CreateTable(
            name: "project_user",
            columns: table => new
            {
                projectid = table.Column<long>(name: "project_id", type: "bigint", nullable: false),
                userid = table.Column<long>(name: "user_id", type: "bigint", nullable: false),
                rowid = table.Column<long>(name: "row_id", type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1")
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_project_user", x => new { x.projectid, x.userid });
                table.ForeignKey(
                    name: "fk_project_user_project_project_id",
                    column: x => x.projectid,
                    principalTable: "project",
                    principalColumn: "row_id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_project_user_user_user_temp_id",
                    column: x => x.userid,
                    principalTable: "user",
                    principalColumn: "row_id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "ix_project_name",
            table: "project",
            column: "name");

        migrationBuilder.CreateIndex(
            name: "ix_project_technical_name",
            table: "project",
            column: "technical_name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_project_user_user_id",
            table: "project_user",
            column: "user_id");

        migrationBuilder.CreateIndex(
            name: "ix_user_user_name",
            table: "user",
            column: "user_name");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "project_user");

        migrationBuilder.DropTable(
            name: "project");

        migrationBuilder.DropTable(
            name: "user");
    }
}
