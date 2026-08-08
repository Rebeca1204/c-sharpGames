// Versão 1 - Simples quiz de console em C# que sorteia letras aleatórias e pede ao usuário o código ASCII correspondente
// Encerra o jogo na primeira resposta errada e conta quantas acertou em sequência
/*
using System;
Random random = new Random();
char[] chars = {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' ,'H' , 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'};
bool win;
int i =0;
do {
    Console.Clear();
    int x = random.Next(0,25);
    Console.WriteLine($"Question {i+1}: What is the number in the ASCII table equivalent to: {chars[x]} ");
    int answer;
    bool ok = int.TryParse(Console.ReadLine(), out answer);
    while (!ok){
        Console.WriteLine("Answer must be a number. Try again:");
        ok = int.TryParse(Console.ReadLine(), out answer);
    }
    if (answer == (int)chars[x]){
        Console.WriteLine("Correct!");
        Thread.Sleep(500);
        win = true;
        i++;
    }else {
        Console.WriteLine($"Wrong answer! Correct: {(int) chars[x]}");
        Thread.Sleep(500);
        win = false;
    }

}while (win);
*/
// Versão 2 - Quiz completo com múltiplos modos de jogo — tabela ASCII, programação em C#/.NET, modo misto e "Meu Material"
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.CursorVisible  = true;

const string RankingFile  = "quiz_ranking.txt";
const int    TimeLimitSec = 15;
const int    MaxLives     = 3;

var rng      = new Random();
var rankings = LoadRanking();

char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

var specialAscii = new (char ch, string hint)[]
{
    (' ', "espaço"),      ('!', "exclamação"), ('"', "aspas duplas"),
    ('#', "hashtag"),     ('$', "cifrão"),     ('%', "porcentagem"),
    ('&', "e comercial"), ('(', "abre parên"), (')', "fecha parên"),
    ('*', "asterisco"),   ('+', "mais"),        (',', "vírgula"),
    ('-', "hífen"),       ('.', "ponto"),        ('/', "barra"),
    (':', "dois pontos"), (';', "ponto-vírgula"),('<', "menor que"),
    ('=', "igual"),       ('>', "maior que"),    ('?', "interrogação"),
    ('@', "arroba"),      ('[', "abre colch"),   (']', "fecha colch"),
    ('^', "circunflexo"), ('_', "underscore"),   ('`', "crase"),
    ('{', "abre chave"),  ('|', "pipe"),         ('}', "fecha chave"),
    ('~', "til"),
};

