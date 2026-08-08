// Versão 1 - jogo de adivinhação simples com dificuldades e pontuação decrescente
/*using System;

Random random = new Random();
bool jogarNovamente = true;

while (jogarNovamente)
{
    Console.Clear();
    Console.WriteLine("BEM-VINDO AO JOGO DA ADIVINHAÇÃO MAX");
    Console.WriteLine("Escolha sua dificuldade:");
    Console.WriteLine("1 - Fácil (1 a 50 - 10 tentativas)");
    Console.WriteLine("2 - Médio (1 a 100 - 7 tentativas)");
    Console.WriteLine("3 - Difícil (1 a 200 - 5 tentativas)");
    
    int limiteMax = 100;
    int tentativasRestantes = 7;
    int escolha;
    
    while (!int.TryParse(Console.ReadLine(), out escolha) || escolha < 1 || escolha > 3)
    {
        Console.WriteLine("Opção inválida. Escolha 1, 2 ou 3:");
    }

    switch (escolha)
    {
        case 1: limiteMax = 50; tentativasRestantes = 10; break;
        case 2: limiteMax = 100; tentativasRestantes = 7; break;
        case 3: limiteMax = 200; tentativasRestantes = 5; break;
    }

    int valorSecreto = random.Next(1, limiteMax + 1);
    int pontuacao = 1000;
    bool acertou = false;

    Console.Clear();
    Console.WriteLine($"Mentalizei um número de 1 a {limiteMax}. Boa sorte!");

    while (tentativasRestantes > 0 && !acertou)
    {
        Console.WriteLine($"\nTentativas restantes: {tentativasRestantes} | Pontuação atual: {pontuacao}");
        Console.Write("Digite seu palpite: ");
        
        if (!int.TryParse(Console.ReadLine(), out int palpite) || palpite < 1 || palpite > limiteMax)
        {
            Console.WriteLine($"Por favor, digite um número válido entre 1 e {limiteMax}.");
            continue;
        }

        if (palpite == valorSecreto)
        {
            acertou = true;
        }
        else
        {
            tentativasRestantes--;
            pontuacao -= 150; 
            string dica = palpite > valorSecreto ? "ALTO" : "BAIXO";
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Errado! Seu palpite foi muito {dica}.");
            Console.ResetColor();
        }
    }

    Console.WriteLine("\n-------------------------------------------");
    if (acertou)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"PARABÉNS! Você acertou! O número era {valorSecreto}.");
        Console.WriteLine($"Sua pontuação final foi: {pontuacao} pontos.");
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($"FIM DE JOGO! Suas tentativas acabaram.");
        Console.WriteLine($"O número secreto era: {valorSecreto}.");
    }
    Console.ResetColor();

    Console.WriteLine("\nDeseja jogar novamente? (S/N)");
    string resposta = Console.ReadLine()?.ToUpper();
    jogarNovamente = (resposta == "S" || resposta == "SIM");
}

Console.WriteLine("Obrigado por jogar!");
*/
// Versão 2 - jogo completo com dicas inteligentes, histórico visual, dicas especiais de emergência, ranking persistente, modo cronometrado e interface colorida 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.CursorVisible  = true;

const string RankingFile = "ranking.txt";
var rankings = LoadRanking();
var rng      = new Random();


