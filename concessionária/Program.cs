using System;
using System.Collections.Generic;
using System.IO;
using System.Linq; // Adicionado para facilitar algumas operações

namespace Shop
{
    class Program
    {
        static List<Veiculo> estoque = new List<Veiculo>();
        static List<Veiculo> carrinho = new List<Veiculo>();
        static string pathCarrinho = "carrinho.txt";
        static string pathVendas = "vendas.txt";
        static string pathEstoque = "estoque.txt";
        private const int LARGURA_MENU = 60; 

        static void Main(string[] args)
        {
            InicializarEstoque();
            int opcao;

            do
            {
                Console.Clear();
                
                // --- MENU PRINCIPAL ---
                DesenharBordaSuperior();
                DesenharLinhaTexto("CONCESSIONÁRIA CMD", ConsoleColor.Yellow, true);
                DesenharLinhaSeparadora();
                DesenharLinhaTexto("1 - Listar Carros", ConsoleColor.Cyan);
                DesenharLinhaTexto("2 - Listar Motos", ConsoleColor.Cyan);
                DesenharLinhaTexto("3 - Adicionar ao Carrinho", ConsoleColor.Cyan);
                DesenharLinhaTexto("4 - Ver Carrinho", ConsoleColor.Cyan);
                DesenharLinhaTexto("5 - Finalizar Compra", ConsoleColor.Cyan);
                DesenharLinhaSeparadora();
                DesenharLinhaTexto("6 - Adicionar Produto no Estoque", ConsoleColor.Yellow);
                DesenharLinhaTexto("7 - Remover Produto do Estoque", ConsoleColor.Yellow);
                DesenharLinhaTexto("8 - Mostrar Itens Vendidos", ConsoleColor.Yellow);
                DesenharLinhaSeparadora();
                DesenharLinhaTexto("9 - Remover Item do Carrinho");
                DesenharLinhaTexto("0 - Sair", ConsoleColor.Red);
                DesenharBordaInferior();

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("  Escolha uma opção: ");
                Console.ResetColor();

                if (!int.TryParse(Console.ReadLine(), out opcao))
                {
                    opcao = -1;
                }

                Console.WriteLine();

                switch (opcao)
                {
                    case 1: ListarVeiculos("Carro"); break;
                    case 2: ListarVeiculos("Moto"); break;
                    case 3: AdicionarCarrinho(); break;
                    case 4: VerCarrinho(); break;
                    case 5: FinalizarCompra(); break;
                    case 6: AdicionarProdutoEstoque(); break;
                    case 7: RemoverProdutoEstoque(); break;
                    case 8: MostrarItensVendidos(); break;
                    case 9: RemoverDoCarrinho(); break;
                    case 0:
                        ExibirMensagem("Obrigado por utilizar o sistema. Saindo...", ConsoleColor.Magenta);
                        break;
                    default:
                        ExibirMensagemErro("Opção inválida! Por favor, tente novamente.");
                        break;
                }

                if (opcao != 0)
                {
                    PausarParaContinuar();
                }

            } while (opcao != 0);
        }

        static void ListarVeiculos(string tipo)
        {
            Console.Clear();
            List<Veiculo> veiculosFiltrados = estoque.Where(v => v.Tipo.Equals(tipo, StringComparison.OrdinalIgnoreCase)).ToList();

            DesenharBordaSuperior();
            DesenharLinhaTexto($"LISTA DE {tipo.ToUpper()}S", ConsoleColor.Yellow, true);
            DesenharLinhaSeparadora();

            if (veiculosFiltrados.Count == 0)
            {
                DesenharLinhaTexto($"Nenhum(a) {tipo} encontrado(a) no estoque.");
            }
            else
            {
                int i = 1;
                foreach (Veiculo v in veiculosFiltrados)
                {
                    DesenharLinhaTexto($"{i++} - {v.Nome} - R$ {v.Preco:N2} | Estoque: {v.Quantidade}");
                }
            }
            DesenharBordaInferior();
        }

