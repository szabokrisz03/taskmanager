using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Srv.Migrations;

/// <inheritdoc />
public partial class WiLinkTemplates : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "wi_link_template",
            columns: table => new
            {
                rowid = table.Column<long>(name: "row_id", type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                projectid = table.Column<long>(name: "project_id", type: "bigint", nullable: false),
                name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                witype = table.Column<int>(name: "wi_type", type: "int", nullable: false),
                teamproject = table.Column<string>(name: "team_project", type: "nvarchar(max)", nullable: false),
                iteration = table.Column<int>(type: "int", nullable: false),
                assignedto = table.Column<string>(name: "assigned_to", type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_wi_link_template", x => x.rowid);
                table.ForeignKey(
                    name: "fk_wi_link_template_project_project_id",
                    column: x => x.projectid,
                    principalTable: "project",
                    principalColumn: "row_id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "ix_wi_link_template_project_id",
            table: "wi_link_template",
            column: "project_id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "wi_link_template");
    }
}
