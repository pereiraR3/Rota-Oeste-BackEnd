using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_rota_oeste.Migrations
{
    /// <inheritdoc />
    public partial class IniTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Telefone = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Foto = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "checklist",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_checklist", x => x.Id);
                    table.ForeignKey(
                        name: "FK_checklist_usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cliente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_usuario = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Foto = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cliente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cliente_usuario_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "questao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CheckListId = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_questao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_questao_checklist_CheckListId",
                        column: x => x.CheckListId,
                        principalTable: "checklist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "interacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    CheckListId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "BIT", nullable: false),
                    Data = table.Column<DateTime>(type: "DATETIME2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_interacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_interacao_checklist_CheckListId",
                        column: x => x.CheckListId,
                        principalTable: "checklist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_interacao_cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "resposta_alternativa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestaoId = table.Column<int>(type: "int", nullable: false),
                    InteracaoId = table.Column<int>(type: "int", nullable: false),
                    Alternativa = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resposta_alternativa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_resposta_alternativa_interacao_InteracaoId",
                        column: x => x.InteracaoId,
                        principalTable: "interacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_resposta_alternativa_questao_QuestaoId",
                        column: x => x.QuestaoId,
                        principalTable: "questao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_checklist_UsuarioId",
                table: "checklist",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_cliente_id_usuario",
                table: "cliente",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_cliente_Telefone",
                table: "cliente",
                column: "Telefone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_interacao_CheckListId",
                table: "interacao",
                column: "CheckListId");

            migrationBuilder.CreateIndex(
                name: "IX_interacao_ClienteId",
                table: "interacao",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_questao_CheckListId",
                table: "questao",
                column: "CheckListId");

            migrationBuilder.CreateIndex(
                name: "IX_resposta_alternativa_InteracaoId",
                table: "resposta_alternativa",
                column: "InteracaoId");

            migrationBuilder.CreateIndex(
                name: "IX_resposta_alternativa_QuestaoId",
                table: "resposta_alternativa",
                column: "QuestaoId");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_Telefone",
                table: "usuario",
                column: "Telefone",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "resposta_alternativa");

            migrationBuilder.DropTable(
                name: "interacao");

            migrationBuilder.DropTable(
                name: "questao");

            migrationBuilder.DropTable(
                name: "cliente");

            migrationBuilder.DropTable(
                name: "checklist");

            migrationBuilder.DropTable(
                name: "usuario");
        }
    }
}
