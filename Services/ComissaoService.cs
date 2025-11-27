using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DesafioDev.Models;

namespace DesafioDev.Services
{
    public class ComissaoService
    {
        public void CalcularComissoes(string caminhoJson)
        {
            try
            {
                string jsonContent = System.IO.File.ReadAllText(caminhoJson);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var dados = JsonSerializer.Deserialize<VendasResponse>(jsonContent, options);

                if (dados == null || dados.Vendas == null || dados.Vendas.Count == 0)
                {
                    Console.WriteLine("Nenhum dado de vendas encontrado.");
                    return;
                }

                // vendas por vendedor
                var vendedores = dados.Vendas
                    .GroupBy(v => v.Vendedor)
                    .OrderBy(g => g.Key)
                    .ToList();

                Console.WriteLine("\n= CÁLCULO DE COMISSÕES =\n");

                decimal comissaoTotal = 0;

                foreach (var grupo in vendedores)
                {
                    Console.WriteLine($"Vendedor: {grupo.Key}");
                    Console.WriteLine(new string('-', 60));

                    decimal comissaoVendedor = 0;
                    decimal totalVendas = 0;

                    foreach (var venda in grupo)
                    {
                        decimal comissao = CalcularComissaoVenda(venda.Valor);
                        comissaoVendedor += comissao;
                        totalVendas += venda.Valor;

                        string statusComissao = comissao == 0 ? "Sem comissão" : $"Comissão: R$ {comissao:F2}";
                        Console.WriteLine($"  Venda: R$ {venda.Valor:F2} -> {statusComissao}");
                    }

                    Console.WriteLine(new string('-', 60));
                    Console.WriteLine($"Total de vendas: R$ {totalVendas:F2}");
                    Console.WriteLine($"Comissão total: R$ {comissaoVendedor:F2}");
                    Console.WriteLine();

                    comissaoTotal += comissaoVendedor;
                }

                Console.WriteLine($"Comissão Total de Todos os Vendedores: R$ {comissaoTotal:F2}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar o arquivo: {ex.Message}");
            }
        }

        private decimal CalcularComissaoVenda(decimal valor)
        {
            if (valor < 100)
                return 0; // Sem comissão

            if (valor < 500)
                return valor * 0.01m; // 1% de comissão

            return valor * 0.05m; // 5% de comissão
        }
    }
}