var csharpQuestions = new Question[]
{
    new MC("Qual keyword em C# é usada para herança?",
           new[]{"implements","extends",":","inherits"}, 2,
           "Em C#, herança usa ':' — ex: class Dog : Animal"),

    new MC("O que é um 'namespace' em C#?",
           new[]{"Um tipo de variável","Um contêiner lógico para organizar código",
                 "Uma função especial","Um loop"},1,
           "Namespaces organizam classes e evitam conflitos de nomes"),

    new MC("Qual é o tipo inteiro padrão de 64 bits em C#?",
           new[]{"int","short","long","uint"},2,
           "'long' armazena inteiros de 64 bits (-9.2E18 a 9.2E18)"),

    new MC("O que faz o operador '??' em C#?",
           new[]{"Compara dois valores","Retorna o operando direito se o esquerdo for null",
                 "Declara variável nullable","Lança exceção se null"},1,
           "Null-coalescing operator: valor ?? valorPadrão"),

    new MC("Qual coleção garante unicidade dos elementos em C#?",
           new[]{"List<T>","Array","HashSet<T>","Queue<T>"},2,
           "HashSet<T> não permite duplicatas e tem busca O(1)"),

    new MC("O que é LINQ em C#?",
           new[]{"Uma biblioteca gráfica","Language Integrated Query para coleções",
                 "Um framework web","Um tipo de loop"},1,
           "LINQ permite consultas em coleções com sintaxe tipo SQL"),

    new MC("Qual modificador torna um método substituível em subclasses?",
           new[]{"static","abstract","virtual","sealed"},2,
           "'virtual' permite override; 'abstract' obriga override"),

    new MC("O que é 'async/await' em C#?",
           new[]{"Sincronização de threads","Programação assíncrona sem bloquear a thread",
                 "Um tipo de loop","Tratamento de exceções"},1,
           "async/await permite operações I/O sem travar a execução"),

    new MC("Qual a diferença entre 'struct' e 'class' em C#?",
           new[]{"Nenhuma diferença","struct é valor, class é referência",
                 "struct não tem métodos","class não tem campos"},1,
           "struct fica na stack (valor); class fica no heap (referência)"),

    new MC("O que faz 'Console.ReadLine()' em C#?",
           new[]{"Escreve no console","Lê uma tecla","Lê uma linha de texto do console",
                 "Limpa o console"},2,
           "ReadLine() lê até o usuário pressionar Enter, retorna string?"),

    new MC("O que é uma 'interface' em C#?",
           new[]{"Uma janela gráfica","Um contrato de métodos sem implementação",
                 "Um tipo de variável","Uma classe abstrata"},1,
           "Interface define o 'o quê', não o 'como' — sem implementação"),

    new MC("Qual keyword lança uma exceção em C#?",
           new[]{"raise","error","throw","exception"},2,
           "'throw new Exception(msg)' lança; 'try/catch' captura"),

    new MC("O que é o .NET Runtime?",
           new[]{"Um editor de código","O ambiente que executa aplicações .NET",
                 "Um banco de dados","Um servidor web"},1,
           "O runtime gerencia memória, GC e execução do código compilado"),

    new MC("O que significa 'GC' no contexto do .NET?",
           new[]{"Graphics Controller","Garbage Collector","General Class","Global Config"},1,
           "Garbage Collector libera memória de objetos não referenciados automaticamente"),

    new MC("Qual método converte string para inteiro em C#?",
           new[]{"int.Parse() ou int.TryParse()","Convert.ToChar()","string.ToInt()","Parse.Int()"},0,
           "int.Parse() lança exceção; int.TryParse() retorna bool — mais seguro"),

    new MC("O que é 'var' em C#?",
           new[]{"Uma variável sem tipo","Tipo inferido pelo compilador em tempo de compilação",
                 "Variável global","Tipo dynamic"},1,
           "'var' é tipagem estática inferida — o tipo é definido pelo compilador"),

    new MC("Qual coleção preserva a ordem de inserção e permite duplicatas em C#?",
           new[]{"HashSet<T>","Dictionary<K,V>","List<T>","SortedSet<T>"},2,
           "List<T> é a coleção mais usada: ordenada por inserção, permite duplicatas"),

    new MC("O que é 'sealed' em uma classe C#?",
           new[]{"A classe é abstrata","A classe não pode ser herdada",
                 "A classe é estática","A classe é interna"},1,
           "'sealed class' impede herança — como string em C#"),

    new MC("Qual operador verifica tipo E faz cast seguro em C#?",
           new[]{"is","as","(T)","typeof"},1,
           "'as' retorna null se falhar; '(T)' lança InvalidCastException"),

    new MC("O que é um 'record' em C#?",
           new[]{"Um arquivo de log","Tipo de referência imutável com igualdade por valor",
                 "Uma struct especial","Um array nomeado"},1,
           "record é ideal para DTOs — igualdade compara propriedades, não referência"),
};

