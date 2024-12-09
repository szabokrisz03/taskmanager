using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Srv.Migrations;

/// <inheritdoc />
public partial class MakeItWork : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "full_name",
            table: "user",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.CreateTable(
            name: "comment_line",
            columns: table => new
            {
                rowid = table.Column<long>(name: "row_id", type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                taskid = table.Column<long>(name: "task_id", type: "bigint", nullable: false),
                comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                userid = table.Column<long>(name: "user_id", type: "bigint", nullable: false),
                creationdate = table.Column<DateTime>(name: "creation_date", type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_comment_line", x => x.rowid);
                table.ForeignKey(
                    name: "fk_comment_line_project_task_project_task_temp_id",
                    column: x => x.taskid,
                    principalTable: "project_task",
                    principalColumn: "row_id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "ix_project_task_project_id_name",
            table: "project_task",
            columns: new[] { "project_id", "name" });

        migrationBuilder.CreateIndex(
            name: "ix_comment_line_task_id",
            table: "comment_line",
            column: "task_id");

        migrationBuilder.CreateIndex(
            name: "ix_comment_line_user_id",
            table: "comment_line",
            column: "user_id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "comment_line");

        migrationBuilder.DropIndex(
            name: "ix_project_task_project_id_name",
            table: "project_task");

        migrationBuilder.DropColumn(
            name: "full_name",
            table: "user");
    }
}
