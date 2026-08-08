// Versão 1 - Jogo da velha simples onde o computador joga de forma totalmente aleatória e sem estratégia.
/*
char[] matriz = new char[9];
string player, computer;
Random random = new Random();

InitializeGame();
for (int i = 0; i < 5; i++){
    Console.Clear();
    PrintMatriz();
    Console.WriteLine("\nChoose a space to play: (1-9)");
    
    int play;
    bool ok = int.TryParse(Console.ReadLine(), out play);
    while (!ok || play < 1 || play > 9 || matriz[play - 1] == player[0] || matriz[play - 1] == computer[0]){
        Console.WriteLine("Index not available! Choose another space:");
        ok = int.TryParse(Console.ReadLine(), out play);
    }
    matriz[play - 1] = player[0];

    if (i != 4){
        play = random.Next(1,10);
        while (matriz[play - 1] == player[0] || matriz[play - 1] == computer[0]){
            play = random.Next(1,10);
        }
        matriz[play - 1] = computer[0];
    }

    if (Win()){
        PrintMatriz();
        Console.WriteLine("Win!!");
        break;
    }
    
}
if (!Win())
    Console.WriteLine("Draw!");



bool Win(){
    return (matriz[0] == matriz[1] && matriz[1] == matriz[2]) ||
        (matriz[3] == matriz[4] && matriz[4] == matriz[5]) ||
        (matriz[6] == matriz[7] && matriz[7] == matriz[8]) ||
        (matriz[0] == matriz[3] && matriz[3] == matriz[6]) || 
        (matriz[1] == matriz[4] && matriz[4] == matriz[7]) ||
        (matriz[2] == matriz[5] && matriz[5] == matriz[8]) ||
        (matriz[0] == matriz[4] && matriz[4] == matriz[8]) ||
        (matriz[2] == matriz[4] && matriz[4] == matriz[6]);
}

void PrintMatriz(){
    Console.WriteLine($"{matriz[0]} | {matriz[1]} | {matriz[2]}");
    Console.WriteLine("-----------");
    Console.WriteLine($"{matriz[3]} | {matriz[4]} | {matriz[5]}");
    Console.WriteLine("-----------");
    Console.WriteLine($"{matriz[6]} | {matriz[7]} | {matriz[8]}");
}

void InitializeGame(){
    Console.Clear();
    for (int i =0; i < 9; i++){
        int x = i+1;
        matriz[i] = (char)(x + '0');
    }

    Console.WriteLine("Which player do you want to be: (X ou O)");
    player =  Console.ReadLine();
    while (player != "X" && player != "O"){
        Console.WriteLine("Which player do you want to be: (X ou O)");
        player =  Console.ReadLine();   
    }
    computer = player.Equals("X") ? "O" : "X";
}
*/
// Versão 2 - Jogo da Velha simples com placar, sorteio de turnos e um computador que bloqueia ou vence de forma direta.
/*
using System;
using System.Threading;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        GerenciadorJogo jogo = new GerenciadorJogo();
        jogo.IniciarEventos();
    }
}

class Tabuleiro
{
    private char[] matriz = new char[9];

    public void Inicializar()
    {
        for (int i = 0; i < 9; i++)
        {
            matriz[i] = (char)((i + 1) + '0');
        }
    }

    public char ObterPosicao(int index) => matriz[index];

    public void MarcarPosicao(int index, char simbolo)
    {
        matriz[index] = simbolo;
    }

    public bool PosicaoLivre(int index)
    {
        return matriz[index] != 'X' && matriz[index] != 'O';
    }

    public bool TemEspacoLivre()
    {
        for (int i = 0; i < 9; i++)
        {
            if (PosicaoLivre(i)) return true;
        }
        return false;
    }

    public void Desenhar()
    {
        Console.WriteLine("\n");
        RenderizarPeca(matriz[0]); Console.Write(" | "); RenderizarPeca(matriz[1]); Console.Write(" | "); RenderizarPeca(matriz[2]); Console.WriteLine();
        Console.WriteLine("-----------");
        RenderizarPeca(matriz[3]); Console.Write(" | "); RenderizarPeca(matriz[4]); Console.Write(" | "); RenderizarPeca(matriz[5]); Console.WriteLine();
        Console.WriteLine("-----------");
        RenderizarPeca(matriz[6]); Console.Write(" | "); RenderizarPeca(matriz[7]); Console.Write(" | "); RenderizarPeca(matriz[8]); Console.WriteLine("\n");
    }

    private void RenderizarPeca(char c)
    {
        if (c == 'X') Console.ForegroundColor = ConsoleColor.Red;
        else if (c == 'O') Console.ForegroundColor = ConsoleColor.Blue;
        else Console.ForegroundColor = ConsoleColor.DarkGray;

        Console.Write(c);
        Console.ResetColor();
    }

    public bool VerificarVitoria()
    {
        return (matriz[0] == matriz[1] && matriz[1] == matriz[2]) ||
               (matriz[3] == matriz[4] && matriz[4] == matriz[5]) ||
               (matriz[6] == matriz[7] && matriz[7] == matriz[8]) ||
               (matriz[0] == matriz[3] && matriz[3] == matriz[6]) || 
               (matriz[1] == matriz[4] && matriz[4] == matriz[7]) ||
               (matriz[2] == matriz[5] && matriz[5] == matriz[8]) ||
               (matriz[0] == matriz[4] && matriz[4] == matriz[8]) ||
               (matriz[2] == matriz[4] && matriz[4] == matriz[6]);
    }
}

abstract class Jogador
{
    public string Nome { get; protected set; }
    public char Simbolo { get; protected set; }

    public Jogador(string nome, char simbolo)
    {
        Nome = nome;
        Simbolo = simbolo;
    }

    public abstract int EscolherJogada(Tabuleiro tabuleiro, char simboloOponente);
}

class JogadorHumano : Jogador
{
    public JogadorHumano(char simbolo) : base("Você", simbolo) { }

    public override int EscolherJogada(Tabuleiro tabuleiro, char simboloOponente)
    {
        int play;
        bool ok = int.TryParse(Console.ReadLine(), out play);
        while (!ok || play < 1 || play > 9 || !tabuleiro.PosicaoLivre(play - 1))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Espaço inválido ou já ocupado! Tente novamente: ");
            Console.ResetColor();
            ok = int.TryParse(Console.ReadLine(), out play);
        }
        return play - 1; 
    }
}

class JogadorComputador : Jogador
{
    private Random random = new Random();

    public JogadorComputador(char simbolo) : base("Computador", simbolo) { }

    public override int EscolherJogada(Tabuleiro tabuleiro, char simboloOponente)
    {
        Console.WriteLine("\nComputador pensando...");
        Thread.Sleep(800);

        int jogada = BuscarMelhorPosicao(tabuleiro, Simbolo);
        if (jogada != -1) return jogada;

        jogada = BuscarMelhorPosicao(tabuleiro, simboloOponente);
        if (jogada != -1) return jogada;

        int play;
        do
        {
            play = random.Next(0, 9);
        } while (!tabuleiro.PosicaoLivre(play));

        return play;
    }

    private int BuscarMelhorPosicao(Tabuleiro tabuleiro, char simboloAlvo)
    {
        int[,] linhasVitoria = new int[,]
        {
            {0, 1, 2}, {3, 4, 5}, {6, 7, 8},
            {0, 3, 6}, {1, 4, 7}, {2, 5, 8},
            {0, 4, 8}, {2, 4, 6}
        };

        for (int i = 0; i < 8; i++)
        {
            int a = linhasVitoria[i, 0];
            int b = linhasVitoria[i, 1];
            int c = linhasVitoria[i, 2];

            char cA = tabuleiro.ObterPosicao(a);
            char cB = tabuleiro.ObterPosicao(b);
            char cC = tabuleiro.ObterPosicao(c);

            if (cA == simboloAlvo && cB == simboloAlvo && tabuleiro.PosicaoLivre(c)) return c;
            if (cA == simboloAlvo && cC == simboloAlvo && tabuleiro.PosicaoLivre(b)) return b;
            if (cB == simboloAlvo && cC == simboloAlvo && tabuleiro.PosicaoLivre(a)) return a;
        }
        return -1;
    }
}

class GerenciadorJogo
{
    private Tabuleiro tabuleiro = new Tabuleiro();
    private Jogador jogador1;
    private Jogador jogador2;
    private Random random = new Random();

    private int vitoriasHumano = 0;
    private int vitoriasComputador = 0;
    private int empates = 0;

    public void IniciarEventos()
    {
        bool jogarNovamente = true;
        while (jogarNovamente)
        {
            ConfigurarPartida();
            LoopDaPartida();

            Console.WriteLine("\n[1] Jogar Novamente");
            Console.WriteLine("[0] Sair");
            Console.Write("Escolha uma opção: ");
            string opcao = Console.ReadLine();
            jogarNovamente = (opcao == "1");
        }

        Console.Clear();
        Console.WriteLine("Obrigado por jogar! Até a próxima.");
    }

    private void ConfigurarPartida()
    {
        tabuleiro.Inicializar();
        Console.Clear();
        PrintBanner();

        Console.WriteLine();
        Console.Write("Qual jogador você quer ser? (X ou O): ");
        string escolha = Console.ReadLine().ToUpper();
        while (escolha != "X" && escolha != "O")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Escolha inválida! Digite apenas X ou O: ");
            Console.ResetColor();
            escolha = Console.ReadLine().ToUpper();
        }

        char hSimbolo = escolha[0];
        char cSimbolo = hSimbolo == 'X' ? 'O' : 'X';

        Jogador humano = new JogadorHumano(hSimbolo);
        Jogador computador = new JogadorComputador(cSimbolo);

        if (random.Next(0, 2) == 0)
        {
            jogador1 = humano;
            jogador2 = computador;
        }
        else
        {
            jogador1 = computador;
            jogador2 = humano;
        }

        ExibirTela();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"Sorteio realizado! {jogador1.Nome} ({jogador1.Simbolo}) joga primeiro.");
        Console.ResetColor();
        Console.WriteLine("\nPressione qualquer tecla para começar...");
        Console.ReadKey();
    }

    private void LoopDaPartida()
    {
        Jogador jogadorAtual = jogador1;
        bool jogoAtivo = true;

        while (jogoAtivo)
        {
            ExibirTela();
            Console.WriteLine($"Vez de: {jogadorAtual.Nome} ({jogadorAtual.Simbolo})");

            char simboloOponente = jogadorAtual == jogador1 ? jogador2.Simbolo : jogador1.Simbolo;
            int jogada = jogadorAtual.EscolherJogada(tabuleiro, simboloOponente);
            
            tabuleiro.MarcarPosicao(jogada, jogadorAtual.Simbolo);

            if (tabuleiro.VerificarVitoria())
            {
                ExibirTela();
                FinalizarPartida(jogadorAtual);
                jogoAtivo = false;
            }
            else if (!tabuleiro.TemEspacoLivre())
            {
                ExibirTela();
                FinalizarPartida(null); 
                jogoAtivo = false;
            }
            else
            {
                jogadorAtual = jogadorAtual == jogador1 ? jogador2 : jogador1;
            }
        }
    }

    private void FinalizarPartida(Jogador vencedor)
    {
        if (vencedor == null)
        {
            empates++;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nDeu Velha! O jogo empatou.");
        }
        else if (vencedor is JogadorHumano)
        {
            vitoriasHumano++;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nParabéns! Você venceu o computador! 🎉");
        }
        else
        {
            vitoriasComputador++;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nO computador venceu! Mais sorte na próxima vez.");
        }
        Console.ResetColor();
    }

    private void ExibirTela()
    {
        Console.Clear();
        PrintBanner();
        
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"PLACAR: Você [{vitoriasHumano}] | Computador [{vitoriasComputador}] | Empates [{empates}]");
        Console.WriteLine("===========================================================");
        Console.ResetColor();

        tabuleiro.Desenhar();
    }

    private void PrintBanner()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"     _                             _          __     __    _ _           ");
        Console.WriteLine(@"    | | ___   __ _  ___         __| | __ _    \ \   / /__ | | |__   __ _ ");
        Console.WriteLine(@" _  | |/ _ \ / _` |/ _ \       / _` |/ _` |    \ \ / / _ \| | '_ \ / _` |");
        Console.WriteLine(@"| |_| | (_) | (_| | (_) |     | (_| | (_| |     \ V /  __/| | | | | (_| |");
        Console.WriteLine(@" \___/ \___/ \__, |\___/       \__,_|\__,_|      \_/ \___||_|_| |_|\__,_|");
        Console.WriteLine(@"             |___/                                                         ");
        Console.WriteLine("=========================================================================");
        Console.ResetColor();
    }
}
*/       
// Versão 3 - 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.CursorVisible = true;
        new GerenciadorJogo().IniciarEventos();
    }
}

