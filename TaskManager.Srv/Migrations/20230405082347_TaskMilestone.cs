using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Srv.Migrations;

/// <inheritdoc />
public partial class TaskMilestone : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<DateTime>(
            name: "actual",
            table: "task_milestone",
            type: "datetime2",
            nullable: true,
            defaultValueSql: "GETDATE()",
            oldClrType: typeof(DateTime),
            oldType: "datetime2",
            oldNullable: true);

        migrationBuilder.AddColumn<string>(
            name: "name",
            table: "task_milestone",
            type: "nvarchar(450)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<long>(
            name: "project_task_row_id",
            table: "task_milestone",
            type: "bigint",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "ix_task_milestone_name",
            table: "task_milestone",
            column: "name");

        migrationBuilder.CreateIndex(
            name: "ix_task_milestone_project_task_row_id",
            table: "task_milestone",
            column: "project_task_row_id");

        migrationBuilder.AddForeignKey(
            name: "fk_task_milestone_project_task_project_task_row_id",
            table: "task_milestone",
            column: "project_task_row_id",
            principalTable: "project_task",
            principalColumn: "row_id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "fk_task_milestone_project_task_project_task_row_id",
            table: "task_milestone");

        migrationBuilder.DropIndex(
            name: "ix_task_milestone_name",
            table: "task_milestone");

        migrationBuilder.DropIndex(
            name: "ix_task_milestone_project_task_row_id",
            table: "task_milestone");

        migrationBuilder.DropColumn(
            name: "name",
            table: "task_milestone");

        migrationBuilder.DropColumn(
            name: "project_task_row_id",
            table: "task_milestone");

        migrationBuilder.AlterColumn<DateTime>(
            name: "actual",
            table: "task_milestone",
            type: "datetime2",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldType: "datetime2",
            oldNullable: true,
            oldDefaultValueSql: "GETDATE()");
    }
}
