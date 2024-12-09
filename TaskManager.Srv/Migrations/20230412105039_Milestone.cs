using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Srv.Migrations;

/// <inheritdoc />
public partial class Milestone : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
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

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