class Tabuleiro
{
    private char[] matriz = new char[9];

    public void Inicializar()
    {
        for (int i = 0; i < 9; i++)
            matriz[i] = (char)('1' + i);
    }

    public char ObterPosicao(int index) => matriz[index];
    public void MarcarPosicao(int index, char simbolo) => matriz[index] = simbolo;
    public void DesfazerJogada(int index, char original) => matriz[index] = original;

    public bool PosicaoLivre(int index) =>
        matriz[index] != 'X' && matriz[index] != 'O';

    public bool TemEspacoLivre() =>
        Enumerable.Range(0, 9).Any(i => PosicaoLivre(i));

    public List<int> PosicaoLivres() =>
        Enumerable.Range(0, 9).Where(i => PosicaoLivre(i)).ToList();

    private static readonly int[][] Linhas = new int[][]
    {
        new[]{0,1,2}, new[]{3,4,5}, new[]{6,7,8},
        new[]{0,3,6}, new[]{1,4,7}, new[]{2,5,8},
        new[]{0,4,8}, new[]{2,4,6}
    };

    public int[]? LinhaVencedora()
    {
        foreach (var l in Linhas)
            if (matriz[l[0]] == matriz[l[1]] && matriz[l[1]] == matriz[l[2]])
                return l;
        return null;
    }

