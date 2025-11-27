using System;

namespace DesafioDev.Models
{
    public class Movimentacao
    {
        public int Id { get; set; }
        public int CodigoProduto { get; set; }
        public string DescricaoProduto { get; set; }
        public string TipoMovimentacao { get; set; } // "Entrada" ou "Saída"
        public int Quantidade { get; set; }
        public DateTime DataMovimentacao { get; set; }
        public int EstoqueAntes { get; set; }
        public int EstoqueDepois { get; set; }
    }
}
