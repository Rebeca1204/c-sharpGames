using System;
using System.Threading;

namespace JokenpoGame;

enum Escolha { Pedra = 0, Papel = 1, Tesoura = 2, Lagarto = 3, Spock = 4, Invalido = 5 }

class Program
{
    private static readonly Random _random = new();
    private static int _vitorias, _derrotas, _empates;

    private static readonly string[] _nomesExibicao = { "Pedra", "Papel", "Tesoura", "Lagarto", "Spock" };

    static void Main()
    {
        bool rodarPrograma = true;

        while (rodarPrograma)
        {
            ExibirMenuPrincipal();
            string modoOpcao = Console.ReadLine().Trim();

            if (modoOpcao == "N" || modoOpcao == "n") break;
            if (modoOpcao != "1" && modoOpcao != "2")
            {
                MostrarMensagemErro("Opção inválida! Escolha 1, 2 ou 3.");
                continue;
            }

            int limiteOpcoes = (modoOpcao == "1") ? 3 : 5;
            ExecutarModoJogo(limiteOpcoes);
        }

        ExibirTelaEncerramento();
    }

    private static void ExecutarModoJogo(int limiteOpcoes)
    {
        bool jogarNovamente = true;

        do
        {
            ConfigurarTelaPartida(limiteOpcoes);
            
            Console.Write("\n Sua escolha: ");
            string entrada = Console.ReadLine().Trim().ToLower();

            Escolha jogador = ConverterEntradaParaEscolha(entrada, limiteOpcoes);
            if (jogador == Escolha.Invalido)
            {
                MostrarMensagemErro("Opção inválida para o modo selecionado!");
                continue;
            }

            ExecutarAnimacaoSuspense(limiteOpcoes);

            Escolha computador = (Escolha)_random.Next(0, limiteOpcoes);

            Console.WriteLine($"Você escolheu:      {_nomesExibicao[(int)jogador].ToUpper()}");
            Console.WriteLine($"Computador escolheu: {_nomesExibicao[(int)computador].ToUpper()}\n");

            ProcessarResultado(jogador, computador);

            Console.WriteLine("\n-------------------------------------------------");
            Console.Write(" Deseja continuar neste modo? (Digite 'n' para voltar ao menu ou Enter para avançar): ");
            string resposta = Console.ReadLine().Trim().ToLower();
            if (!string.IsNullOrEmpty(resposta) && resposta[0] == 'n') jogarNovamente = false;

        } while (jogarNovamente);
    }

    private static Escolha ConverterEntradaParaEscolha(string entrada, int limiteOpcoes)
    {
        if (string.IsNullOrEmpty(entrada)) return Escolha.Invalido;

        return entrada[0] switch
        {
            'p' when entrada.StartsWith("pe") => Escolha.Pedra,
            'p' when entrada.StartsWith("pa") => Escolha.Papel,
            't' => Escolha.Tesoura,
            'l' when limiteOpcoes == 5 => Escolha.Lagarto,
            's' when limiteOpcoes == 5 => Escolha.Spock,
            _ => Escolha.Invalido
        };
    }

    private static void ProcessarResultado(Escolha j, Escolha c)
    {
        if (j == c)
        {
            ConfigurarFundoResultado(ConsoleColor.Yellow, ConsoleColor.Black);
            Console.WriteLine("RESULTADO: EMPATE!                           ");
            _empates++;
        }
        else if ((j == Escolha.Pedra && (c == Escolha.Lagarto || c == Escolha.Tesoura)) ||
                 (j == Escolha.Papel && (c == Escolha.Pedra || c == Escolha.Spock)) ||
                 (j == Escolha.Tesoura && (c == Escolha.Papel || c == Escolha.Lagarto)) ||
                 (j == Escolha.Lagarto && (c == Escolha.Papel || c == Escolha.Spock)) ||
                 (j == Escolha.Spock && (c == Escolha.Pedra || c == Escolha.Tesoura)))
        {
            _vitorias++;
            ConfigurarFundoResultado(ConsoleColor.Green, ConsoleColor.White);
            Console.WriteLine($"RESULTADO: VOCÊ GANHOU! {_nomesExibicao[(int)j]} {ObterVerbo(j, c)} {_nomesExibicao[(int)c]} ");
        }
        else
        {
            _derrotas++;
            ConfigurarFundoResultado(ConsoleColor.Red, ConsoleColor.White);
            Console.WriteLine($"RESULTADO: VOCÊ PERDEU! {_nomesExibicao[(int)c]} {ObterVerbo(c, j)} {_nomesExibicao[(int)j]} ");
        }
        Console.ResetColor();
    }