bool playAgain = true;
while (playAgain)
{
    ShowBanner();
    ShowRankingPreview(rankings);

    var (mode, customPool) = ChooseMode();
    if (mode == "SAIR") break;

    var result = RunQuiz(mode, customPool);

    ShowBanner();
    ShowResultScreen(result);

    if (result.Score > 0)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("\n  Seu nome para o ranking: ");
        Console.ResetColor();
        string name = (Console.ReadLine()?.Trim() is { Length: > 0 } n)
                      ? n[..Math.Min(n.Length, 12)]
                      : "Anônimo";

        rankings.Add(new RankEntry(name, result.Score, result.Mode,
                                   result.Total, result.Correct));
        rankings = rankings.OrderByDescending(r => r.Score).Take(5).ToList();
        SaveRanking(rankings);
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
Console.WriteLine("\n  Até a próxima!\n");
Console.ResetColor();

QuizResult RunQuiz(string mode, List<Question>? forcedPool = null)
{
    int  lives   = MaxLives;
    int  score   = 0;
    int  streak  = 0;
    int  correct = 0;
    int  total   = 0;

    var pool = forcedPool ?? BuildQuestionPool(mode, rng);
    int qIdx = 0;

    while (lives > 0 && qIdx < pool.Count)
    {
        var q = pool[qIdx++];
        total++;

        ShowBanner();
        DrawHUD(lives, score, streak, total - 1, mode);
        Separator();

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"\n  Q{total}/{pool.Count}: {q.Text}\n");
        Console.ResetColor();

        bool    answered  = false;
        bool    timedOut  = false;
        string? userInput = null;

        var sw  = Stopwatch.StartNew();
        var cts = new CancellationTokenSource();

        var timerThread = new Thread(() =>
        {
            while (!cts.IsCancellationRequested)
            {
                int rem = TimeLimitSec - (int)sw.Elapsed.TotalSeconds;
                if (rem <= 0) { timedOut = true; return; }
                try
                {
                    int curTop  = Console.CursorTop;
                    int curLeft = Console.CursorLeft;
                    Console.SetCursorPosition(2, 4);
                    DrawTimerBar(rem, TimeLimitSec);
                    Console.SetCursorPosition(curLeft, curTop);
                }
                catch { }
                Thread.Sleep(250);
            }
        }) { IsBackground = true };
        timerThread.Start();

        if (q is MC mc)
        {
            char[] opts = { 'A', 'B', 'C', 'D' };
            for (int i = 0; i < mc.Options.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write($"  [{opts[i]}] ");
                Console.ResetColor();
                Console.WriteLine(mc.Options[i]);
            }
            Console.Write("\n  Sua resposta (A/B/C/D): ");

            var inputThread = new Thread(() =>
            {
                while (!timedOut && !answered)
                {
                    if (Console.KeyAvailable)
                    {
                        char k = char.ToUpper(Console.ReadKey(true).KeyChar);
                        if (k is 'A' or 'B' or 'C' or 'D')
                        {
                            userInput = k.ToString();
                            answered  = true;
                        }
                    }
                    Thread.Sleep(50);
                }
            }) { IsBackground = true };
            inputThread.Start();

            while (!answered && !timedOut) Thread.Sleep(50);
        }
        else
        {
            Console.Write("  Sua resposta: ");
            var inputThread = new Thread(() =>
            {
                userInput = Console.ReadLine();
                answered  = true;
            }) { IsBackground = true };
            inputThread.Start();

            while (!answered && !timedOut) Thread.Sleep(50);
        }

        cts.Cancel();
        sw.Stop();
        int elapsed = (int)sw.Elapsed.TotalSeconds;

        Console.WriteLine();
        Separator();

        if (timedOut && !answered)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  TEMPO ESGOTADO! Resposta: {q.CorrectDisplay}");
            Console.ResetColor();
            lives--;
            streak = 0;
        }
        else
        {
            bool isCorrect = q.Check(userInput ?? "");

            if (isCorrect)
            {
                streak++;
                int multiplier = streak >= 5 ? 3 : streak >= 3 ? 2 : 1;
                int timeBonus  = Math.Max(0, (TimeLimitSec - elapsed) * 2);
                int gained     = (100 + timeBonus) * multiplier;
                score  += gained;
                correct++;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("  CORRETO! ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write($"+{gained} pts");
                if (multiplier > 1)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write($"  x{multiplier} streak ({streak} seguidas!)");
                }
                Console.ResetColor();
                Console.WriteLine();
            }
            else
            {
                lives--;
                streak = 0;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"  ERRADO!  Resposta correta: {q.CorrectDisplay}");
                Console.ResetColor();
            }

            if (!string.IsNullOrWhiteSpace(q.Explanation))
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"  {q.Explanation}");
                Console.ResetColor();
            }
        }

        DrawLives(lives);
        Thread.Sleep(lives > 0 ? 2200 : 1500);
    }

    return new QuizResult(score, correct, total, mode);
}