bool playAgain = true;
while (playAgain)
{
    ShowBanner();
    ShowRankingPreview(rankings);

    var (label, limiteMax, maxTent, temTempo, tempoSeg) = ChooseDifficulty();

    if (label == "SAIR") break;

    Console.Clear();
    ShowBanner();

    int valorSecreto = rng.Next(1, limiteMax + 1);
    int tentativas   = maxTent;
    int pontuacao    = 1000;
    bool acertou     = false;
    bool tempoEsgotado = false;

    var palpites    = new List<int>();
    var stopwatch   = System.Diagnostics.Stopwatch.StartNew();
    var cts         = new System.Threading.CancellationTokenSource();

    Thread? timerThread = null;
    if (temTempo)
    {
        timerThread = new Thread(() =>
        {
            while (!cts.Token.IsCancellationRequested)
            {
                int elapsed   = (int)stopwatch.Elapsed.TotalSeconds;
                int remaining = tempoSeg - elapsed;
                if (remaining <= 0) { tempoEsgotado = true; return; }
                Thread.Sleep(200);
            }
        }) { IsBackground = true };
        timerThread.Start();
    }

    while (tentativas > 0 && !acertou && !tempoEsgotado)
    {
        RedrawGame(label, limiteMax, maxTent, tentativas, pontuacao,
                   palpites, temTempo, tempoSeg, stopwatch);

        string? linha = null;
        if (temTempo)
        {
            var inputThread = new Thread(() => { linha = Console.ReadLine(); })
                              { IsBackground = true };
            inputThread.Start();

            while (inputThread.IsAlive)
            {
                if (tempoEsgotado) break;
                Thread.Sleep(50);
            }

            if (tempoEsgotado) break;
        }
        else
        {
            linha = Console.ReadLine();
        }

        if (!int.TryParse(linha, out int palpite) || palpite < 1 || palpite > limiteMax)
        {
            ShowWarning($"Digite um número válido entre 1 e {limiteMax}.");
            continue;
        }

        if (palpites.Contains(palpite))
        {
            ShowWarning("Você já tentou esse número!");
            continue;
        }

        palpites.Add(palpite);

        if (palpite == valorSecreto)
        {
            acertou = true;
            if (temTempo)
            {
                int sobrou = tempoSeg - (int)stopwatch.Elapsed.TotalSeconds;
                pontuacao += sobrou * 5;
            }
        }
        else
        {
            tentativas--;
            pontuacao = Math.Max(0, pontuacao - 150);
            MostraDica(palpite, valorSecreto, limiteMax, tentativas, palpites, rng);
        }
    }

    cts.Cancel();
    stopwatch.Stop();

    Console.Clear();
    ShowBanner();

    if (acertou)
    {
        ShowResult(won: true, valorSecreto, pontuacao, palpites.Count, stopwatch.Elapsed);

        string nome = AskName();
        rankings.Add(new RankEntry(nome, pontuacao, label, palpites.Count,
                                   (int)stopwatch.Elapsed.TotalSeconds));
        rankings = rankings.OrderByDescending(r => r.Score).Take(5).ToList();
        SaveRanking(rankings);
    }
    else
    {
        ShowResult(won: false, valorSecreto, pontuacao, palpites.Count, stopwatch.Elapsed,
                   tempoEsgotado);
    }

    ShowFullRanking(rankings);

    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write("\n  Jogar novamente? (S/N): ");
    Console.ResetColor();
    string? resp = Console.ReadLine()?.Trim().ToUpper();
    playAgain = resp is "S" or "SIM";
}

Console.Clear();
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("\nObrigado por jogar!\n");
Console.ResetColor();


void RedrawGame(string diff, int lim, int maxT, int tent, int pts,
                List<int> hist, bool temTempo, int tempoSeg,
                System.Diagnostics.Stopwatch sw)
{
    Console.Clear();
    ShowBanner();

    Console.ForegroundColor = ConsoleColor.DarkCyan;
    Console.WriteLine($"  Dificuldade: {diff}   |   Range: 1 a {lim}");
    Console.ResetColor();
    Separator();

    if (temTempo)
    {
        int restante = Math.Max(0, tempoSeg - (int)sw.Elapsed.TotalSeconds);
        var cor = restante > tempoSeg * 0.4 ? ConsoleColor.Green
                : restante > tempoSeg * 0.2 ? ConsoleColor.Yellow
                :                             ConsoleColor.Red;
        Console.ForegroundColor = cor;
        Console.WriteLine($"  ⏱  Tempo restante: {restante}s  {TimerBar(restante, tempoSeg)}");
        Console.ResetColor();
    }

    Console.Write("  Tentativas: ");
    DrawAttemptBar(tent, maxT);
    Console.WriteLine($"  ({tent}/{maxT})");

    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.WriteLine($"  Pontuação atual: {pts}");
    Console.ResetColor();

    if (hist.Count > 0)
    {
        Separator();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("  Seus palpites: ");
        var sorted = hist.OrderBy(x => x).ToList();
        foreach (int p in sorted)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"{p}  ");
        }
        Console.ResetColor();
        Console.WriteLine();
    }

    Separator();
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write($"\n  Seu palpite (1 a {lim}): ");
    Console.ResetColor();
}

