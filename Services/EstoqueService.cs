using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DesafioDev.Models;

namespace DesafioDev.Services
{
    public class EstoqueService
    {
        private List<Produto> produtos;
        private List<Movimentacao> movimentacoes;
        private int proximoIdMovimentacao = 1;

        public EstoqueService()
        {
            produtos = new List<Produto>();
            movimentacoes = new List<Movimentacao>();
        }

        public void CarregarEstoque(string caminhoJson)
        {
            try
            {
                string jsonContent = System.IO.File.ReadAllText(caminhoJson);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var dados = JsonSerializer.Deserialize<EstoqueResponse>(jsonContent, options);

                if (dados != null && dados.Estoque != null)
                {
                    produtos = dados.Estoque.ToList();
                    Console.WriteLine($"\n{produtos.Count} produto(s) carregado(s) com sucesso!\n");
                }
                else
                {
                    Console.WriteLine("Nenhum dado de estoque encontrado.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar o arquivo: {ex.Message}");
            }
        }

        public void ExibirProdutos()
        {
            if (produtos.Count == 0)
            {
                Console.WriteLine("Nenhum produto carregado. Carregue primeiro o arquivo de estoque.");
                return;
            }

            Console.WriteLine("\n= ESTOQUE ATUAL =\n");
            foreach (var produto in produtos)
            {
                Console.WriteLine($"Código: {produto.CodigoProduto} | " +
                    $"Descrição: {produto.DescricaoProduto} | " +
                    $"Estoque: {produto.Estoque}");
            }
            Console.WriteLine();
        }

        public void RegistrarMovimentacao()
        {
            if (produtos.Count == 0)
            {
                Console.WriteLine("\nNenhum produto carregado. Carregue primeiro o arquivo de estoque.");
                return;
            }

            Console.WriteLine("\n= NOVA MOVIMENTAÇÃO =\n");

            // Selecionar produto
            Console.WriteLine("Produtos disponíveis:");
            ExibirProdutos();

            Console.Write("Digite o código do produto: ");
            if (!int.TryParse(Console.ReadLine(), out int codigoProduto))
            {
                Console.WriteLine("Código inválido!");
                return;
            }

            var produto = produtos.FirstOrDefault(p => p.CodigoProduto == codigoProduto);
            if (produto == null)
            {
                Console.WriteLine("Produto não encontrado!");
                return;
            }

            // Selecionar tipo de movimentação
            Console.WriteLine("\nTipo de movimentação:");
            Console.WriteLine("1 - Entrada");
            Console.WriteLine("2 - Saída");
            Console.Write("Escolha (1 ou 2): ");
            string escolha = Console.ReadLine();

            string tipoMovimentacao = escolha == "1" ? "Entrada" : (escolha == "2" ? "Saída" : "");
            if (string.IsNullOrEmpty(tipoMovimentacao))
            {
                Console.WriteLine("Opção inválida!");
                return;
            }

            // Quantidade
            Console.Write("Quantidade: ");
            if (!int.TryParse(Console.ReadLine(), out int quantidade) || quantidade <= 0)
            {
                Console.WriteLine("Quantidade inválida!");
                return;
            }

            // Validar saída
            if (tipoMovimentacao == "Saída" && quantidade > produto.Estoque)
            {
                Console.WriteLine($"Erro! Estoque insuficiente. Disponível: {produto.Estoque}");
                return;
            }

            // Registrar movimentação
            int estoqueAntes = produto.Estoque;
            int estoqueDepois = tipoMovimentacao == "Entrada" 
                ? estoqueAntes + quantidade 
                : estoqueAntes - quantidade;

            produto.Estoque = estoqueDepois;

            var movimentacao = new Movimentacao
            {
                Id = proximoIdMovimentacao++,
                CodigoProduto = codigoProduto,
                DescricaoProduto = produto.DescricaoProduto,
                TipoMovimentacao = tipoMovimentacao,
                Quantidade = quantidade,
                DataMovimentacao = DateTime.Now,
                EstoqueAntes = estoqueAntes,
                EstoqueDepois = estoqueDepois
            };

            movimentacoes.Add(movimentacao);

            Console.WriteLine("\n= MOVIMENTAÇÃO REGISTRADA =");
            Console.WriteLine($"ID: {movimentacao.Id}");
            Console.WriteLine($"Produto: {movimentacao.DescricaoProduto}");
            Console.WriteLine($"Tipo: {movimentacao.TipoMovimentacao}");
            Console.WriteLine($"Quantidade: {movimentacao.Quantidade}");
            Console.WriteLine($"Estoque antes: {movimentacao.EstoqueAntes}");
            Console.WriteLine($"Estoque depois: {movimentacao.EstoqueDepois}");
            Console.WriteLine($"Data/Hora: {movimentacao.DataMovimentacao:dd/MM/yyyy HH:mm:ss}");
            Console.WriteLine("===\n");
        }

        public void ExibirHistoricoMovimentacoes()
        {
            if (movimentacoes.Count == 0)
            {
                Console.WriteLine("\nNenhuma movimentação registrada.");
                return;
            }

            Console.WriteLine("\n= HISTÓRICO DE MOVIMENTAÇÕES =\n");
            foreach (var mov in movimentacoes)
            {
                Console.WriteLine($"ID: {mov.Id} | Data: {mov.DataMovimentacao:dd/MM/yyyy HH:mm:ss} | " +
                    $"Produto: {mov.DescricaoProduto} | Tipo: {mov.TipoMovimentacao} | " +
                    $"Qtd: {mov.Quantidade} | Antes: {mov.EstoqueAntes} → Depois: {mov.EstoqueDepois}");
            }
            Console.WriteLine();
        }
    }
}