(string mode, List<Question>? pool) ChooseMode()
{
    while (true)
    {
        ShowBanner();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("  Escolha o modo:\n");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("  [1] ASCII          — tabela ASCII (letras, especiais, faixas)");
        Console.WriteLine("  [2] C# / .NET      — perguntas de programação");
        Console.WriteLine("  [3] Misto          — mistura os dois temas");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("  [4] Meu Material   — carrega quiz de um arquivo .txt");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("  [N] Sair");
        Console.ResetColor();
        Console.Write("\n  Sua escolha: ");

        string? inp = Console.ReadLine()?.Trim().ToUpper();
        switch (inp)
        {
            case "1": return ("ASCII",  null);
            case "2": return ("CSHARP", null);
            case "3": return ("MISTO",  null);
            case "4":
                var (pool, label) = LoadCustomMode();
                if (pool != null) return (label, pool);
                break;
            case "N": return ("SAIR", null);
            default:
                ShowWarning("Digite 1, 2, 3, 4 ou N.");
                break;
        }
    }
}
(List<Question>? pool, string label) LoadCustomMode()
{
    ShowBanner();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("  ── MODO: MEU MATERIAL ──\n");
    Console.ResetColor();

    var txts = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.txt")
                        .Where(f => !Path.GetFileName(f).Equals(RankingFile,
                               StringComparison.OrdinalIgnoreCase))
                        .ToArray();

    if (txts.Length == 0)
    {
        ShowWarning("Nenhum arquivo .txt encontrado na pasta atual.");
        PrintCustomFormatHelp();
        Console.ReadKey(true);
        return (null, "");
    }

    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("  Arquivos disponíveis:\n");
    for (int i = 0; i < txts.Length; i++)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"  [{i + 1}] ");
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine(Path.GetFileName(txts[i]));
    }
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("\n  [H] Ver formato esperado   [V] Voltar");
    Console.ResetColor();
    Console.Write("\n  Escolha: ");

    string? choice = Console.ReadLine()?.Trim().ToUpper();

    if (choice == "V" || choice == "VOLTAR") return (null, "");
    if (choice == "H")
    {
        PrintCustomFormatHelp();
        Console.ReadKey(true);
        return (null, "");
    }

    if (!int.TryParse(choice, out int idx) || idx < 1 || idx > txts.Length)
    {
        ShowWarning("Opção inválida.");
        return (null, "");
    }

    string filePath = txts[idx - 1];
    string fileName = Path.GetFileNameWithoutExtension(filePath);

    var (questions, report) = ParseCustomFile(filePath);

    if (questions.Count == 0)
    {
        ShowBanner();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("  Nenhuma pergunta válida encontrada no arquivo.\n");
        Console.ResetColor();
        PrintCustomFormatHelp();
        Console.ReadKey(true);
        return (null, "");
    }

    ShowDifficultyReport(fileName, questions, report);

    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write("\n  Iniciar quiz com este material? (S/N): ");
    Console.ResetColor();
    string? confirm = Console.ReadLine()?.Trim().ToUpper();
    if (confirm is not ("S" or "SIM")) return (null, "");

    var pool = questions.OrderBy(_ => rng.Next()).Take(Math.Min(20, questions.Count)).ToList();
    string label = $"TXT:{fileName[..Math.Min(fileName.Length, 8)]}";
    return (pool, label);
}

