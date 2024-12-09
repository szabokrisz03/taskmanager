using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Srv.Migrations;

/// <inheritdoc />
public partial class ProjectTask : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "project_task",
            columns: table => new
            {
                rowid = table.Column<long>(name: "row_id", type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                projectid = table.Column<long>(name: "project_id", type: "bigint", nullable: false),
                name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                technicalname = table.Column<Guid>(name: "technical_name", type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                state = table.Column<int>(type: "int", nullable: false),
                projectrowid = table.Column<long>(name: "project_row_id", type: "bigint", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_project_task", x => x.rowid);
                table.ForeignKey(
                    name: "fk_project_task_project_project_id",
                    column: x => x.projectid,
                    principalTable: "project",
                    principalColumn: "row_id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_project_task_project_project_row_id",
                    column: x => x.projectrowid,
                    principalTable: "project",
                    principalColumn: "row_id");
            });

        migrationBuilder.CreateTable(
            name: "task_milestone",
            columns: table => new
            {
                rowid = table.Column<long>(name: "row_id", type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                taskid = table.Column<long>(name: "task_id", type: "bigint", nullable: false),
                milestoneid = table.Column<long>(name: "milestone_id", type: "bigint", nullable: false),
                planned = table.Column<DateTime>(type: "datetime2", nullable: false),
                actual = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_task_milestone", x => x.rowid);
                table.ForeignKey(
                    name: "fk_task_milestone_project_task_task_id",
                    column: x => x.taskid,
                    principalTable: "project_task",
                    principalColumn: "row_id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "ix_project_task_name",
            table: "project_task",
            column: "name");

        migrationBuilder.CreateIndex(
            name: "ix_project_task_project_id",
            table: "project_task",
            column: "project_id");

        migrationBuilder.CreateIndex(
            name: "ix_project_task_project_row_id",
            table: "project_task",
            column: "project_row_id");

        migrationBuilder.CreateIndex(
            name: "ix_project_task_technical_name",
            table: "project_task",
            column: "technical_name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_task_milestone_task_id",
            table: "task_milestone",
            column: "task_id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "task_milestone");

        migrationBuilder.DropTable(
            name: "project_task");
    }
}
