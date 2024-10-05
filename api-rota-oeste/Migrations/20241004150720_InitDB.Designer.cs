﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using api_rota_oeste.Data;

#nullable disable

namespace api_rota_oeste.Migrations
{
    [DbContext(typeof(ApiDbContext))]
    [Migration("20241004150720_InitDB")]
    partial class InitDB
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("api_rota_oeste.Models.Alternativa.AlternativaModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Codigo")
                        .HasColumnType("int")
                        .HasColumnName("codigo");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("descricao");

                    b.Property<int>("QuestaoId")
                        .HasColumnType("int")
                        .HasColumnName("id_questao");

                    b.HasKey("Id");

                    b.HasIndex("QuestaoId");

                    b.HasIndex("Id", "QuestaoId")
                        .IsUnique();

                    b.ToTable("alternativa");
                });

            modelBuilder.Entity("api_rota_oeste.Models.CheckList.CheckListModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("DataCriacao")
                        .IsRequired()
                        .HasColumnType("datetime2")
                        .HasColumnName("data_criacao");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)")
                        .HasColumnName("nome");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int")
                        .HasColumnName("id_usuario");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId");

                    b.ToTable("checklist");
                });

            modelBuilder.Entity("api_rota_oeste.Models.Cliente.ClienteModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<byte[]>("Foto")
                        .HasColumnType("VARBINARY(MAX)")
                        .HasColumnName("foto");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)")
                        .HasColumnName("nome");

                    b.Property<string>("Telefone")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)")
                        .HasColumnName("telefone");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int")
                        .HasColumnName("id_usuario");

                    b.HasKey("Id");

                    b.HasIndex("Telefone")
                        .IsUnique();

                    b.HasIndex("UsuarioId");

                    b.ToTable("cliente");
                });

            modelBuilder.Entity("api_rota_oeste.Models.ClienteRespondeCheckList.ClienteRespondeCheckListModel", b =>
                {
                    b.Property<int>("ClienteId")
                        .HasColumnType("int")
                        .HasColumnName("id_cliente");

                    b.Property<int>("CheckListId")
                        .HasColumnType("int")
                        .HasColumnName("id_checklist");

                    b.HasKey("ClienteId", "CheckListId");

                    b.HasIndex("CheckListId");

                    b.HasIndex("ClienteId", "CheckListId")
                        .IsUnique();

                    b.ToTable("cliente_responde_checklist");
                });

            modelBuilder.Entity("api_rota_oeste.Models.Interacao.InteracaoModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CheckListId")
                        .HasColumnType("int")
                        .HasColumnName("id_checklist");

                    b.Property<int>("ClienteId")
                        .HasColumnType("int")
                        .HasColumnName("id_cliente");

                    b.Property<DateTime>("Data")
                        .HasColumnType("DATETIME2")
                        .HasColumnName("data_criacao");

                    b.Property<bool>("Status")
                        .HasColumnType("BIT")
                        .HasColumnName("status");

                    b.HasKey("Id");

                    b.HasIndex("CheckListId");

                    b.HasIndex("ClienteId", "CheckListId")
                        .IsUnique();

                    b.ToTable("interacao");
                });

            modelBuilder.Entity("api_rota_oeste.Models.Questao.QuestaoModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CheckListId")
                        .HasColumnType("int")
                        .HasColumnName("id_checklist");

                    b.Property<int>("Tipo")
                        .HasColumnType("int")
                        .HasColumnName("tipo");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("nvarchar(120)")
                        .HasColumnName("titulo");

                    b.HasKey("Id");

                    b.HasIndex("CheckListId");

                    b.ToTable("questao");
                });

            modelBuilder.Entity("api_rota_oeste.Models.RespostaAlternativa.RespostaModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<byte[]>("Foto")
                        .HasColumnType("VARBINARY(MAX)")
                        .HasColumnName("foto");

                    b.Property<int>("InteracaoId")
                        .HasColumnType("int")
                        .HasColumnName("id_interacao");

                    b.Property<int>("QuestaoId")
                        .HasColumnType("int")
                        .HasColumnName("id_questao");

                    b.HasKey("Id");

                    b.HasIndex("InteracaoId");

                    b.HasIndex("QuestaoId");

                    b.ToTable("resposta");
                });

            modelBuilder.Entity("api_rota_oeste.Models.RespostaTemAlternativa.RespostaTemAlternativaModel", b =>
                {
                    b.Property<int>("AlternativaId")
                        .HasColumnType("int");

                    b.Property<int>("RespostaId")
                        .HasColumnType("int");

                    b.HasKey("AlternativaId", "RespostaId");

                    b.HasIndex("RespostaId", "AlternativaId")
                        .IsUnique();

                    b.ToTable("resposta_tem_alternativa");
                });

            modelBuilder.Entity("api_rota_oeste.Models.Usuario.UsuarioModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<byte[]>("Foto")
                        .HasColumnType("VARBINARY(MAX)")
                        .HasColumnName("foto");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)")
                        .HasColumnName("nome");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)")
                        .HasColumnName("senha");

                    b.Property<string>("Telefone")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)")
                        .HasColumnName("telefone");

                    b.HasKey("Id");

                    b.HasIndex("Telefone")
                        .IsUnique();

                    b.ToTable("usuario");
                });

            modelBuilder.Entity("api_rota_oeste.Models.Alternativa.AlternativaModel", b =>
                {
                    b.HasOne("api_rota_oeste.Models.Questao.QuestaoModel", "Questao")
                        .WithMany("AlternativaModels")
                        .HasForeignKey("QuestaoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Questao");
                });

            modelBuilder.Entity("api_rota_oeste.Models.CheckList.CheckListModel", b =>
                {
                    b.HasOne("api_rota_oeste.Models.Usuario.UsuarioModel", "Usuario")
                        .WithMany("CheckLists")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("api_rota_oeste.Models.Cliente.ClienteModel", b =>
                {
                    b.HasOne("api_rota_oeste.Models.Usuario.UsuarioModel", "Usuario")
                        .WithMany("Clientes")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("api_rota_oeste.Models.ClienteRespondeCheckList.ClienteRespondeCheckListModel", b =>
                {
                    b.HasOne("api_rota_oeste.Models.CheckList.CheckListModel", "CheckList")
                        .WithMany("ClienteRespondeCheckLists")
                        .HasForeignKey("CheckListId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("api_rota_oeste.Models.Cliente.ClienteModel", "Cliente")
                        .WithMany("ClienteRespondeCheckLists")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("CheckList");

                    b.Navigation("Cliente");
                });

            modelBuilder.Entity("api_rota_oeste.Models.Interacao.InteracaoModel", b =>
                {
                    b.HasOne("api_rota_oeste.Models.CheckList.CheckListModel", "CheckList")
                        .WithMany()
                        .HasForeignKey("CheckListId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("api_rota_oeste.Models.Cliente.ClienteModel", "Cliente")
                        .WithMany("Interacoes")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CheckList");

                    b.Navigation("Cliente");
                });

            modelBuilder.Entity("api_rota_oeste.Models.Questao.QuestaoModel", b =>
                {
                    b.HasOne("api_rota_oeste.Models.CheckList.CheckListModel", "CheckList")
                        .WithMany("Questoes")
                        .HasForeignKey("CheckListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CheckList");
                });

            modelBuilder.Entity("api_rota_oeste.Models.RespostaAlternativa.RespostaModel", b =>
                {
                    b.HasOne("api_rota_oeste.Models.Interacao.InteracaoModel", "Interacao")
                        .WithMany("RespostaAlternativaModels")
                        .HasForeignKey("InteracaoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("api_rota_oeste.Models.Questao.QuestaoModel", "Questao")
                        .WithMany("RespostaModels")
                        .HasForeignKey("QuestaoId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Interacao");

                    b.Navigation("Questao");
                });

            modelBuilder.Entity("api_rota_oeste.Models.RespostaTemAlternativa.RespostaTemAlternativaModel", b =>
                {
                    b.HasOne("api_rota_oeste.Models.Alternativa.AlternativaModel", "Alternativa")
                        .WithMany("RespostaTemAlternativaModels")
                        .HasForeignKey("AlternativaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api_rota_oeste.Models.RespostaAlternativa.RespostaModel", "Resposta")
                        .WithMany("RespostaTemAlternativaModels")
                        .HasForeignKey("RespostaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Alternativa");

                    b.Navigation("Resposta");
                });

            modelBuilder.Entity("api_rota_oeste.Models.Alternativa.AlternativaModel", b =>
                {
                    b.Navigation("RespostaTemAlternativaModels");
                });

            modelBuilder.Entity("api_rota_oeste.Models.CheckList.CheckListModel", b =>
                {
                    b.Navigation("ClienteRespondeCheckLists");

                    b.Navigation("Questoes");
                });

            modelBuilder.Entity("api_rota_oeste.Models.Cliente.ClienteModel", b =>
                {
                    b.Navigation("ClienteRespondeCheckLists");

                    b.Navigation("Interacoes");
                });

            modelBuilder.Entity("api_rota_oeste.Models.Interacao.InteracaoModel", b =>
                {
                    b.Navigation("RespostaAlternativaModels");
                });

            modelBuilder.Entity("api_rota_oeste.Models.Questao.QuestaoModel", b =>
                {
                    b.Navigation("AlternativaModels");

                    b.Navigation("RespostaModels");
                });

            modelBuilder.Entity("api_rota_oeste.Models.RespostaAlternativa.RespostaModel", b =>
                {
                    b.Navigation("RespostaTemAlternativaModels");
                });

            modelBuilder.Entity("api_rota_oeste.Models.Usuario.UsuarioModel", b =>
                {
                    b.Navigation("CheckLists");

                    b.Navigation("Clientes");
                });
#pragma warning restore 612, 618
        }
    }
}