(List<Question> questions, ParseReport report) ParseCustomFile(string path)
{
    var questions   = new List<Question>();
    var lines       = File.ReadAllLines(path)
                          .Select(l => l.Trim())
                          .Where(l => !string.IsNullOrWhiteSpace(l) && !l.StartsWith('#'))
                          .ToArray();

    int errCount    = 0;
    int mcCount     = 0;
    int freeCount   = 0;
    int warnCount   = 0;
    var warnings    = new List<string>();

    string? pendingQ    = null;
    string? pendingR    = null;
    string? pendingO    = null;
    string? pendingHint = null;

    void TryFlush()
    {
        if (pendingQ == null || pendingR == null) return;

        if (pendingQ.Length < 15)
        {
            warnings.Add($"Pergunta muito curta: \"{pendingQ}\"");
            warnCount++;
        }
        if (pendingR.Contains('/') && pendingO == null)
        {
            warnings.Add($"Possível resposta ambígua (contém '/'): \"{pendingR}\"");
            warnCount++;
        }

        if (pendingO != null)
        {
            var opts = pendingO.Split('|').Select(o => o.Trim()).ToArray();
            if (opts.Length < 2 || opts.Length > 4)
            {
                warnings.Add($"Opções inválidas (use 2–4 separadas por '|'): \"{pendingO}\"");
                errCount++;
                pendingQ = pendingR = pendingO = pendingHint = null;
                return;
            }
            string correctAnswer = opts[0];
            var shuffled = opts.OrderBy(_ => rng.Next()).ToArray();
            int correctIdx = Array.IndexOf(shuffled, correctAnswer);

            if (correctIdx < 0) { shuffled[0] = correctAnswer; correctIdx = 0; }

            questions.Add(new MC(pendingQ, shuffled, correctIdx, pendingHint ?? ""));
            mcCount++;
        }
        else
        {
            questions.Add(new Free(pendingQ, pendingR, pendingHint ?? ""));
            freeCount++;
        }

        pendingQ = pendingR = pendingO = pendingHint = null;
    }

    foreach (var line in lines)
    {
        if (line.StartsWith("P:", StringComparison.OrdinalIgnoreCase))
        {
            TryFlush();
            pendingQ = line[2..].Trim();
        }
        else if (line.StartsWith("R:", StringComparison.OrdinalIgnoreCase))
        {
            pendingR = line[2..].Trim();
        }
        else if (line.StartsWith("O:", StringComparison.OrdinalIgnoreCase))
        {
            pendingO = line[2..].Trim();
        }
        else if (line.StartsWith("D:", StringComparison.OrdinalIgnoreCase))
        {
            pendingHint = line[2..].Trim();
        }
    }
    TryFlush();

    var report = new ParseReport(mcCount, freeCount, errCount, warnCount, warnings);
    return (questions, report);
}

void ShowDifficultyReport(string fileName, List<Question> questions, ParseReport report)
{
    ShowBanner();
    Separator();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"  ANÁLISE: {fileName}\n");
    Console.ResetColor();

    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine($"  Total de perguntas : {questions.Count}");
    Console.WriteLine($"  Múltipla escolha   : {report.McCount}");
    Console.WriteLine($"  Discursivas        : {report.FreeCount}");

    int diffScore = EstimateDifficulty(questions, report);
    string diffLabel = diffScore switch
    {
        <= 30 => "Fácil",
        <= 60 => "Médio",
        <= 80 => "Difícil",
        _     => "Muito difícil"
    };
    var diffColor = diffScore switch
    {
        <= 30 => ConsoleColor.Green,
        <= 60 => ConsoleColor.Yellow,
        <= 80 => ConsoleColor.DarkYellow,
        _     => ConsoleColor.Red
    };
    int barFilled = diffScore / 5; // 0-20
    Console.Write("  Dificuldade estim. : ");
    Console.ForegroundColor = diffColor;
    Console.Write($"[{new string('█', barFilled)}{new string('░', 20 - barFilled)}] ");
    Console.WriteLine($"{diffLabel} ({diffScore}/100)");
    Console.ResetColor();

    if (questions.Count < 5)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("  ⚠  Banco pequeno — recomendado ter ao menos 5 perguntas.");
    }
    else if (questions.Count >= 20)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("  ✔  Banco robusto — variação garantida a cada partida.");
    }
    Console.ResetColor();

    if (report.WarnCount > 0)
    {
        Separator();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"  ⚠  {report.WarnCount} aviso(s) de qualidade:\n");
        foreach (var w in report.Warnings.Take(5))
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"    • {w}");
        }
        if (report.Warnings.Count > 5)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"    ... e mais {report.Warnings.Count - 5} aviso(s).");
        }
        Console.ResetColor();
    }

    if (report.ErrCount > 0)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n  ✖  {report.ErrCount} pergunta(s) ignorada(s) por erro de formato.");
        Console.ResetColor();
    }

    Separator();
}