    public bool VerificarVitoria() => LinhaVencedora() != null;

    public char? VencedorChar()
    {
        var l = LinhaVencedora();
        return l != null ? matriz[l[0]] : (char?)null;
    }

    public void Desenhar(int[]? destaque = null)
    {
        Console.WriteLine();
        for (int row = 0; row < 3; row++)
        {
            Console.Write("      ");
            for (int col = 0; col < 3; col++)
            {
                int idx = row * 3 + col;
                bool hl = destaque != null && destaque.Contains(idx);
                DrawCell(matriz[idx], hl);
                if (col < 2)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("  │  ");
                    Console.ResetColor();
                }
            }
            Console.WriteLine();
            if (row < 2)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("     ────┼─────┼────");
                Console.ResetColor();
            }
        }
        Console.WriteLine();
    }

    private static void DrawCell(char c, bool highlight)
    {
        if (highlight) Console.BackgroundColor = ConsoleColor.DarkGreen;
        Console.ForegroundColor = c == 'X' ? ConsoleColor.Red
                                : c == 'O' ? ConsoleColor.Cyan
                                :            ConsoleColor.DarkGray;
        Console.Write($" {c} ");
        Console.ResetColor();
    }
}

enum Dificuldade { Facil, Medio, Dificil }

abstract class Jogador
{
    public string Nome     { get; }
    public char   Simbolo  { get; set; }   // definido pelo GerenciadorJogo
    protected Jogador(string nome) => Nome = nome;
    public abstract int EscolherJogada(Tabuleiro tab, char simOponente);
}

