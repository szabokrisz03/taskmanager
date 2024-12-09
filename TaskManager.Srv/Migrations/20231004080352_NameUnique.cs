using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Srv.Migrations
{
    /// <inheritdoc />
    public partial class NameUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_task_milestone_name",
                table: "task_milestone");

            migrationBuilder.DropIndex(
                name: "ix_task_milestone_task_id",
                table: "task_milestone");

            migrationBuilder.DropIndex(
                name: "ix_project_task_project_id_name",
                table: "project_task");

            migrationBuilder.CreateIndex(
                name: "ix_task_milestone_task_id_name",
                table: "task_milestone",
                columns: new[] { "task_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_project_task_project_id_name",
                table: "project_task",
                columns: new[] { "project_id", "name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_task_milestone_task_id_name",
                table: "task_milestone");

            migrationBuilder.DropIndex(
                name: "ix_project_task_project_id_name",
                table: "project_task");

            migrationBuilder.CreateIndex(
                name: "ix_task_milestone_name",
                table: "task_milestone",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_task_milestone_task_id",
                table: "task_milestone",
                column: "task_id");

            migrationBuilder.CreateIndex(
                name: "ix_project_task_project_id_name",
                table: "project_task",
                columns: new[] { "project_id", "name" });
        }
    }
}
