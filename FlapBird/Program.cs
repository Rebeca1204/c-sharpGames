using System;
using System.IO;
using System.Threading;

Console.CursorVisible = false;
Console.OutputEncoding = System.Text.Encoding.UTF8;

int height = Console.WindowHeight - 2;
int width  = Console.WindowWidth  - 5;

const string HighScoreFile = "highscore.txt";
const string CoinsFile     = "coins.txt";
const string SkinFile      = "skin.txt";

int highScore   = LoadInt(HighScoreFile);
int totalCoins  = LoadInt(CoinsFile);
int currentSkin = LoadInt(SkinFile);

var skins = new[]
{
    new Skin("Pássaro Padrão",  "~(^')>", "~(v')>", 0),
    new Skin("Morcego 🦇",      "v(^;^)>", "v(v;v)>", 10),
    new Skin("Aviãozinho ✈",   "_=[^]=>", "_=[v]=>", 25),
    new Skin("Alien 👾",        "¤(°o°)>", "¤(°-°)>", 50),
    new Skin("Dragão 🐉",       "~{^;^}>", "~{v;v}>", 100),
};

bool playAgain = true;

while (playAgain)
{
    height = Console.WindowHeight - 2;
    width  = Console.WindowWidth  - 5;

    int choice = ShowMainMenu(skins, currentSkin, highScore, totalCoins);

    if (choice == -1) { playAgain = false; break; }           // Sair
    if (choice == -2) { currentSkin = ShowShop(skins, ref totalCoins, currentSkin); continue; }

    playAgain = RunGame(skins[currentSkin]);
}

Console.ResetColor();
Console.Clear();
Console.WriteLine("Até logo!");


bool RunGame(Skin skin)
{
    var rng = new Random();

    int    birdX    = width / 6;
    double birdY    = height / 2.0;
    double velocity = 0;
    const double Gravity   = 0.20;
    const double JumpForce = -1.1;

    var trail = new double[] { birdY, birdY, birdY, birdY };

    int    pipeX        = width;
    int    gapSize      = 9;
    int    pipeTopH     = rng.Next(2, height - gapSize - 2);
    double pipeVelY     = 0;      
    bool   pipeMoving   = false;  

    bool coinActive = false;
    int  coinX = 0, coinY = 0;
    int  sessionCoins = 0;        

    int  points   = 0;
    int  gameSpeed = 35;

    Console.Clear();
    DrawBorder();

    while (true)
    {
        if (Console.WindowHeight - 2 != height || Console.WindowWidth - 5 != width)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Console redimensionado. Jogo encerrado.");
            Console.ResetColor();
            return false;
        }

        bool jumped = false;
        while (Console.KeyAvailable)
        {
            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.Spacebar || key == ConsoleKey.UpArrow)
            {
                velocity = JumpForce;
                jumped   = true;
                BeepAsync(400, 40);   
            }
        }

        string birdSprite = jumped || velocity < 0 ? skin.Up : skin.Down;

        for (int t = 0; t < trail.Length; t++)
        {
            int tx = birdX - (t + 1) * 2;
            if (tx >= 0)
            {
                Console.SetCursorPosition(tx, (int)Math.Round(trail[t]));
                Console.Write("  ");
            }
        }

        Console.SetCursorPosition(birdX, (int)Math.Round(birdY));
        Console.Write(new string(' ', birdSprite.Length));

        velocity += Gravity;
        trail[3] = trail[2]; trail[2] = trail[1]; trail[1] = trail[0];
        trail[0] = birdY;
        birdY += velocity;
        birdY = Math.Clamp(birdY, 1, height - 1);

        char[] trailChars = { '░', '·', '·', ' ' };
        for (int t = 0; t < trail.Length - 1; t++)
        {
            int tx = birdX - (t + 1) * 2;
            if (tx >= 0)
            {
                Console.ForegroundColor = t == 0 ? ConsoleColor.DarkYellow : ConsoleColor.DarkGray;
                Console.SetCursorPosition(tx, (int)Math.Round(trail[t]));
                Console.Write(trailChars[t]);
            }
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.SetCursorPosition(birdX, (int)Math.Round(birdY));
        Console.Write(birdSprite);
        Console.ResetColor();

        ClearPipe(pipeX, pipeTopH, gapSize);
        if (pipeMoving)
        {
            pipeTopH = (int)Math.Clamp(pipeTopH + pipeVelY, 2, height - gapSize - 2);
            if (pipeTopH <= 2 || pipeTopH >= height - gapSize - 2)
                pipeVelY = -pipeVelY;
        }

        pipeX--;

        if (coinActive)
        {
            EraseCoin(coinX, coinY);
            coinX--;
            if (coinX < 2) coinActive = false;
        }

        if (pipeX < 2)
        {
            ClearPipe(pipeX, pipeTopH, gapSize);
            points++;
            BeepAsync(880, 80);    

            if (points % 2 == 0 && gameSpeed > 20) gameSpeed -= 2;

            if (points == 10)
            {
                pipeMoving = true;
                pipeVelY   = 0.4;
            }

            pipeX    = width;
            pipeTopH = rng.Next(2, height - gapSize - 2);

            if (!coinActive && rng.NextDouble() < 0.40)
            {
                coinActive = true;
                coinX = pipeX - width / 4;         
                coinY = pipeTopH + gapSize / 2;   
            }
        }

        DrawPipe(pipeX, pipeTopH, gapSize);

        if (coinActive)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(coinX, coinY);
            Console.Write('$');
            Console.ResetColor();

            int birdRight = birdX + birdSprite.Length - 1;
            int bY = (int)Math.Round(birdY);
            if (bY == coinY && birdRight >= coinX && birdX <= coinX)
            {
                EraseCoin(coinX, coinY);
                coinActive = false;
                sessionCoins++;
                BeepAsync(1200, 60);
            }
        }

        DrawHud(points, highScore, sessionCoins, skin.Name);

        int birdInt = (int)Math.Round(birdY);
        if (Collides(birdInt, birdX, birdSprite.Length, pipeX, pipeTopH, gapSize))
        {
            BeepAsync(150, 500);   
            Thread.Sleep(150);

            totalCoins += sessionCoins;
            SaveInt(CoinsFile, totalCoins);

            if (points > highScore)
            {
                highScore = points;
                SaveInt(HighScoreFile, highScore);
            }

            return ShowGameOver(points, highScore, sessionCoins, totalCoins);
        }

        Thread.Sleep(gameSpeed);
    }
}

