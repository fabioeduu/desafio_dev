using System;
using System.IO;
using DesafioDev.Services;

var comissao = new ComissaoService();
var estoque = new EstoqueService();
var juros = new JurosService();

string vendas = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "vendas.json");
string produtos = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "estoque.json");

while (true)
{
    Console.Clear();
    Console.WriteLine("= MENU PRINCIPAL =\n");
    Console.WriteLine("1. Cálculo de Comissões de Vendedores");
    Console.WriteLine("2. Controle de Movimentações de Estoque");
    Console.WriteLine("3. Cálculo de Multa por Atraso");
    Console.WriteLine("0. Sair");
    Console.Write("\nOpção: ");
    
    string op = Console.ReadLine();
    Console.Clear();

    if (op == "1")
    {
        Console.WriteLine("CÁLCULO DE COMISSÕES\n");
        comissao.CalcularComissoes(vendas);
    }
    else if (op == "2")
    {
        Console.WriteLine("MOVIMENTAÇÕES DE ESTOQUE\n");
        estoque.CarregarEstoque(produtos);
        
        bool voltar = false;
        while (!voltar)
        {
            Console.WriteLine("\n1. Ver estoque | 2. Nova movimentação | 3. Histórico | 0. Voltar");
            Console.Write("Opção: ");
            
            string sub = Console.ReadLine();
            if (sub == "1") estoque.ExibirProdutos();
            else if (sub == "2") estoque.RegistrarMovimentacao();
            else if (sub == "3") estoque.ExibirHistoricoMovimentacoes();
            else if (sub == "0") voltar = true;
            else Console.WriteLine("Inválido!");
        }
    }
    else if (op == "3")
    {
        Console.WriteLine("MULTA POR ATRASO\n");
        juros.CalcularJuros();
    }
    else if (op == "0")
    {
        Console.WriteLine("Até logo!");
        break;
    }
    else
    {
        Console.WriteLine("Opção inválida!");
    }

    Console.WriteLine("\nPressione ENTER para continuar...");
    Console.ReadLine();
}
