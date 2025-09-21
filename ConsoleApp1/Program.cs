using System;
using System.Collections.Generic;
using System.IO;

namespace Shop
{
    class Program
    {
        static List<Veiculo> estoque = new List<Veiculo>();
        static List<Veiculo> carrinho = new List<Veiculo>();
        static string pathCarrinho = "carrinho.txt";
        static string pathVendas = "vendas.txt";

        static void Main(string[] args)
        {
            InicializarEstoque();
            int opcao;

            do
            {
                Console.Clear();
                Console.WriteLine("=== Concessionária CMD ===");
                Console.WriteLine("1 - Listar Carros");
                Console.WriteLine("2 - Listar Motos");
                Console.WriteLine("3 - Adicionar ao Carrinho");
                Console.WriteLine("4 - Ver Carrinho");
                Console.WriteLine("5 - Finalizar Compra");
                Console.WriteLine("6 - Adicionar Produto no Estoque");
                Console.WriteLine("7 - Mostrar Itens Vendidos");
                Console.WriteLine("8 - Remover Item do Carrinho");
                Console.WriteLine("0 - Sair");
                Console.Write("Escolha uma opção: ");

                if (!int.TryParse(Console.ReadLine(), out opcao))
                {
                    opcao = -1;
                }

                switch (opcao)
                {
                    case 1:
                        ListarVeiculos("Carro");
                        break;
                    case 2:
                        ListarVeiculos("Moto");
                        break;
                    case 3:
                        AdicionarCarrinho();
                        break;
                    case 4:
                        VerCarrinho();
                        break;
                    case 5:
                        FinalizarCompra();
                        break;
                    case 6:
                        AdicionarProdutoEstoque();
                        break;
                    case 7:
                        MostrarItensVendidos();
                        break;
                    case 8:
                        RemoverDoCarrinho();
                        break;
                    case 0:
                        Console.WriteLine("Saindo...");
                        break;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }

                if (opcao != 0)
                {
                    Console.WriteLine("\nPressione qualquer tecla para continuar...");
                    Console.ReadKey();
                }

            } while (opcao != 0);
        }

        static void InicializarEstoque()
        {
            estoque.Add(new Veiculo("Carro", "Chevrolet Onix", 75000, 5));
            estoque.Add(new Veiculo("Carro", "Toyota Corolla", 130000, 3));
            estoque.Add(new Veiculo("Carro", "Honda Civic", 120000, 4));

            estoque.Add(new Veiculo("Moto", "Honda CG 160", 15000, 10));
            estoque.Add(new Veiculo("Moto", "Yamaha Fazer 250", 23000, 7));
            estoque.Add(new Veiculo("Moto", "BMW GS 850", 62000, 2));
        }

        static void ListarVeiculos(string tipo)
        {
            Console.Clear();
            Console.WriteLine($"=== Lista de {tipo}s ===");
            int i = 1;
            foreach (var v in estoque)
            {
                if (v.Tipo == tipo)
                {
                    Console.WriteLine($"{i} - {v.Nome} - R$ {v.Preco:N2} | Estoque: {v.Quantidade}");
                }
                i++;
            }
        }

        static void AdicionarCarrinho()
        {
            Console.Clear();
            Console.WriteLine("=== Estoque ===");
            for (int i = 0; i < estoque.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {estoque[i].Tipo} | {estoque[i].Nome} - R$ {estoque[i].Preco:N2} | Estoque: {estoque[i].Quantidade}");
            }

            Console.Write("Digite o número do veículo para adicionar ao carrinho: ");
            if (int.TryParse(Console.ReadLine(), out int escolha) && escolha > 0 && escolha <= estoque.Count)
            {
                var veiculo = estoque[escolha - 1];

                Console.Write($"Quantas unidades de {veiculo.Nome} deseja adicionar? ");
                if (!int.TryParse(Console.ReadLine(), out int quantidadeDesejada) || quantidadeDesejada <= 0)
                {
                    Console.WriteLine("Quantidade inválida!");
                    return;
                }

                if (veiculo.Quantidade >= quantidadeDesejada)
                {
                    carrinho.Add(new Veiculo(veiculo.Tipo, veiculo.Nome, veiculo.Preco, quantidadeDesejada));
                    veiculo.Quantidade -= quantidadeDesejada; // baixa do estoque
                    Console.WriteLine($"{quantidadeDesejada} unidade(s) de {veiculo.Nome} foram adicionadas ao carrinho.");
                    SalvarCarrinho();
                }
                else
                {
                    Console.WriteLine("Estoque insuficiente!");
                }
            }
            else
            {
                Console.WriteLine("Opção inválida!");
            }
        }

        static void VerCarrinho()
        {
            Console.Clear();
            Console.WriteLine("=== Seu Carrinho ===");
            if (carrinho.Count == 0)
            {
                Console.WriteLine("Carrinho vazio!");
            }
            else
            {
                int i = 1;
                foreach (var v in carrinho)
                {
                    Console.WriteLine($"{i} - {v.Tipo} | {v.Nome} - R$ {v.Preco:N2} x {v.Quantidade} = R$ {(v.Preco * v.Quantidade):N2}");
                    i++;
                }
            }
        }