    private static string ObterVerbo(Escolha vencedor, Escolha perdedor)
    {
        return (vencedor, perdedor) switch
        {
            (Escolha.Pedra, Escolha.Lagarto) => "esmaga",
            (Escolha.Pedra, Escolha.Tesoura) => "quebra",
            (Escolha.Papel, Escolha.Pedra) => "cobre",
            (Escolha.Papel, Escolha.Spock) => "refuta",
            (Escolha.Tesoura, Escolha.Papel) => "corta",
            (Escolha.Tesoura, Escolha.Lagarto) => "decapita",
            (Escolha.Lagarto, Escolha.Papel) => "come",
            (Escolha.Lagarto, Escolha.Spock) => "envenena",
            (Escolha.Spock, Escolha.Pedra) => "vaporiza",
            (Escolha.Spock, Escolha.Tesoura) => "derrete",
            _ => "vence"
        };
    }

    private static void ExibirMenuPrincipal()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=================================================");
        Console.WriteLine("      BEM-VINDO AO PEDRA, PAPEL E TESOURA       ");
        Console.WriteLine("=================================================");
        Console.ResetColor();
        Console.WriteLine("\n Escolha o modo de jogo:");
        Console.WriteLine(" [1] Clássico (Pedra, Papel e Tesoura)");
        Console.WriteLine(" [2] Expandido (Pedra, Papel, Tesoura, Lagarto e Spock)");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(" [N] Sair do Programa");
        Console.ResetColor();
        Console.Write("\n Digite sua opção: ");
    }

    private static void ConfigurarTelaPartida(int limiteOpcoes)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=================================================");
        Console.WriteLine(limiteOpcoes == 3 ? "               MODO: CLÁSSICO                 " : "              MODO: EXPANDIDO                 ");
        Console.WriteLine("=================================================");
        Console.ResetColor();

        Console.Write(" Placar Geral: ");
        Console.ForegroundColor = ConsoleColor.Green;  Console.Write($"【{_vitorias} Vitórias】 ");
        Console.ForegroundColor = ConsoleColor.Red;    Console.Write($"【{_derrotas} Derrotas】 ");
        Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine($"【{_empates} Empates】\n");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.DarkGray; Console.WriteLine("-------------------------------------------------"); Console.ResetColor();
        
        Console.WriteLine(" Escolha uma opção:");
        Console.WriteLine(limiteOpcoes == 3 ? " [pe]dra | [pa]pel | [t]esoura" : " [pe]dra | [pa]pel | [t]esoura | [l]agarto | [s]pock");
    }

    private static void ExecutarAnimacaoSuspense(int limiteOpcoes)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Magenta;
        string[] etapas = limiteOpcoes == 3 ? ["Pedra...", "Papel...", "Tesoura! Go!!!"] : ["Pedra...", "Papel...", "Tesoura...", "Lagarto...", "Spock!!!"];
        foreach (var etapa in etapas)
        {
            Console.Write($"{etapa} ");
            Thread.Sleep(300);
        }
        Console.ResetColor();
        Console.WriteLine("\n");
    }

    private static void MostrarMensagemErro(string mensagem)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"\n {mensagem}");
        Console.ResetColor();
        Thread.Sleep(1500);
    }

    private static void ConfigurarFundoResultado(ConsoleColor fundo, ConsoleColor texto)
    {
        Console.BackgroundColor = fundo;
        Console.ForegroundColor = texto;
    }

    private static void ExibirTelaEncerramento()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=================================================");
        Console.WriteLine("             OBRIGADO POR JOGAR!               ");
        Console.WriteLine("=================================================");
        Console.ResetColor();
        Console.WriteLine($" Placar Final Consolidado: {_vitorias} Vitórias, {_derrotas} Derrotas e {_empates} Empates.");
        Console.ReadKey();
    }
}