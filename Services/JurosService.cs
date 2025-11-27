using System;

namespace DesafioDev.Services
{
    public class JurosService
    {
        private const decimal TAXA_DIARIA = 0.025m; // 2.5% ao dia

        public void CalcularJuros()
        {
            Console.WriteLine("\n= CÁLCULO DE MULTA POR ATRASO =\n");

            // Valor da dívida
            Console.Write("Digite o valor da dívida (R$): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal valorDivida) || valorDivida <= 0)
            {
                Console.WriteLine("Valor inválido!");
                return;
            }

            // Data de vencimento
            Console.Write("Digite a data de vencimento (dd/MM/yyyy): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime dataVencimento))
            {
                Console.WriteLine("Data inválida!");
                return;
            }

            DateTime dataHoje = DateTime.Now.Date;

            if (dataHoje <= dataVencimento)
            {
                Console.WriteLine("\n= RESULTADO =");
                Console.WriteLine($"Valor da dívida: R$ {valorDivida:F2}");
                Console.WriteLine($"Data de vencimento: {dataVencimento:dd/MM/yyyy}");
                Console.WriteLine("Situação: ADIMPLENTE - Sem atraso!");
                Console.WriteLine("Multa: R$ 0,00");
                Console.WriteLine("Valor total: R$ " + valorDivida.ToString("F2"));
                Console.WriteLine("======\n");
                return;
            }

            int diasAtraso = (int)(dataHoje - dataVencimento).TotalDays;
            decimal multa = CalcularMulta(valorDivida, diasAtraso);
            decimal valorTotal = valorDivida + multa;

            Console.WriteLine("\n=== RESULTADO ===");
            Console.WriteLine($"Valor original: R$ {valorDivida:F2}");
            Console.WriteLine($"Data de vencimento: {dataVencimento:dd/MM/yyyy}");
            Console.WriteLine($"Data de hoje: {dataHoje:dd/MM/yyyy}");
            Console.WriteLine($"Dias em atraso: {diasAtraso}");
            Console.WriteLine($"Taxa de multa por dia: {TAXA_DIARIA * 100}%");
            Console.WriteLine("-------------");
            Console.WriteLine($"Multa total: R$ {multa:F2}");
            Console.WriteLine($"Valor total a pagar: R$ {valorTotal:F2}");
            Console.WriteLine("=======\n");
        }

        private decimal CalcularMulta(decimal valorDivida, int diasAtraso)
        {
            // Fórmula: Multa = Valor × (1 + taxa)^dias - Valor
            // Ou: Multa = Valor × ((1 + taxa)^dias - 1)
            
            // Limitar a 365 dias para evitar overflow em valores muito altos
            if (diasAtraso > 365)
            {
                Console.WriteLine("\n Aviso: Atraso muito grande! Limitando a 365 dias para cálculo.");
                diasAtraso = 365;
            }
            
            decimal taxa = 1 + TAXA_DIARIA;
            decimal fatorJuros = (decimal)Math.Pow((double)taxa, diasAtraso);
            decimal multa = valorDivida * (fatorJuros - 1);

            return multa;
        }
    }
}