        static void AdicionarCarrinho()
        {
            Console.Clear();
            DesenharBordaSuperior();
            DesenharLinhaTexto("ADICIONAR ITEM AO CARRINHO", ConsoleColor.Yellow, true);
            DesenharLinhaSeparadora();
            for (int i = 0; i < estoque.Count; i++)
            {
                string info = $"{i + 1} - {estoque[i].Tipo} | {estoque[i].Nome} - R$ {estoque[i].Preco:N2} | Estoque: {estoque[i].Quantidade}";
                DesenharLinhaTexto(info);
            }
            DesenharBordaInferior();
            Console.WriteLine();

            Console.Write("  Digite o número do veículo para adicionar: ");
            if (int.TryParse(Console.ReadLine(), out int escolha) && escolha > 0 && escolha <= estoque.Count)
            {
                Veiculo veiculoSelecionado = estoque[escolha - 1];

                Console.Write($"  Quantas unidades de {veiculoSelecionado.Nome} deseja adicionar? ");
                if (!int.TryParse(Console.ReadLine(), out int quantidadeDesejada) || quantidadeDesejada <= 0)
                {
                    ExibirMensagemErro("Quantidade inválida!");
                    return;
                }

                if (veiculoSelecionado.Quantidade >= quantidadeDesejada)
                {
                    carrinho.Add(new Veiculo(veiculoSelecionado.Tipo, veiculoSelecionado.Nome, veiculoSelecionado.Preco, quantidadeDesejada));
                    veiculoSelecionado.Quantidade -= quantidadeDesejada;
                    ExibirMensagemSucesso($"{quantidadeDesejada} unidade(s) de {veiculoSelecionado.Nome} foram adicionadas ao carrinho.");
                    SalvarCarrinho();
                    SalvarEstoque();
                }
                else
                {
                    ExibirMensagemErro("Estoque insuficiente!");
                }
            }
            else
            {
                ExibirMensagemErro("Opção inválida!");
            }
        }

        static void VerCarrinho()
        {
            Console.Clear();
            DesenharBordaSuperior();
            DesenharLinhaTexto("SEU CARRINHO DE COMPRAS", ConsoleColor.Yellow, true);
            DesenharLinhaSeparadora();

            if (carrinho.Count == 0)
            {
                DesenharLinhaTexto("Carrinho vazio!");
            }
            else
            {
                double total = 0;
                int i = 1;
                foreach (Veiculo v in carrinho)
                {
                    string itemInfo = $"{i++} - {v.Nome} ({v.Tipo}) | {v.Quantidade} x R$ {v.Preco:N2} = R$ {v.Preco * v.Quantidade:N2}";
                    DesenharLinhaTexto(itemInfo);
                    total += v.Preco * v.Quantidade;
                }
                DesenharLinhaSeparadora();
                DesenharLinhaTexto($"TOTAL: R$ {total:N2}", ConsoleColor.Green);
            }
            DesenharBordaInferior();
        }

        static void RemoverDoCarrinho()
        {
            Console.Clear();
            DesenharBordaSuperior();
            DesenharLinhaTexto("REMOVER ITEM DO CARRINHO", ConsoleColor.Yellow, true);
            DesenharLinhaSeparadora();

            if (carrinho.Count == 0)
            {
                DesenharLinhaTexto("Carrinho vazio!");
                DesenharBordaInferior();
                return;
            }

            for (int i = 0; i < carrinho.Count; i++)
            {
                string itemInfo = $"{i + 1} - {carrinho[i].Nome} | Quantidade: {carrinho[i].Quantidade}";
                DesenharLinhaTexto(itemInfo);
            }
            DesenharBordaInferior();
            Console.WriteLine();

            Console.Write("  Digite o número do item que deseja remover: ");
            if (int.TryParse(Console.ReadLine(), out int escolha) && escolha > 0 && escolha <= carrinho.Count)
            {
                Veiculo itemParaRemover = carrinho[escolha - 1];
                
                Console.Write($"  Quantas unidades de {itemParaRemover.Nome} deseja remover? (Total = {itemParaRemover.Quantidade}) ");
                if (!int.TryParse(Console.ReadLine(), out int qtdRemover) || qtdRemover <= 0)
                {
                    ExibirMensagemErro("Quantidade inválida!");
                    return;
                }

                Veiculo itemNoEstoque = estoque.Find(v => v.Nome == itemParaRemover.Nome && v.Tipo == itemParaRemover.Tipo);

                if (qtdRemover >= itemParaRemover.Quantidade)
                {
                    if (itemNoEstoque != null) itemNoEstoque.Quantidade += itemParaRemover.Quantidade;
                    carrinho.RemoveAt(escolha - 1);
                    ExibirMensagemSucesso($"{itemParaRemover.Nome} removido completamente do carrinho.");
                }
                else
                {
                    itemParaRemover.Quantidade -= qtdRemover;
                    if (itemNoEstoque != null) itemNoEstoque.Quantidade += qtdRemover;
                    ExibirMensagemSucesso($"{qtdRemover} unidade(s) de {itemParaRemover.Nome} foram removidas do carrinho.");
                }

                SalvarCarrinho();
                SalvarEstoque();
            }
            else
            {
                ExibirMensagemErro("Opção inválida!");
            }
        }

