using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_rota_oeste.Migrations
{
    /// <inheritdoc />
    public partial class InitDBStart : Migration
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
                    telefone = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    senha = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    foto = table.Column<byte[]>(type: "VARBINARY(MAX)", nullable: true)
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
                    id_usuario = table.Column<int>(type: "int", nullable: false),
                    nome = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    data_criacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_checklist", x => x.Id);
                    table.ForeignKey(
                        name: "FK_checklist_usuario_id_usuario",
                        column: x => x.id_usuario,
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
                    nome = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    telefone = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    foto = table.Column<byte[]>(type: "VARBINARY(MAX)", nullable: true)
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
                    id_checklist = table.Column<int>(type: "int", nullable: false),
                    titulo = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_questao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_questao_checklist_id_checklist",
                        column: x => x.id_checklist,
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
                    id_cliente = table.Column<int>(type: "int", nullable: false),
                    id_checklist = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<bool>(type: "BIT", nullable: false),
                    data_criacao = table.Column<DateTime>(type: "DATETIME2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_interacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_interacao_checklist_id_checklist",
                        column: x => x.id_checklist,
                        principalTable: "checklist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_interacao_cliente_id_cliente",
                        column: x => x.id_cliente,
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
                    id_questao = table.Column<int>(type: "int", nullable: false),
                    id_interacao = table.Column<int>(type: "int", nullable: false),
                    foto = table.Column<byte[]>(type: "VARBINARY(MAX)", nullable: true),
                    Alternativa = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resposta_alternativa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_resposta_alternativa_interacao_id_interacao",
                        column: x => x.id_interacao,
                        principalTable: "interacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_resposta_alternativa_questao_id_questao",
                        column: x => x.id_questao,
                        principalTable: "questao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_checklist_id_usuario",
                table: "checklist",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_cliente_id_usuario",
                table: "cliente",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_cliente_telefone",
                table: "cliente",
                column: "telefone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_interacao_id_checklist",
                table: "interacao",
                column: "id_checklist");

            migrationBuilder.CreateIndex(
                name: "IX_interacao_id_cliente",
                table: "interacao",
                column: "id_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_questao_id_checklist",
                table: "questao",
                column: "id_checklist");

            migrationBuilder.CreateIndex(
                name: "IX_resposta_alternativa_id_interacao",
                table: "resposta_alternativa",
                column: "id_interacao");

            migrationBuilder.CreateIndex(
                name: "IX_resposta_alternativa_id_questao",
                table: "resposta_alternativa",
                column: "id_questao");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_telefone",
                table: "usuario",
                column: "telefone",
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