class JogadorHumano : Jogador
{
    public JogadorHumano(string nome) : base(nome) { }

    public override int EscolherJogada(Tabuleiro tab, char simOponente)
    {
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int p)
                && p >= 1 && p <= 9 && tab.PosicaoLivre(p - 1))
                return p - 1;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("  ✖  Posição inválida ou ocupada — tente novamente: ");
            Console.ResetColor();
        }
    }
}

class JogadorComputador : Jogador
{
    public readonly Dificuldade Dificuldade;
    private readonly Random rng = new();

    public JogadorComputador(Dificuldade dif)
        : base($"CPU ({Label(dif)})")
    {
        Dificuldade = dif;
    }

    static string Label(Dificuldade d) => d switch
    {
        Dificuldade.Facil   => "Fácil",
        Dificuldade.Medio   => "Médio",
        Dificuldade.Dificil => "Difícil",
        _ => ""
    };

    public override int EscolherJogada(Tabuleiro tab, char simOponente)
    {
        AnimarPensamento();
        return Dificuldade switch
        {
            Dificuldade.Facil   => Aleatorio(tab),
            Dificuldade.Medio   => Medio(tab, simOponente),
            Dificuldade.Dificil => Minimax(tab, simOponente),
            _                   => Aleatorio(tab)
        };
    }

