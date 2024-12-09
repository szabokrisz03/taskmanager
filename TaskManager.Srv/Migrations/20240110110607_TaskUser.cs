using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Srv.Migrations
{
    /// <inheritdoc />
    public partial class TaskUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "responsible_person",
                table: "project_task",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_project_task_responsible_person",
                table: "project_task",
                column: "responsible_person");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_project_task_responsible_person",
                table: "project_task");

            migrationBuilder.DropColumn(
                name: "responsible_person",
                table: "project_task");
        }
    }
}