        static void RemoverDoCarrinho()
        {
            Console.Clear();
            Console.WriteLine("=== Remover Item do Carrinho ===");

            if (carrinho.Count == 0)
            {
                Console.WriteLine("Carrinho vazio!");
                return;
            }

            for (int i = 0; i < carrinho.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {carrinho[i].Tipo} | {carrinho[i].Nome} - R$ {carrinho[i].Preco:N2} x {carrinho[i].Quantidade}");
            }

            Console.Write("Digite o número do item que deseja remover: ");
            if (int.TryParse(Console.ReadLine(), out int escolha) && escolha > 0 && escolha <= carrinho.Count)
            {
                var item = carrinho[escolha - 1];

                Console.Write($"Quantas unidades de {item.Nome} deseja remover? ");
                if (!int.TryParse(Console.ReadLine(), out int qtdRemover) || qtdRemover <= 0)
                {
                    Console.WriteLine("Quantidade inválida!");
                    return;
                }

                if (qtdRemover >= item.Quantidade)
                {
                    // devolve tudo ao estoque
                    var original = estoque.Find(v => v.Nome == item.Nome && v.Tipo == item.Tipo);
                    if (original != null) original.Quantidade += item.Quantidade;

                    carrinho.RemoveAt(escolha - 1);
                    Console.WriteLine($"{item.Nome} removido completamente do carrinho.");
                }
                else
                {
                    item.Quantidade -= qtdRemover;
                    var original = estoque.Find(v => v.Nome == item.Nome && v.Tipo == item.Tipo);
                    if (original != null) original.Quantidade += qtdRemover;

                    Console.WriteLine($"{qtdRemover} unidade(s) de {item.Nome} removidas do carrinho.");
                }

                SalvarCarrinho();
            }
            else
            {
                Console.WriteLine("Opção inválida!");
            }
        }

        static void FinalizarCompra()
        {
            Console.Clear();
            Console.WriteLine("=== Finalizar Compra ===");
            if (carrinho.Count == 0)
            {
                Console.WriteLine("Carrinho vazio, não é possível finalizar!");
                return;
            }

            double total = 0;
            foreach (var v in carrinho)
            {
                Console.WriteLine($"{v.Tipo} | {v.Nome} - R$ {v.Preco:N2} x {v.Quantidade} = R$ {(v.Preco * v.Quantidade):N2}");
                total += v.Preco * v.Quantidade;
            }

            Console.WriteLine($"\nTotal: R$ {total:N2}");
            Console.WriteLine("Compra finalizada com sucesso!");

            SalvarVenda();
            carrinho.Clear();
            File.WriteAllText(pathCarrinho, ""); 
        }

        static void AdicionarProdutoEstoque()
        {
            Console.Clear();
            Console.WriteLine("=== Adicionar Produto ao Estoque ===");
            Console.Write("Digite o tipo (Carro/Moto): ");
            string tipo = Console.ReadLine();

            if (tipo.ToLower() != "carro" && tipo.ToLower() != "moto")
            {
                Console.WriteLine("Tipo inválido! Só é permitido Carro ou Moto.");
                return;
            }

            Console.Write("Digite o nome do veículo: ");
            string nome = Console.ReadLine();

            Console.Write("Digite o preço: ");
            if (!double.TryParse(Console.ReadLine(), out double preco))
            {
                Console.WriteLine("Preço inválido!");
                return;
            }

            Console.Write("Digite a quantidade em estoque: ");
            if (!int.TryParse(Console.ReadLine(), out int quantidade) || quantidade < 0)
            {
                Console.WriteLine("Quantidade inválida!");
                return;
            }

            estoque.Add(new Veiculo(char.ToUpper(tipo[0]) + tipo.Substring(1).ToLower(), nome, preco, quantidade));
            Console.WriteLine($"{nome} foi adicionado ao estoque com {quantidade} unidades.");
        }

        static void MostrarItensVendidos()
        {
            Console.Clear();
            Console.WriteLine("=== Itens Vendidos ===");

            if (!File.Exists(pathVendas))
            {
                Console.WriteLine("Nenhuma venda registrada ainda.");
                return;
            }

            string[] linhas = File.ReadAllLines(pathVendas);
            if (linhas.Length == 0)
            {
                Console.WriteLine("Nenhuma venda registrada ainda.");
            }
            else
            {
                foreach (string linha in linhas)
                {
                    Console.WriteLine(linha);
                }
            }
        }

        static void SalvarCarrinho()
        {
            using (StreamWriter sw = new StreamWriter(pathCarrinho))
            {
                foreach (var v in carrinho)
                {
                    sw.WriteLine($"{v.Tipo} | {v.Nome} - R$ {v.Preco:N2} x {v.Quantidade}");
                }
            }
        }

        static void SalvarVenda()
        {
            using (StreamWriter sw = new StreamWriter(pathVendas, true))
            {
                sw.WriteLine($"Venda realizada em {DateTime.Now}");
                foreach (var v in carrinho)
                {
                    sw.WriteLine($"{v.Tipo} | {v.Nome} - R$ {v.Preco:N2} x {v.Quantidade} = R$ {(v.Preco * v.Quantidade):N2}");
                }
                sw.WriteLine("-----------------------------");
            }
        }
    }

    class Veiculo
    {
        public string Tipo { get; set; }
        public string Nome { get; set; }
        public double Preco { get; set; }
        public int Quantidade { get; set; } // estoque ou qtd no carrinho

        public Veiculo(string tipo, string nome, double preco, int quantidade)
        {
            Tipo = tipo;
            Nome = nome;
            Preco = preco;
            Quantidade = quantidade;
        }
    }
}
x