        static void FinalizarCompra()
        {
            Console.Clear();
            DesenharBordaSuperior();
            DesenharLinhaTexto("FINALIZAR COMPRA", ConsoleColor.Yellow, true);
            DesenharLinhaSeparadora();
            
            if (carrinho.Count == 0)
            {
                DesenharLinhaTexto("Carrinho vazio, não é possível finalizar!");
            }
            else
            {
                double total = 0;
                foreach (var v in carrinho)
                {
                    string itemInfo = $"{v.Nome} ({v.Tipo}) | {v.Quantidade} x R$ {v.Preco:N2} = R$ {v.Preco * v.Quantidade:N2}";
                    DesenharLinhaTexto(itemInfo);
                    total += v.Preco * v.Quantidade;
                }
                DesenharLinhaSeparadora();
                DesenharLinhaTexto($"TOTAL PAGO: R$ {total:N2}", ConsoleColor.Green);
                DesenharLinhaSeparadora();
                DesenharLinhaTexto("Compra finalizada com sucesso!", ConsoleColor.Green, true);

                SalvarVenda();
                carrinho.Clear();
                File.WriteAllText(pathCarrinho, "");
                SalvarEstoque(); // O estoque já foi atualizado ao adicionar no carrinho, mas salvamos de novo por garantia.
            }
            DesenharBordaInferior();
        }

        static void AdicionarProdutoEstoque()
        {
            Console.Clear();
            DesenharBordaSuperior();
            DesenharLinhaTexto("ADICIONAR PRODUTO AO ESTOQUE", ConsoleColor.Yellow, true);
            DesenharBordaInferior();
            Console.WriteLine();

            Console.Write("  Digite o tipo (Carro/Moto): ");
            string tipo = Console.ReadLine();
            if (!tipo.Equals("carro", StringComparison.OrdinalIgnoreCase) && !tipo.Equals("moto", StringComparison.OrdinalIgnoreCase))
            {
                ExibirMensagemErro("Tipo inválido! Só é permitido Carro ou Moto.");
                return;
            }

            Console.Write("  Digite o nome do veículo: ");
            string nome = Console.ReadLine();

            Console.Write("  Digite o preço: ");
            if (!double.TryParse(Console.ReadLine(), out double preco) || preco < 0)
            {
                ExibirMensagemErro("Preço inválido!");
                return;
            }

            Console.Write("  Digite a quantidade em estoque: ");
            if (!int.TryParse(Console.ReadLine(), out int quantidade) || quantidade < 0)
            {
                ExibirMensagemErro("Quantidade inválida!");
                return;
            }
            
            string tipoFormatado = char.ToUpper(tipo[0]) + tipo.Substring(1).ToLower();
            
            estoque.Add(new Veiculo(tipoFormatado, nome, preco, quantidade));
            SalvarEstoque();
            ExibirMensagemSucesso($"{nome} foi adicionado ao estoque com {quantidade} unidades.");
        }

        static void RemoverProdutoEstoque()
        {
            Console.Clear();
            DesenharBordaSuperior();
            DesenharLinhaTexto("REMOVER PRODUTO DO ESTOQUE", ConsoleColor.Yellow, true);
            DesenharLinhaSeparadora();

            if (estoque.Count == 0)
            {
                DesenharLinhaTexto("Estoque vazio!");
                DesenharBordaInferior();
                return;
            }

            // Listar itens do estoque
            for (int i = 0; i < estoque.Count; i++)
            {
                string itemInfo = $"{i + 1} - {estoque[i].Tipo} | {estoque[i].Nome} - R$ {estoque[i].Preco:N2} | Estoque: {estoque[i].Quantidade}";
                DesenharLinhaTexto(itemInfo);
            }
            DesenharBordaInferior();
            Console.WriteLine();

            Console.Write("  Digite o número do produto que deseja remover: ");
            if (int.TryParse(Console.ReadLine(), out int escolha) && escolha > 0 && escolha <= estoque.Count)
            {
                Veiculo item = estoque[escolha - 1];

                Console.Write($"  Quantas unidades de {item.Nome} deseja remover? (Total = {item.Quantidade}) ");
                if (!int.TryParse(Console.ReadLine(), out int qtdRemover) || qtdRemover <= 0)
                {
                    ExibirMensagemErro("Quantidade inválida!");
                    return;
                }

                if (qtdRemover >= item.Quantidade)
                {
                    estoque.RemoveAt(escolha - 1);
                    ExibirMensagemSucesso($"{item.Nome} removido completamente do estoque.");
                }
                else
                {
                    item.Quantidade -= qtdRemover;
                    ExibirMensagemSucesso($"{qtdRemover} unidade(s) de {item.Nome} foram removidas do estoque.");
                }

                SalvarEstoque();
            }
            else
            {
                ExibirMensagemErro("Opção inválida!");
            }
        }

