using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_rota_oeste.Migrations
{
    /// <inheritdoc />
    public partial class bancoNovo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    telefone = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    nome = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    senha = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    foto = table.Column<byte[]>(type: "VARBINARY(MAX)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "checklist",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_usuario = table.Column<int>(type: "int", nullable: false),
                    nome = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    data_criacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_checklist", x => x.id);
                    table.ForeignKey(
                        name: "FK_checklist_usuario_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "usuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cliente",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_usuario = table.Column<int>(type: "int", nullable: false),
                    nome = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    telefone = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    foto = table.Column<byte[]>(type: "VARBINARY(MAX)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cliente", x => x.id);
                    table.ForeignKey(
                        name: "FK_cliente_usuario_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "usuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "questao",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_checklist = table.Column<int>(type: "int", nullable: false),
                    titulo = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    tipo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_questao", x => x.id);
                    table.ForeignKey(
                        name: "FK_questao_checklist_id_checklist",
                        column: x => x.id_checklist,
                        principalTable: "checklist",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cliente_responde_checklist",
                columns: table => new
                {
                    id_cliente = table.Column<int>(type: "int", nullable: false),
                    id_checklist = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cliente_responde_checklist", x => new { x.id_cliente, x.id_checklist });
                    table.ForeignKey(
                        name: "FK_cliente_responde_checklist_checklist_id_checklist",
                        column: x => x.id_checklist,
                        principalTable: "checklist",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_cliente_responde_checklist_cliente_id_cliente",
                        column: x => x.id_cliente,
                        principalTable: "cliente",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "interacao",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_cliente = table.Column<int>(type: "int", nullable: false),
                    id_checklist = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<bool>(type: "BIT", nullable: false),
                    data_criacao = table.Column<DateTime>(type: "DATETIME2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_interacao", x => x.id);
                    table.ForeignKey(
                        name: "FK_interacao_checklist_id_checklist",
                        column: x => x.id_checklist,
                        principalTable: "checklist",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_interacao_cliente_id_cliente",
                        column: x => x.id_cliente,
                        principalTable: "cliente",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "alternativa",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_questao = table.Column<int>(type: "int", nullable: false),
                    descricao = table.Column<string>(type: "TEXT", nullable: false),
                    codigo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alternativa", x => x.id);
                    table.ForeignKey(
                        name: "FK_alternativa_questao_id_questao",
                        column: x => x.id_questao,
                        principalTable: "questao",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "resposta",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_questao = table.Column<int>(type: "int", nullable: false),
                    id_interacao = table.Column<int>(type: "int", nullable: false),
                    alternativa = table.Column<int>(type: "int", nullable: true),
                    foto = table.Column<byte[]>(type: "VARBINARY(MAX)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resposta", x => x.id);
                    table.ForeignKey(
                        name: "FK_resposta_interacao_id_interacao",
                        column: x => x.id_interacao,
                        principalTable: "interacao",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_resposta_questao_id_questao",
                        column: x => x.id_questao,
                        principalTable: "questao",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_alternativa_id_id_questao",
                table: "alternativa",
                columns: new[] { "id", "id_questao" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_alternativa_id_questao",
                table: "alternativa",
                column: "id_questao");

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
                name: "IX_cliente_responde_checklist_id_checklist",
                table: "cliente_responde_checklist",
                column: "id_checklist");

            migrationBuilder.CreateIndex(
                name: "IX_cliente_responde_checklist_id_cliente_id_checklist",
                table: "cliente_responde_checklist",
                columns: new[] { "id_cliente", "id_checklist" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_interacao_id_checklist",
                table: "interacao",
                column: "id_checklist");

            migrationBuilder.CreateIndex(
                name: "IX_interacao_id_cliente_id_checklist",
                table: "interacao",
                columns: new[] { "id_cliente", "id_checklist" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_questao_id_checklist",
                table: "questao",
                column: "id_checklist");

            migrationBuilder.CreateIndex(
                name: "IX_resposta_id_interacao",
                table: "resposta",
                column: "id_interacao");

            migrationBuilder.CreateIndex(
                name: "IX_resposta_id_questao",
                table: "resposta",
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
                name: "alternativa");

            migrationBuilder.DropTable(
                name: "cliente_responde_checklist");

            migrationBuilder.DropTable(
                name: "resposta");

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