    private int Aleatorio(Tabuleiro t)
    {
        var l = t.PosicaoLivres();
        return l[rng.Next(l.Count)];
    }

    private int Medio(Tabuleiro t, char op)
    {
        int j = BuscarVitoria(t, Simbolo); if (j != -1) return j;
            j = BuscarVitoria(t, op);      if (j != -1) return j;
        if (t.PosicaoLivre(4)) return 4;
        return Aleatorio(t);
    }

    private int BuscarVitoria(Tabuleiro t, char s)
    {
        foreach (int i in t.PosicaoLivres())
        {
            char orig = t.ObterPosicao(i);
            t.MarcarPosicao(i, s);
            bool v = t.VerificarVitoria();
            t.DesfazerJogada(i, orig);
            if (v) return i;
        }
        return -1;
    }

    private int Minimax(Tabuleiro t, char simOp)
    {
        int melhorScore = int.MinValue;
        int melhorJogada = t.PosicaoLivres()[0]; 

        foreach (int i in t.PosicaoLivres())
        {
            char orig = t.ObterPosicao(i);
            t.MarcarPosicao(i, Simbolo);
            int score = MinimaxRec(t, 0, false, int.MinValue, int.MaxValue, Simbolo, simOp);
            t.DesfazerJogada(i, orig);

            if (score > melhorScore)
            {
                melhorScore  = score;
                melhorJogada = i;
            }
        }
        return melhorJogada;
    }

    private int MinimaxRec(Tabuleiro t, int prof, bool max,
                           int alpha, int beta, char eu, char op)
    {
        char? v = t.VencedorChar();
        if (v == eu) return 10 - prof;
        if (v == op) return prof - 10;
        if (!t.TemEspacoLivre()) return 0;

        if (max)
        {
            int best = int.MinValue;
            foreach (int i in t.PosicaoLivres())
            {
                char orig = t.ObterPosicao(i);
                t.MarcarPosicao(i, eu);
                best = Math.Max(best, MinimaxRec(t, prof + 1, false, alpha, beta, eu, op));
                t.DesfazerJogada(i, orig);
                alpha = Math.Max(alpha, best);
                if (beta <= alpha) break;
            }
            return best;
        }
        else
        {
            int best = int.MaxValue;
            foreach (int i in t.PosicaoLivres())
            {
                char orig = t.ObterPosicao(i);
                t.MarcarPosicao(i, op);
                best = Math.Min(best, MinimaxRec(t, prof + 1, true, alpha, beta, eu, op));
                t.DesfazerJogada(i, orig);
                beta = Math.Min(beta, best);
                if (beta <= alpha) break;
            }
            return best;
        }
    }

    private void AnimarPensamento()
    {
        int delay = Dificuldade == Dificuldade.Dificil ? 900 : 600;
        Console.ForegroundColor = ConsoleColor.DarkGray;
        foreach (var s in new[] { "pensando.  ", "pensando.. ", "pensando..." })
        {
            Console.Write($"\r  {s}");
            Thread.Sleep(delay / 3);
        }
        Console.Write("\r                  \r");
        Console.ResetColor();
    }
}

class Estatisticas
{
    private Dictionary<string, int> vitorias = new();
    private int empates;

    public void Registrar(string? nome)
    {
        if (nome == null) { empates++; return; }
        vitorias[nome] = vitorias.GetValueOrDefault(nome, 0) + 1;
    }