        static void MostrarItensVendidos()
        {
            Console.Clear();
            DesenharBordaSuperior();
            DesenharLinhaTexto("HISTÓRICO DE VENDAS", ConsoleColor.Yellow, true);
            DesenharLinhaSeparadora();

            if (!File.Exists(pathVendas) || new FileInfo(pathVendas).Length == 0)
            {
                DesenharLinhaTexto("Nenhuma venda registrada ainda.");
            }
            else
            {
                string[] linhas = File.ReadAllLines(pathVendas);
                foreach (string linha in linhas)
                {
                    // Trunca a linha se for maior que a largura do menu para não quebrar a borda
                    string linhaVisivel = linha.Length > LARGURA_MENU - 6 ? linha.Substring(0, LARGURA_MENU - 6) + "..." : linha;
                    DesenharLinhaTexto(linhaVisivel);
                }
            }
            DesenharBordaInferior();
        }

        static void InicializarEstoque()
        {
            estoque.Clear();
            if (!File.Exists(pathEstoque))
            {
                File.Create(pathEstoque).Close();
                return;
            }
            string[] linhas = File.ReadAllLines(pathEstoque);
            foreach (string linha in linhas)
            {
                if (string.IsNullOrWhiteSpace(linha)) continue;
                string[] dados = linha.Split(';');
                if (dados.Length != 4) continue;

                string tipo = dados[0];
                string nome = dados[1];
                if (!double.TryParse(dados[2], out double preco)) continue;
                if (!int.TryParse(dados[3], out int quantidade)) continue;
                estoque.Add(new Veiculo(tipo, nome, preco, quantidade));
            }
        }

        static void SalvarEstoque()
        {
            using (StreamWriter sw = new StreamWriter(pathEstoque, false))
            {
                foreach (Veiculo v in estoque)
                {
                    sw.WriteLine($"{v.Tipo};{v.Nome};{v.Preco};{v.Quantidade}");
                }
            }
        }

        static void SalvarCarrinho()
        {
            using (StreamWriter sw = new StreamWriter(pathCarrinho))
            {
                foreach (Veiculo v in carrinho)
                {
                    sw.WriteLine($"{v.Tipo} | {v.Nome} - R$ {v.Preco:N2} x {v.Quantidade}");
                }
            }
        }

        static void SalvarVenda()
        {
            using (StreamWriter sw = new StreamWriter(pathVendas, true))
            {
                sw.WriteLine($"--- Venda realizada em {DateTime.Now} ---");
                foreach (Veiculo v in carrinho)
                {
                    sw.WriteLine($"{v.Tipo} | {v.Nome} | {v.Quantidade} x R$ {v.Preco:N2} = R$ {(v.Preco * v.Quantidade):N2}");
                }
                sw.WriteLine(new string('-', LARGURA_MENU));
            }
        }

        static void DesenharBordaSuperior()
        {
            Console.Write("╔");
            Console.Write(new string('═', LARGURA_MENU - 2));
            Console.WriteLine("╗");
        }

        static void DesenharBordaInferior()
        {
            Console.Write("╚");
            Console.Write(new string('═', LARGURA_MENU - 2));
            Console.WriteLine("╝");
        }

        static void DesenharLinhaSeparadora()
        {
            Console.Write("╟");
            Console.Write(new string('─', LARGURA_MENU - 2));
            Console.WriteLine("╢");
        }

        static void DesenharLinhaTexto(string texto, ConsoleColor cor = ConsoleColor.Gray, bool centralizado = false)
        {
            Console.Write("║");
            Console.ForegroundColor = cor;
            if (centralizado)
            {
                int espacosEsquerda = (LARGURA_MENU - 2 - texto.Length) / 2;
                Console.Write(new string(' ', espacosEsquerda));
                Console.Write(texto);
                Console.Write(new string(' ', LARGURA_MENU - 2 - texto.Length - espacosEsquerda));
            }
            else
            {
                string textoComMargem = "  " + texto;
                Console.Write(textoComMargem.PadRight(LARGURA_MENU - 2));
            }
            Console.ResetColor();
            Console.WriteLine("║");
        }
        
        static void PausarParaContinuar()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\n  Pressione qualquer tecla para continuar...");
            Console.ResetColor();
            Console.ReadKey();
        }

        static void ExibirMensagemSucesso(string mensagem)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n  ✓ {mensagem}");
            Console.ResetColor();
        }

        static void ExibirMensagemErro(string mensagem)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n  ✗ {mensagem}");
            Console.ResetColor();
        }

        static void ExibirMensagem(string mensagem, ConsoleColor cor)
        {
            Console.ForegroundColor = cor;
            Console.WriteLine($"\n  {mensagem}");
            Console.ResetColor();
        }
    }

    class Veiculo
    {
        public string Tipo { get; set; }
        public string Nome { get; set; }
        public double Preco { get; set; }
        public int Quantidade { get; set; }

        public Veiculo(string tipo, string nome, double preco, int quantidade)
        {
            Tipo = tipo;
            Nome = nome;
            Preco = preco;
            Quantidade = quantidade;
        }
    }
}