void MostraDica(int palpite, int segredo, int lim, int tentativasRestantes,
                List<int> hist, Random r)
{
    int dist     = Math.Abs(palpite - segredo);
    double ratio = (double)dist / lim;

    string temp;
    ConsoleColor cor;
    if      (ratio < 0.05) { temp = "QUEIMANDO!";    cor = ConsoleColor.Red;     }
    else if (ratio < 0.12) { temp = "Muito quente!"; cor = ConsoleColor.Yellow;  }
    else if (ratio < 0.25) { temp = "Quente!";       cor = ConsoleColor.DarkYellow; }
    else if (ratio < 0.45) { temp = "Frio...";        cor = ConsoleColor.Cyan;   }
    else                   { temp = "Muito frio!";    cor = ConsoleColor.Blue;    }

    string direcao = palpite > segredo ? "▼ MAIS BAIXO" : "▲ MAIS ALTO";

    Console.WriteLine();
    Console.ForegroundColor = cor;
    Console.WriteLine($"  {temp}   {direcao}");
    Console.ResetColor();

    if (tentativasRestantes <= 3 && tentativasRestantes > 0)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write("Dica especial: o número é ");

        int tipoDica = r.Next(3);
        switch (tipoDica)
        {
            case 0:
                Console.WriteLine(segredo % 2 == 0 ? "PAR." : "ÍMPAR.");
                break;
            case 1:
                int dezena = (segredo / 10) * 10;
                Console.WriteLine($"entre {dezena + 1} e {dezena + 10}.");
                break;
            case 2:
                var digitos  = segredo.ToString().Select(c => int.Parse(c.ToString())).ToList();
                int somaD    = digitos.Sum();
                Console.WriteLine($"a soma dos seus dígitos é {somaD}.");
                break;
        }
        Console.ResetColor();
    }

    Thread.Sleep(1800);
}

void ShowResult(bool won, int segredo, int pts, int numPalpites,
                TimeSpan elapsed, bool tempoEsgotado = false)
{
    Separator();
    if (won)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("PARABÉNS! Você acertou!");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"  Número secreto : {segredo}");
        Console.WriteLine($"  Palpites usados: {numPalpites}");
        Console.WriteLine($"  Tempo           : {elapsed.TotalSeconds:F1}s");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"  Pontuação final : {pts} pts");
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(tempoEsgotado
            ? "TEMPO ESGOTADO! Não foi desta vez..."
            : "FIM DE JOGO! Suas tentativas acabaram.");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"  O número secreto era: {segredo}");
    }
    Console.ResetColor();
    Separator();
}

string AskName()
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write("\n  Você entrou no ranking! Seu nome: ");
    Console.ResetColor();
    string? nome = Console.ReadLine()?.Trim();
    return string.IsNullOrWhiteSpace(nome) ? "Anônimo" : nome[..Math.Min(nome.Length, 12)];
}

void ShowRankingPreview(List<RankEntry> rank)
{
    if (rank.Count == 0) return;
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.WriteLine("Top pontuações:");
    for (int i = 0; i < Math.Min(3, rank.Count); i++)
    {
        var e = rank[i];
        Console.ForegroundColor = i == 0 ? ConsoleColor.Yellow
                                : i == 1 ? ConsoleColor.Gray
                                :           ConsoleColor.DarkYellow;
        Console.WriteLine($"  {i + 1}. {e.Name,-12} {e.Score,5} pts  [{e.Difficulty}]");
    }
    Console.ResetColor();
    Separator();
}

void ShowFullRanking(List<RankEntry> rank)
{
    if (rank.Count == 0) return;
    Separator();
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.WriteLine("RANKING GERAL — Top 5");
    Separator();
    string[] medals = { "1", "2", "3", "  4.", "  5." };
    for (int i = 0; i < rank.Count; i++)
    {
        var e = rank[i];
        Console.ForegroundColor = i < 3 ? ConsoleColor.Yellow : ConsoleColor.Gray;
        Console.WriteLine($"  {medals[i]}  {e.Name,-12} {e.Score,5} pts" +
                          $"  [{e.Difficulty}]  {e.Guesses} palpites  {e.Seconds}s");
    }
    Console.ResetColor();
    Separator();
}