int EstimateDifficulty(List<Question> questions, ParseReport report)
{
    if (questions.Count == 0) return 0;

    double score = 0;

    score += (double)report.FreeCount / questions.Count * 40;

    double avgLen = questions.Average(q => q.Text.Length);
    score += Math.Min(avgLen / 3.0, 20);

    if (questions.Count < 10) score += 10;

    int withHints = questions.Count(q => !string.IsNullOrWhiteSpace(q.Explanation));
    double hintRatio = (double)withHints / questions.Count;
    score -= hintRatio * 10;

    return Math.Clamp((int)score, 0, 100);
}

void PrintCustomFormatHelp()
{
    ShowBanner();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("  FORMATO DO ARQUIVO .TXT\n");
    Console.ResetColor();
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("  Crie um arquivo .txt na mesma pasta do quiz com este formato:\n");

    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("  # Linhas começando com # são comentários e são ignoradas");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine();
    Console.WriteLine("  # ── Questão discursiva (digitada):");
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("  P: Qual a capital do Brasil?");
    Console.WriteLine("  R: Brasília");
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("  D: Dica opcional aqui");
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("  # ── Múltipla escolha (O: primeira opção = CORRETA):");
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("  P: Quantos estados tem o Brasil?");
    Console.WriteLine("  R: 26");
    Console.WriteLine("  O: 26|24|25|27");
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("  D: O Brasil tem 26 estados + 1 Distrito Federal");
    Console.ResetColor();
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("  Campos: P (pergunta), R (resposta correta),");
    Console.WriteLine("          O (opções — 1ª é a certa), D (dica/explicação)");
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("  Pressione qualquer tecla para voltar...");
    Console.ResetColor();
}

List<Question> BuildQuestionPool(string mode, Random r)
{
    var pool = new List<Question>();

    if (mode is "ASCII" or "MISTO")
    {
        foreach (char c in letters)
            pool.Add(new Free(
                $"Qual é o código ASCII da letra '{c}'?",
                ((int)c).ToString(),
                $"'{c}' = {(int)c} na tabela ASCII"));

        var letterSubset = letters.OrderBy(_ => r.Next()).Take(10).ToList();
        foreach (char correct in letterSubset)
        {
            var wrong = letters.Where(c => c != correct)
                               .OrderBy(_ => r.Next()).Take(3).ToArray();
            var opts  = new[] { correct.ToString() }
                        .Concat(wrong.Select(c => c.ToString()))
                        .OrderBy(_ => r.Next()).ToArray();
            int idx   = Array.IndexOf(opts, correct.ToString());
            pool.Add(new MC(
                $"Qual caractere corresponde ao código ASCII {(int)correct}?",
                opts, idx,
                $"ASCII {(int)correct} = '{correct}'"));
        }

        foreach (var (ch, hint) in specialAscii)
        {
            var wrongVals = Enumerable.Range(32, 95)
                                      .Where(v => v != (int)ch)
                                      .OrderBy(_ => r.Next()).Take(3)
                                      .Select(v => v.ToString()).ToArray();
            var opts = new[] { ((int)ch).ToString() }
                       .Concat(wrongVals)
                       .OrderBy(_ => r.Next()).ToArray();
            int idx = Array.IndexOf(opts, ((int)ch).ToString());
            pool.Add(new MC(
                $"Qual é o código ASCII do caractere '{ch}' ({hint})?",
                opts, idx,
                $"'{ch}' ({hint}) = {(int)ch}"));
        }

        pool.AddRange(new Question[]
        {
            new MC("Qual faixa da tabela ASCII representa letras maiúsculas (A-Z)?",
                   new[]{"32–64","65–90","97–122","48–57"}, 1,
                   "Maiúsculas: 65 (A) a 90 (Z)"),
            new MC("Qual faixa representa letras minúsculas (a-z)?",
                   new[]{"65–90","48–57","97–122","32–47"}, 2,
                   "Minúsculas: 97 (a) a 122 (z)"),
            new MC("Qual faixa representa os dígitos 0-9?",
                   new[]{"0–9","32–41","48–57","58–64"}, 2,
                   "Dígitos: 48 ('0') a 57 ('9')"),
            new MC("Quantos caracteres a tabela ASCII padrão define?",
                   new[]{"128","256","64","512"}, 0,
                   "ASCII padrão: 128 caracteres (0–127)"),
            new MC("Qual é o código ASCII do caractere nulo (NULL)?",
                   new[]{"32","1","0","255"}, 2,
                   "NULL = 0; é usado como terminador de string em C"),
            new MC("Qual é o código ASCII do SPACE (espaço)?",
                   new[]{"0","31","32","64"}, 2,
                   "Espaço = 32; primeiro caractere imprimível da tabela"),
            new MC("Qual é o código ASCII de DEL (delete)?",
                   new[]{"8","26","127","255"}, 2,
                   "DEL = 127; último caractere da tabela ASCII padrão"),
            new MC("Qual é a diferença entre o código de 'A' e 'a'?",
                   new[]{"16","24","32","64"}, 2,
                   "A=65, a=97; diferença de 32 — serve para converter case com XOR"),
            new MC("O que é ASCII?",
                   new[]{"Linguagem de programação",
                         "Padrão de codificação de caracteres em 7 bits",
                         "Formato de arquivo de imagem",
                         "Protocolo de rede"}, 1,
                   "American Standard Code for Information Interchange — criado em 1963"),
            new MC("Qual caractere tem código ASCII 10?",
                   new[]{"TAB","SPACE","LF (Line Feed / nova linha)","CR (Carriage Return)"}, 2,
                   "LF=10 (\\n); CR=13 (\\r); TAB=9; SPACE=32"),
        });
    }

    if (mode is "CSHARP" or "MISTO")
        pool.AddRange(csharpQuestions);

    return pool.OrderBy(_ => r.Next()).Take(20).ToList();
}

