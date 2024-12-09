using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Srv.Migrations;

/// <inheritdoc />
public partial class TaskWi : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "connecting_wi_db",
            columns: table => new
            {
                rowid = table.Column<long>(name: "row_id", type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                wiid = table.Column<int>(name: "wi_id", type: "int", nullable: false),
                taskid = table.Column<long>(name: "task_id", type: "bigint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_connecting_wi_db", x => x.rowid);
            });

        migrationBuilder.CreateIndex(
            name: "ix_connecting_wi_db_task_id",
            table: "connecting_wi_db",
            column: "task_id");

        migrationBuilder.CreateIndex(
            name: "ix_connecting_wi_db_wi_id",
            table: "connecting_wi_db",
            column: "wi_id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "connecting_wi_db");
    }
}