List<RankEntry> LoadRanking()
{
    var list = new List<RankEntry>();
    if (!File.Exists(RankingFile)) return list;
    foreach (var line in File.ReadAllLines(RankingFile))
    {
        var p = line.Split('|');
        if (p.Length == 5 &&
            int.TryParse(p[1], out int sc) &&
            int.TryParse(p[3], out int gu) &&
            int.TryParse(p[4], out int se))
            list.Add(new RankEntry(p[0], sc, p[2], gu, se));
    }
    return list.OrderByDescending(r => r.Score).Take(5).ToList();
}

void SaveRanking(List<RankEntry> rank)
{
    try
    {
        File.WriteAllLines(RankingFile,
            rank.Select(e => $"{e.Name}|{e.Score}|{e.Difficulty}|{e.Guesses}|{e.Seconds}"));
    }
    catch { }
}
(string label, int lim, int tent, bool temTempo, int tempoSeg) ChooseDifficulty()
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("  Escolha a dificuldade:\n");
    Console.ResetColor();

    var opts = new[]
    {
        ("1", "Fácil",             50,  10, false,  0),
        ("2", "Médio",            100,   7, false,  0),
        ("3", "Difícil",          200,   5, false,  0),
        ("4", "Contra o Tempo",  100,  99, true,  45),
    };

    foreach (var (k, n, lim, t, tm, seg) in opts)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"  [{k}] {n,-22}");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        string desc = tm ? $"1 a {lim} | {seg}s" : $"1 a {lim} | {t} tentativas";
        Console.WriteLine(desc);
    }

    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine($"  [N] Sair");
    Console.ResetColor();
    Console.Write("\n  Sua escolha: ");

    while (true)
    {
        string? inp = Console.ReadLine()?.Trim().ToUpper();
        switch (inp)
        {
            case "1": return ("Fácil",            50,  10, false,  0);
            case "2": return ("Médio",            100,   7, false,  0);
            case "3": return ("Difícil",          200,   5, false,  0);
            case "4": return ("Contra o Tempo", 100,  99, true,  45);
            case "N": return ("SAIR",             -1,    0, false,  0);
            default:
                ShowWarning("Digite 1, 2, 3, 4 ou N.");
                Console.Write("  Sua escolha: ");
                break;
        }
    }
}
void ShowBanner()
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine(@"
   ██████╗ ██╗   ██╗███████╗███████╗███████╗
  ██╔════╝ ██║   ██║██╔════╝██╔════╝██╔════╝
  ██║  ███╗██║   ██║█████╗  ███████╗███████╗
  ██║   ██║██║   ██║██╔══╝  ╚════██║╚════██║
  ╚██████╔╝╚██████╔╝███████╗███████║███████║
   ╚═════╝  ╚═════╝ ╚══════╝╚══════╝╚══════╝  A NUMBER
");
    Console.ResetColor();
}

void Separator()
{
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("  " + new string('─', 52));
    Console.ResetColor();
}

void ShowWarning(string msg)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"\n{msg}");
    Console.ResetColor();
    Thread.Sleep(900);
}

void DrawAttemptBar(int restante, int total)
{
    int filled = (int)Math.Round((double)restante / total * 12);
    Console.ForegroundColor = restante > total * 0.5 ? ConsoleColor.Green
                            : restante > total * 0.25 ? ConsoleColor.Yellow
                            :                           ConsoleColor.Red;
    Console.Write("[" + new string('█', filled) + new string('░', 12 - filled) + "] ");
    Console.ResetColor();
}

string TimerBar(int restante, int total)
{
    int filled = (int)Math.Round((double)restante / total * 10);
    return "[" + new string('▰', filled) + new string('▱', 10 - filled) + "]";
}
record RankEntry(string Name, int Score, string Difficulty, int Guesses, int Seconds);