void ShowResultScreen(QuizResult r)
{
    Separator();
    string emoji = r.Correct >= r.Total * 0.8 ? "🏆" :
                   r.Correct >= r.Total * 0.5 ? "👍" : "📚";
    Console.ForegroundColor = r.Correct >= r.Total / 2 ? ConsoleColor.Green : ConsoleColor.Red;
    Console.WriteLine(r.Correct >= r.Total * 0.8 ? $"  {emoji} EXCELENTE! Você domina o assunto!" :
                      r.Correct >= r.Total * 0.5 ? $"  {emoji} Bom trabalho! Continue assim." :
                                                    $"  {emoji} Continue praticando!");
    Console.ResetColor();
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine($"  Acertos   : {r.Correct}/{r.Total}  ({r.Correct * 100 / Math.Max(r.Total,1)}%)");
    Console.WriteLine($"  Modo      : {r.Mode}");
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.WriteLine($"  Pontuação : {r.Score} pts");
    Console.ResetColor();

    int filled = (int)Math.Round((double)r.Correct / Math.Max(r.Total, 1) * 20);
    Console.Write("  Desempenho: ");
    Console.ForegroundColor = filled >= 16 ? ConsoleColor.Green :
                              filled >= 10 ? ConsoleColor.Yellow : ConsoleColor.Red;
    Console.WriteLine($"[{new string('█', filled)}{new string('░', 20 - filled)}]");
    Console.ResetColor();
    Separator();
}

void ShowRankingPreview(List<RankEntry> rank)
{
    if (rank.Count == 0) return;
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.WriteLine("  Top jogadores:\n");
    string[] medals = { "1", "2", "3" };
    for (int i = 0; i < Math.Min(3, rank.Count); i++)
    {
        var e = rank[i];
        Console.ForegroundColor = i == 0 ? ConsoleColor.Yellow
                                : i == 1 ? ConsoleColor.Gray
                                :           ConsoleColor.DarkYellow;
        Console.WriteLine($"  {medals[i]}  {e.Name,-12} {e.Score,5} pts  [{e.Mode}]  {e.Correct}/{e.Total}");
    }
    Console.ResetColor();
    Separator();
}

void ShowFullRanking(List<RankEntry> rank)
{
    if (rank.Count == 0) return;
    Separator();
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.WriteLine("  RANKING GERAL — Top 5");
    Separator();
    string[] medals = { "1", "2", "3", "  4.", "  5." };
    for (int i = 0; i < rank.Count; i++)
    {
        var e = rank[i];
        Console.ForegroundColor = i < 3 ? ConsoleColor.Yellow : ConsoleColor.Gray;
        Console.WriteLine($"  {medals[i]}  {e.Name,-12} {e.Score,5} pts  [{e.Mode}]  {e.Correct}/{e.Total}");
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
            int.TryParse(p[3], out int co) &&
            int.TryParse(p[4], out int to))
            list.Add(new RankEntry(p[0], sc, p[2], co, to));
    }
    return list.OrderByDescending(r => r.Score).Take(5).ToList();
}