bool Collides(int bY, int bX, int bLen, int pX, int topH, int gap)
{
    if (bY <= 1 || bY >= height - 1) return true;
    int bRight = bX + bLen - 1;
    if (bRight >= pX && bX <= pX + 2)
        if (bY <= topH || bY >= topH + gap)
            return true;
    return false;
}

void DrawBorder()
{
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.SetCursorPosition(0, 0);
    Console.Write(new string('═', Math.Min(Console.WindowWidth, width + 5)));
    Console.SetCursorPosition(0, height);
    Console.Write(new string('═', Math.Min(Console.WindowWidth, width + 5)));
    Console.ResetColor();
}

void DrawPipe(int x, int topH, int gap)
{
    if (x <= 0 || x >= Console.WindowWidth - 3) return;
    Console.ForegroundColor = ConsoleColor.Green;
    for (int i = 1; i <= topH; i++)
    { Console.SetCursorPosition(x, i); Console.Write("║█║"); }

    Console.ForegroundColor = ConsoleColor.DarkGreen;
    Console.SetCursorPosition(x, topH);
    Console.Write("╚═╝");
    Console.SetCursorPosition(x, topH + gap);
    Console.Write("╔═╗");

    Console.ForegroundColor = ConsoleColor.Green;
    for (int i = topH + gap + 1; i < height; i++)
    { Console.SetCursorPosition(x, i); Console.Write("║█║"); }

    Console.ResetColor();
}

void ClearPipe(int x, int topH, int gap)
{
    if (x + 3 < 0 || x >= Console.WindowWidth) return;
    for (int i = 1; i < height; i++)
    {
        if (x >= 0 && x + 2 < Console.WindowWidth)
        { Console.SetCursorPosition(x, i); Console.Write("   "); }
    }
}

void EraseCoin(int x, int y)
{
    if (x >= 0 && x < Console.WindowWidth && y >= 0 && y < Console.WindowHeight)
    { Console.SetCursorPosition(x, y); Console.Write(' '); }
}

void DrawHud(int pts, int best, int coins, string skinName)
{
    Console.ForegroundColor = ConsoleColor.White;
    Console.SetCursorPosition(2, 0);
    Console.Write($" PONTOS: {pts,-4} RECORDE: {best,-4} ");
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.Write($"$ {coins}  ");
    Console.ForegroundColor = ConsoleColor.DarkCyan;
    Console.Write($"[{skinName}]  ");
    Console.ResetColor();
}

