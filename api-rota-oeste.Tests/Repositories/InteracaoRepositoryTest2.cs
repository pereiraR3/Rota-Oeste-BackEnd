using api_rota_oeste.Data;
using api_rota_oeste.Repositories.Interfaces;
using api_rota_oeste.Repositories;
using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using api_rota_oeste.Models.CheckList;
using api_rota_oeste.Models.Usuario;
using api_rota_oeste.Models.Interacao;
using api_rota_oeste.Models.Cliente;
using JetBrains.Annotations;

namespace api_rota_oeste.Tests.Repositories
{
    public class InteracaoRepositoryTest2
    {
        private readonly DbContextOptions<ApiDBContext> options;
        private readonly ApiDBContext _context;
        
        public InteracaoRepositoryTest2()
        {
            // Configuração do DbContext para usar o InMemoryDatabase
            options = new DbContextOptionsBuilder<ApiDBContext>()
                .UseInMemoryDatabase(databaseName: "ApiRotaOesteTestDB")
            .Options;

            _context = new ApiDBContext(options);

            using (var _context = new ApiDBContext(options)) 
            { 
                _context.Database.EnsureDeleted();
                _context.Database.EnsureCreated();

                _context.Usuarios.Add(new UsuarioModel { Id = 1, Nome = "Eletrônicos", Telefone = "123456789", Senha = "ijngrt" });
                _context.Clientes.Add(new ClienteModel { Id = 1, Nome = "Fornecedor A", UsuarioId = 1, Telefone = "986546643" });
                _context.CheckLists.Add(new CheckListModel { Id = 1, Nome = "Smartphone", UsuarioId = 1 });
                _context.Interacoes.Add(new InteracaoModel { Id = 1, CheckListId = 1, ClienteId = 1, Status = true, Data = DateTime.Now });
                _context.SaveChangesAsync();
            }

        }

        [Fact]
        public async Task Atualizar()
        {
            using(var _context = new ApiDBContext(options))
            {
                var intRepository = new InteracaoRepository(_context);
                var interacao = await intRepository.BuscarPorId(1);
                Assert.NotNull(interacao);

                var mybool = false;

                interacao.Status = mybool;
                await intRepository.Atualizar(interacao);
            }

            using (var context = new ApiDBContext(options)) 
            {
                var mybool = false;

                var novaInteracao = await _context.Interacoes.FindAsync(1);
                Assert.Equal(mybool, novaInteracao.Status);
            }

        }



    }
}