    public void Exibir(Jogador j1, Jogador j2)
    {
        int v1 = vitorias.GetValueOrDefault(j1.Nome, 0);
        int v2 = vitorias.GetValueOrDefault(j2.Nome, 0);
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write($"  {Truncar(j1.Nome, 10)}: ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"{v1}   ");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("│  ");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write($"{Truncar(j2.Nome, 10)}: ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"{v2}   ");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("│  Empates: ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(empates);
        Console.ResetColor();
    }

    private static string Truncar(string s, int max) =>
        s.Length <= max ? s : s[..max] + "…";
}

class GerenciadorJogo
{
    private readonly Tabuleiro    tab   = new();
    private readonly Estatisticas stats = new();
    private readonly Random       rng   = new();

    private Jogador dono1 = null!;
    private Jogador dono2 = null!;

    private Jogador primeiro = null!;
    private Jogador segundo  = null!;

    public void IniciarEventos()
    {
        EscolherModoEDificuldade(); 

        bool continuar = true;
        while (continuar)
        {
            ConfigurarPartida();     
            LoopDaPartida();

            Console.WriteLine();
            Separator();
            stats.Exibir(dono1, dono2);
            Separator();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("  [1] Jogar novamente   [2] Novo modo   [0] Sair → ");
            Console.ResetColor();

            string? op = Console.ReadLine()?.Trim();
            if      (op == "0") continuar = false;
            else if (op == "2") EscolherModoEDificuldade();
        }

        Console.Clear();
        PrintBanner();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("  Obrigado por jogar! Até a próxima.\n");
        Console.ResetColor();
    }

    private void EscolherModoEDificuldade()
    {
        Console.Clear();
        PrintBanner();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("  Modo de jogo:\n");
        Console.ResetColor();
        Console.WriteLine("  [1]  Jogador vs CPU");
        Console.WriteLine("  [2]  Jogador vs Jogador");
        Console.Write("\n  Escolha: ");
        string? modo = Console.ReadLine()?.Trim();

        if (modo == "2")
            ConfigurarDoisHumanos();
        else
            ConfigurarHumanoCPU();
    }

    private void ConfigurarHumanoCPU()
    {
        Console.Clear();
        PrintBanner();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n  Dificuldade da CPU:\n");
        Console.ResetColor();
        Console.WriteLine("  [1]  Fácil    — joga aleatoriamente");
        Console.WriteLine("  [2]  Médio    — tenta vencer e bloquear");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("  [3]  Difícil  — Minimax (imbatível)");
        Console.ResetColor();
        Console.Write("\n  Escolha: ");

        Dificuldade dif = Console.ReadLine()?.Trim() switch
        {
            "1" => Dificuldade.Facil,
            "3" => Dificuldade.Dificil,
            _   => Dificuldade.Medio
        };

        if (dif == Dificuldade.Dificil)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nNo modo Difícil o Minimax joga perfeitamente.");
            Console.WriteLine("O melhor resultado possível para você é empate.");
            Console.ResetColor();
            Thread.Sleep(1800);
        }

        Console.Write("\nSeu nome (Enter = Jogador): ");
        string? nome = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(nome)) nome = "Jogador";

