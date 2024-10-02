using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_rota_oeste.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAttrFoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "usuario",
                newName: "nome");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "usuario",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "resposta_alternativa",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "questao",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "interacao",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "cliente",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "checklist",
                newName: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "nome",
                table: "usuario",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "usuario",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "resposta_alternativa",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "questao",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "interacao",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "cliente",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "checklist",
                newName: "Id");
        }
    }
}