int ShowMainMenu(Skin[] sk, int activeSkin, int best, int coins)
{
    FlushKeys();
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine(@"
  ______ _             ____  _           _ 
 |  ____| |           |  _ \(_)         | |
 | |__  | | __ _ _ __ | |_) |_ _ __ __| |
 |  __| | |/ _` | '_ \|  _ <| | '__/ _` |
 | |    | | (_| | |_) | |_) | | | | (_| |
 |_|    |_|\__,_| .__/ |____/|_|_|  \__,_|
                 |_|                        ");

    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine($"\n  Recorde: {best} pts   |   Moedas: {coins} $");
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"  Skin ativa: {sk[activeSkin].Name}");
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("  [ENTER / ESPAÇO]  → Jogar");
    Console.WriteLine("  [L]               → Loja de Skins");
    Console.WriteLine("  [N]               → Sair");
    Console.ResetColor();

    while (true)
    {
        var k = Console.ReadKey(true).Key;
        if (k is ConsoleKey.Enter or ConsoleKey.Spacebar) return 0;
        if (k == ConsoleKey.L) return -2;
        if (k == ConsoleKey.N) return -1;
    }
}

int ShowShop(Skin[] sk, ref int coins, int current)
{
    while (true)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("  ╔══════════════════════════════╗");
        Console.WriteLine("  ║       🛒  LOJA DE SKINS       ║");
        Console.WriteLine("  ╚══════════════════════════════╝");
        Console.WriteLine($"\n  Suas moedas: {coins} $\n");

        for (int i = 0; i < sk.Length; i++)
        {
            bool owned    = coins >= sk[i].Price || sk[i].Price == 0;
            bool selected = i == current;

            Console.ForegroundColor = selected  ? ConsoleColor.Green
                                    : owned     ? ConsoleColor.White
                                    :             ConsoleColor.DarkGray;

            string status = selected ? "[ATIVA]"
                          : sk[i].Price == 0 ? "[GRÁTIS]"
                          : owned ? "[COMPRADA]"
                          : $"[{sk[i].Price} $]";

            Console.WriteLine($"  [{i + 1}] {sk[i].Up}  {sk[i].Name}  {status}");
        }

        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("\n  Pressione [1-5] para equipar/comprar.");
        Console.WriteLine("  [ESC] → Voltar ao menu.");
        Console.ResetColor();

        var k = Console.ReadKey(true).Key;
        if (k == ConsoleKey.Escape) return current;

        int idx = k switch
        {
            ConsoleKey.D1 => 0, ConsoleKey.D2 => 1, ConsoleKey.D3 => 2,
            ConsoleKey.D4 => 3, ConsoleKey.D5 => 4, _ => -1
        };

        if (idx < 0 || idx >= sk.Length) continue;

        if (sk[idx].Price == 0 || coins >= sk[idx].Price)
        {
            current = idx;
            SaveInt(SkinFile, current);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n  ✓ Skin \"{sk[idx].Name}\" equipada!");
            Thread.Sleep(900);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n  ✗ Moedas insuficientes! (precisa de {sk[idx].Price} $)");
            Thread.Sleep(1200);
        }
    }
}

bool ShowGameOver(int pts, int best, int sessionC, int totalC)
{
    FlushKeys();
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("\n  ╔═══════════════════════════╗");
    Console.WriteLine("  ║         GAME OVER!         ║");
    Console.WriteLine("  ╚═══════════════════════════╝");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine($"\n  Pontuação final : {pts}");
    Console.WriteLine($"  Recorde         : {best}");
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.WriteLine($"  Moedas coletadas: {sessionC} $  (total: {totalC} $)");

    if (pts >= best && pts > 0)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nNOVO RECORDE! Parabéns!");
    }

    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("\n  [ENTER / ESPAÇO] → Jogar de novo");
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("  [N]              → Sair");
    Console.ResetColor();

    while (true)
    {
        var k = Console.ReadKey(true).Key;
        if (k is ConsoleKey.Enter or ConsoleKey.Spacebar) return true;
        if (k == ConsoleKey.N) return false;
    }
}

void FlushKeys() { while (Console.KeyAvailable) Console.ReadKey(true); }

int  LoadInt(string file) =>
    File.Exists(file) && int.TryParse(File.ReadAllText(file), out int v) ? v : 0;

void SaveInt(string file, int value)
{
    try { File.WriteAllText(file, value.ToString()); } catch { }
}

void BeepAsync(int freq, int dur)
{
    try { new Thread(() => { try { Console.Beep(freq, dur); } catch { } })
           { IsBackground = true }.Start(); }
    catch { }
}

record Skin(string Name, string Up, string Down, int Price);