        dono1 = new JogadorHumano(nome);
        dono2 = new JogadorComputador(dif);
    }

    private void ConfigurarDoisHumanos()
    {
        Console.Clear();
        PrintBanner();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\nModo 2 Jogadores\n");
        Console.ResetColor();

        Console.Write("Nome do Jogador 1: ");
        string? n1 = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(n1)) n1 = "Jogador 1";

        Console.Write("Nome do Jogador 2: ");
        string? n2 = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(n2)) n2 = "Jogador 2";

        dono1 = new JogadorHumano(n1);
        dono2 = new JogadorHumano(n2);
    }

    private void ConfigurarPartida()
    {
        tab.Inicializar();

        bool dono1Comeca = (DateTime.Now.Ticks % 2) == 0;

        primeiro = dono1Comeca ? dono1 : dono2;
        segundo  = dono1Comeca ? dono2 : dono1;

        primeiro.Simbolo = 'X';
        segundo.Simbolo  = 'O';

        Console.Clear();
        PrintBanner();
        Separator();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Sorteio realizado ! ");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("X (começa primeiro): ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(primeiro.Nome);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("O (joga segundo):     ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(segundo.Nome);
        Console.ResetColor();
        Separator();
        Console.Write("Pressione qualquer tecla para começar...");
        Console.ReadKey(true);
    }

    private void LoopDaPartida()
    {
        Jogador atual = primeiro;

        while (true)
        {
            ExibirTela();

            var cor = atual.Simbolo == 'X' ? ConsoleColor.Red : ConsoleColor.Cyan;
            Console.ForegroundColor = cor;
            Console.Write($"  {atual.Nome} ({atual.Simbolo})");
            Console.ResetColor();

            char simOp = atual == primeiro ? segundo.Simbolo : primeiro.Simbolo;
            int jogada;

            if (atual is JogadorComputador)
            {
                Console.WriteLine(" está jogando...");
                jogada = atual.EscolherJogada(tab, simOp);

                tab.MarcarPosicao(jogada, atual.Simbolo);
                ExibirTela();
                Console.ForegroundColor = cor;
                Console.Write($"  {atual.Nome} ({atual.Simbolo})");
                Console.ResetColor();
                Console.WriteLine($" jogou na posição {jogada + 1}.");
                Thread.Sleep(900);
            }
            else
            {
                Console.Write(" — posição (1-9): ");
                jogada = atual.EscolherJogada(tab, simOp);
                tab.MarcarPosicao(jogada, atual.Simbolo);
            }

            if (tab.VerificarVitoria())
            {
                ExibirTela(tab.LinhaVencedora());
                FinalizarPartida(atual);
                return;
            }

            if (!tab.TemEspacoLivre())
            {
                ExibirTela();
                FinalizarPartida(null);
                return;
            }

            atual = atual == primeiro ? segundo : primeiro;
        }
    }

    private void FinalizarPartida(Jogador? vencedor)
    {
        stats.Registrar(vencedor?.Nome);

        Console.WriteLine();
        Separator();

        if (vencedor == null)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Deu velha! Empate.");
        }
        else if (vencedor is JogadorHumano)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{vencedor.Nome} venceu!");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{vencedor.Nome} venceu! Mais sorte na próxima.");
        }

        Console.ResetColor();
        Separator();

        if (vencedor is JogadorComputador || (vencedor == null && dono2 is JogadorComputador cpu2 && cpu2.Dificuldade == Dificuldade.Dificil))
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Minimax avalia todas as jogadas futuras — empate é o seu teto.");
            Console.ResetColor();
        }
    }

    private void ExibirTela(int[]? destaque = null)
    {
        Console.Clear();
        PrintBanner();
        Separator();
        stats.Exibir(dono1, dono2);
        Separator();

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("  Referência:  1 │ 2 │ 3");
        Console.WriteLine("              ───┼───┼───");
        Console.WriteLine("               4 │ 5 │ 6");
        Console.WriteLine("              ───┼───┼───");
        Console.WriteLine("               7 │ 8 │ 9");
        Console.ResetColor();

        tab.Desenhar(destaque);
    }

    private void PrintBanner()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"     _                             _          __     __    _ _           ");
        Console.WriteLine(@"    | | ___   __ _  ___         __| | __ _    \ \   / /__ | | |__   __ _ ");
        Console.WriteLine(@" _  | |/ _ \ / _` |/ _ \       / _` |/ _` |    \ \ / / _ \| | '_ \ / _` |");
        Console.WriteLine(@"| |_| | (_) | (_| | (_) |     | (_| | (_| |     \ V /  __/| | | | | (_| |");
        Console.WriteLine(@" \___/ \___/ \__, |\___/       \__,_|\__,_|      \_/ \___||_|_| |_|\__,_|");
        Console.WriteLine(@"             |___/                                                         ");
        Console.WriteLine("=========================================================================");
        Console.ResetColor();
    }

    private static void Separator()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("  " + new string('─', 50));
        Console.ResetColor();
    }
}