void SaveRanking(List<RankEntry> rank)
{
    try { File.WriteAllLines(RankingFile,
            rank.Select(e => $"{e.Name}|{e.Score}|{e.Mode}|{e.Correct}|{e.Total}")); }
    catch { }
}

void ShowBanner()
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine(@"
    ██████╗ ██╗   ██╗██╗███████╗
   ██╔═══██╗██║   ██║██║╚══███╔╝
   ██║   ██║██║   ██║██║  ███╔╝ 
   ██║▄▄ ██║██║   ██║██║ ███╔╝  
   ╚██████╔╝╚██████╔╝██║███████╗
    ╚══▀▀═╝  ╚═════╝ ╚═╝╚══════╝  ASCII + C# + Seu Material
");
    Console.ResetColor();
}

void DrawHUD(int lives, int score, int streak, int done, string mode)
{
    string livesStr = new string('♥', lives) + new string('♡', MaxLives - lives);
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write($"  {livesStr}   ");
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.Write($"Score: {score}   ");
    if (streak >= 2)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write($"Streak ×{streak}   ");
    }
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine($"[{mode}]  Q{done + 1}");
    Console.ResetColor();
}

void DrawTimerBar(int rem, int total)
{
    int filled = (int)Math.Round((double)rem / total * 20);
    var cor = rem > total * 0.5 ? ConsoleColor.Green
            : rem > total * 0.25 ? ConsoleColor.Yellow
            :                      ConsoleColor.Red;
    Console.ForegroundColor = cor;
    Console.Write($"  {rem,2}s [{new string('█', filled)}{new string('░', 20 - filled)}]   ");
    Console.ResetColor();
}

void DrawLives(int lives)
{
    Console.Write("\n  Vidas: ");
    Console.ForegroundColor = lives > 1 ? ConsoleColor.Green : ConsoleColor.Red;
    Console.WriteLine(new string('♥', lives) + new string('♡', MaxLives - lives));
    Console.ResetColor();
}

void Separator()
{
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("  " + new string('─', 54));
    Console.ResetColor();
}

void ShowWarning(string msg)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"\n  ⚠  {msg}");
    Console.ResetColor();
    Thread.Sleep(900);
}

abstract class Question
{
    public string Text        { get; }
    public string Explanation { get; }
    public abstract string CorrectDisplay { get; }
    public abstract bool   Check(string input);
    protected Question(string text, string explanation)
        { Text = text; Explanation = explanation; }
}

class MC : Question
{
    public  string[] Options      { get; }
    private int      CorrectIndex { get; }
    public override string CorrectDisplay =>
        $"{(char)('A' + CorrectIndex)} — {Options[CorrectIndex]}";

    public MC(string text, string[] opts, int correct, string explanation = "")
        : base(text, explanation) { Options = opts; CorrectIndex = correct; }

    public override bool Check(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;
        char k = char.ToUpper(input.Trim()[0]);
        return k - 'A' == CorrectIndex;
    }
}

class Free : Question
{
    private string[] Accepted { get; }
    public override string CorrectDisplay => Accepted[0];

    public Free(string text, string correct, string explanation = "")
        : base(text, explanation)
    {
        Accepted = correct.Split('/')
                          .Select(s => s.Trim())
                          .Where(s => s.Length > 0)
                          .ToArray();
        if (Accepted.Length == 0) Accepted = new[] { correct };
    }

    public override bool Check(string input)
    {
        string normalized = input.Trim();
        return Accepted.Any(a => a.Equals(normalized, StringComparison.OrdinalIgnoreCase));
    }
}

record ParseReport(int McCount, int FreeCount, int ErrCount, int WarnCount,
                   List<string> Warnings);
record QuizResult(int Score, int Correct, int Total, string Mode);
record RankEntry(string Name, int Score, string Mode, int Correct, int Total);