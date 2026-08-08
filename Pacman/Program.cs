// Versão 1 - 
/*
using System;

Random random = new Random();
Console.CursorVisible = false;
int height = Console.WindowHeight - 1;
int width = Console.WindowWidth - 5;
bool shouldExit = false;

int playerX = 0;
int playerY = 0;

int foodX = 0;
int foodY = 0;

string[] states = { "('-')", "(^-^)", "(X_X)" };
string[] foods = { "@@@@@", "$$$$$", "#####" };

string player = states[0];
int food = 0;

InitializeGame();

while (!shouldExit)
{
    if (TerminalResized())
    {
        Console.Clear();
        Console.Write("O console foi redimensionado. Encerrando o programa.");
        shouldExit = true;
    }
    else
    {
        if (PlayerIsSick())
        {
            FreezePlayer();
        }

        if (PlayerIsFaster())
        {
            Move(2, false);
        }
        else
        {
            Move(1, false); 
        }

        if (GotFood())
        {
            ChangePlayer();
            ShowFood();
        }
    }
}

bool TerminalResized()
{
    return height != Console.WindowHeight - 1 || width != Console.WindowWidth - 5;
}

void ShowFood()
{
    food = random.Next(0, foods.Length);

    foodX = random.Next(0, width - player.Length);
    foodY = random.Next(0, height - 1);

    Console.SetCursorPosition(foodX, foodY);
    Console.Write(foods[food]);
}

bool GotFood()
{
    bool colisaoY = playerY == foodY;
    bool colisaoX = playerX < foodX + foods[food].Length && playerX + player.Length > foodX;
    
    return colisaoY && colisaoX;
}

bool PlayerIsSick()
{
    return player.Equals(states[2]);
}

bool PlayerIsFaster()
{
    return player.Equals(states[1]);
}

void ChangePlayer()
{
    player = states[food];
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);
}

void FreezePlayer()
{
    System.Threading.Thread.Sleep(1000);
    player = states[0];
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);
}

void Move(int speed = 1, bool otherKeysExit = false)
{
    int lastX = playerX;
    int lastY = playerY;

    switch (Console.ReadKey(true).Key)
    {
        case ConsoleKey.UpArrow:
            playerY -= speed; 
            break;
        case ConsoleKey.DownArrow:
            playerY += speed; 
            break;
        case ConsoleKey.LeftArrow:
            playerX -= speed;
            break;
        case ConsoleKey.RightArrow:
            playerX += speed;
            break;
        case ConsoleKey.Escape:
            shouldExit = true;
            break;
        default:
            shouldExit = otherKeysExit;
            break;
    }
    Console.SetCursorPosition(lastX, lastY);
    for (int i = 0; i < player.Length; i++)
    {
        Console.Write(" ");
    }

    playerX = (playerX < 0) ? 0 : (playerX >= width ? width : playerX);
    playerY = (playerY < 0) ? 0 : (playerY >= height ? height : playerY);

    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);
}

void InitializeGame()
{
    Console.Clear();
    ShowFood();
    Console.SetCursorPosition(0, 0);
    Console.Write(player);
}
*/
// Versão 2 - 
using System;
using System.Collections.Generic;
using System.Threading;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.CursorVisible  = false;

const int HUD_ROWS   = 2;
const int MIN_WIDTH  = 42;
const int MIN_HEIGHT = 22;

var rng      = new Random();
bool running = true;
int  level   = 1;
int  score   = 0;
int  lives   = 3;
int  hiScore = 0;

int fieldW, fieldH;
int px = 2, py = 2;
int prevPx = 2, prevPy = 2;   // ← posição anterior do jogador
int pdx = 1, pdy = 0;
string playerChar = ">";
bool   powered    = false;
int    powerTicks = 0;

bool[,] walls = null!;

var foods  = new List<Food>();
int totalFoods    = 0;
int collectedFood = 0;

string[]       foodChars  = { "·", "●", "★" };
int[]          foodScores = { 10, 30, 100 };
ConsoleColor[] foodColors = { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.Cyan };

var ghosts = new List<Ghost>();
ConsoleColor[] ghostColors = { ConsoleColor.Red, ConsoleColor.Magenta, ConsoleColor.DarkYellow };
string ghostChar  = "ʘ";
string scaredChar = "ᵒ";

const int BASE_MS = 120;

if (Console.WindowWidth < MIN_WIDTH || Console.WindowHeight < MIN_HEIGHT)
{
    Console.WriteLine($"Redimensione o terminal para pelo menos {MIN_WIDTH}x{MIN_HEIGHT} e execute novamente.");
    return;
}

ShowTitle();
while (running)
{
    StartLevel();
    bool levelClear = RunLevel();

    if (!levelClear)
    {
        lives--;
        if (lives <= 0)
        {
            GameOver();
            if (!AskPlayAgain()) break;
            lives = 3;
            score = 0;
            level = 1;
        }
        else
        {
            ShowDeathScreen();
        }
    }
    else
    {
        score += level * 500;
        level++;
        ShowLevelClear();
    }
}

Console.Clear();
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("\nScore final: " + score);
Console.ResetColor();

void ShowTitle()
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine(@"
  ██████╗  █████╗  ██████╗███╗   ███╗ █████╗ ███╗   ██╗
  ██╔══██╗██╔══██╗██╔════╝████╗ ████║██╔══██╗████╗  ██║
  ██████╔╝███████║██║     ██╔████╔██║███████║██╔██╗ ██║
  ██╔═══╝ ██╔══██║██║     ██║╚██╔╝██║██╔══██║██║╚██╗██║
  ██║     ██║  ██║╚██████╗██║ ╚═╝ ██║██║  ██║██║ ╚████║
  ╚═╝     ╚═╝  ╚═╝ ╚═════╝╚═╝     ╚═╝╚═╝  ╚═╝╚═╝  ╚═══╝");
    Console.ResetColor();
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("\nSetas: mover   ESC: sair\n");
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("  · = 10pts   ● = 30pts   ★ = 100pts (poder!)");
    Console.WriteLine("\nPressione qualquer tecla para começar...");
    Console.ResetColor();
    Console.ReadKey(true);
}

void ShowLevelClear()
{
    DrawField();
    DrawHUD();
    int midY = HUD_ROWS + fieldH / 2 - 1;
    int midX = fieldW / 2 - 10;
    WriteAt(midX, midY + 1, $"FASE {level - 1} COMPLETA!", ConsoleColor.Green);
    WriteAt(midX, midY + 2, $"+{(level-1)*500} bônus pts", ConsoleColor.Yellow);
    Thread.Sleep(2200);
}

void ShowDeathScreen()
{
    DrawField();
    DrawHUD();
    int midY = HUD_ROWS + fieldH / 2 - 1;
    int midX = fieldW / 2 - 11;
    WriteAt(midX, midY + 1, "VOCÊ MORREU!", ConsoleColor.Red);
    WriteAt(midX, midY + 2, $"Vidas: {lives - 1} restante(s)", ConsoleColor.Yellow);
    Thread.Sleep(1800);
}

void GameOver()
{
    if (score > hiScore) hiScore = score;
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(@"
  ██████╗  █████╗ ███╗   ███╗███████╗     ██████╗ ██╗   ██╗███████╗██████╗
  ██╔════╝██╔══██╗████╗ ████║██╔════╝    ██╔═══██╗██║   ██║██╔════╝██╔══██╗
  ██║  ███╗███████║██╔████╔██║█████╗      ██║   ██║██║   ██║█████╗  ██████╔╝
  ██║   ██║██╔══██║██║╚██╔╝██║██╔══╝      ██║   ██║╚██╗ ██╔╝██╔══╝  ██╔══██╗
  ╚██████╔╝██║  ██║██║ ╚═╝ ██║███████╗    ╚██████╔╝ ╚████╔╝ ███████╗██║  ██║
   ╚═════╝ ╚═╝  ╚═╝╚═╝     ╚═╝╚══════╝     ╚═════╝   ╚═══╝  ╚══════╝╚═╝  ╚═╝");
    Console.ResetColor();
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"\n  Score: {score}   Hi-Score: {hiScore}   Nível: {level}");
    Console.ResetColor();
}

bool AskPlayAgain()
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write("\n  Jogar novamente? (S/N): ");
    Console.ResetColor();
    Console.CursorVisible = true;
    string? r = Console.ReadLine()?.Trim().ToUpper();
    Console.CursorVisible = false;
    return r is "S" or "SIM";
}

void StartLevel()
{
    fieldW = Console.WindowWidth  - 1;
    fieldH = Console.WindowHeight - HUD_ROWS - 1;

    powered       = false;
    powerTicks    = 0;
    collectedFood = 0;
    foods.Clear();
    ghosts.Clear();

    BuildMaze();
    PlaceFoods();
    PlacePlayer();
    PlaceGhosts();

    Console.Clear();
    DrawField();
    DrawHUD();
}

void BuildMaze()
{
    walls = new bool[fieldW, fieldH];

    for (int x = 0; x < fieldW; x++)
    {
        walls[x, 0]        = true;
        walls[x, fieldH-1] = true;
    }
    for (int y = 0; y < fieldH; y++)
    {
        walls[0, y]        = true;
        walls[fieldW-1, y] = true;
    }

    int blockCount = 2 + (level % 3);
    int spacing    = fieldW / (blockCount + 1);
    for (int b = 1; b <= blockCount; b++)
    {
        int bx  = b * spacing;
        int len = fieldH / 3;
        int by  = (b % 2 == 0) ? 2 : fieldH / 2;
        for (int dy = 0; dy < len; dy++)
        {
            int wy = by + dy;
            if (wy > 0 && wy < fieldH - 1 && bx > 0 && bx < fieldW - 1)
                walls[bx, wy] = true;
        }
    }

    int hSpacing = fieldH / 3;
    for (int row = 1; row <= 2; row++)
    {
        int hy  = row * hSpacing;
        int len = fieldW / 4;
        int hx  = (row % 2 == 0) ? fieldW / 4 : fieldW * 3 / 4 - len;
        for (int dx = 0; dx < len; dx++)
        {
            int wx = hx + dx;
            if (wx > 0 && wx < fieldW - 1 && hy > 0 && hy < fieldH - 1)
                walls[wx, hy] = true;
        }
    }
}

void PlacePlayer()
{
    px = 2; py = 2;
    prevPx = px; prevPy = py;
    pdx = 1; pdy = 0;
    playerChar = ">";
    while (walls[px, py]) px++;
    prevPx = px;
}

void PlaceFoods()
{
    int count    = 15 + level * 3;
    totalFoods   = 0;
    int attempts = 0;
    while (foods.Count < count && attempts < 2000)
    {
        attempts++;
        int fx = rng.Next(1, fieldW - 1);
        int fy = rng.Next(1, fieldH - 1);
        if (walls[fx, fy]) continue;
        if (foods.Exists(f => f.X == fx && f.Y == fy)) continue;
        if (Math.Abs(fx - px) < 4 && Math.Abs(fy - py) < 2) continue;

        int type = rng.Next(100) switch { < 80 => 0, < 95 => 1, _ => 2 };
        foods.Add(new Food(fx, fy, type));
        totalFoods++;
    }
}

void PlaceGhosts()
{
    int ghostCount = Math.Min(1 + (level - 1) / 2, ghostColors.Length);
    for (int i = 0; i < ghostCount; i++)
    {
        var g = new Ghost(i);
        g.X         = fieldW - 3;
        g.Y         = fieldH - 3;
        g.PrevX     = g.X;
        g.PrevY     = g.Y;
        g.MoveDelay = Math.Max(1, 3 - level / 3);
        g.MoveDelay = Math.Max(1, g.MoveDelay + (i == 0 ? 0 : i));
        ghosts.Add(g);
    }
}

bool RunLevel()
{
    int tick = 0;

    while (true)
    {
        if (Console.KeyAvailable)
        {
            var key = Console.ReadKey(true).Key;
            ProcessInput(key);
            if (!running) return false;
        }
        prevPx = px;
        prevPy = py;
        MovePlayer();

        foreach (var g in ghosts)
        {
            g.TickCount++;
            if (g.TickCount >= g.MoveDelay)
            {
                g.TickCount = 0;
                g.PrevX = g.X;
                g.PrevY = g.Y;
                MoveGhost(g, tick);
            }
        }

        var eaten = foods.Find(f => f.X == px && f.Y == py);
        if (eaten != null)
        {
            score += foodScores[eaten.Type];
            foods.Remove(eaten);
            collectedFood++;

            if (eaten.Type == 2)
            {
                powered    = true;
                powerTicks = 30 + level * 5;
                FlashMessage("  ★ PODER! Coma os fantasmas! ★", ConsoleColor.Cyan, 600);
            }
        }

        if (powered)
        {
            powerTicks--;
            if (powerTicks <= 0) powered = false;
        }

        foreach (var g in ghosts)
        {
            if (g.X == px && g.Y == py)
            {
                if (powered)
                {
                    score += 200;
                    EraseCell(g.X, g.Y);
                    g.X     = fieldW - 3;
                    g.Y     = fieldH - 3;
                    g.PrevX = g.X;
                    g.PrevY = g.Y;
                    FlashMessage("  +200! Fantasma eliminado!", ConsoleColor.Green, 400);
                }
                else
                {
                    DrawPlayer(ConsoleColor.DarkGray);
                    Thread.Sleep(400);
                    return false;
                }
            }
        }

        if (foods.Count == 0) return true;

        DrawAll();

        tick++;
        Thread.Sleep(BASE_MS);
    }
}

void ProcessInput(ConsoleKey key)
{
    switch (key)
    {
        case ConsoleKey.UpArrow:    pdx = 0;  pdy = -1; break;
        case ConsoleKey.DownArrow:  pdx = 0;  pdy =  1; break;
        case ConsoleKey.LeftArrow:  pdx = -1; pdy =  0; break;
        case ConsoleKey.RightArrow: pdx =  1; pdy =  0; break;
        case ConsoleKey.Escape:     running = false;     break;
    }
    playerChar = (pdx, pdy) switch
    {
        ( 1,  0) => ">",
        (-1,  0) => "<",
        ( 0, -1) => "^",
        ( 0,  1) => "v",
        _        => playerChar
    };
}

void MovePlayer()
{
    int nx = px + pdx;
    int ny = py + pdy;
    nx = (nx <= 0) ? fieldW - 2 : (nx >= fieldW - 1 ? 1 : nx);
    ny = (ny <= 0) ? fieldH - 2 : (ny >= fieldH - 1 ? 1 : ny);
    if (!walls[nx, ny]) { px = nx; py = ny; }
}

void MoveGhost(Ghost g, int tick)
{
    int targetX, targetY;
    if (powered)
    {
        targetX = (px < fieldW / 2) ? fieldW - 2 : 1;
        targetY = (py < fieldH / 2) ? fieldH - 2 : 1;
    }
    else
    {
        targetX = px;
        targetY = py;
    }

    bool chase = rng.Next(10) < (powered ? 3 : 7);

    int dx, dy;
    if (chase)
    {
        int diffX = targetX - g.X;
        int diffY = targetY - g.Y;

        if (Math.Abs(diffX) >= Math.Abs(diffY))
        { dx = Math.Sign(diffX); dy = 0; }
        else
        { dx = 0; dy = Math.Sign(diffY); }
    }
    else
    {
        int[] dirs = { -1, 0, 1 };
        dx = dirs[rng.Next(3)];
        dy = (dx == 0) ? (rng.Next(2) == 0 ? -1 : 1) : 0;
    }

    int nx = g.X + dx;
    int ny = g.Y + dy;

    if (IsValidGhostPos(nx, ny))
    {
        g.X = nx; g.Y = ny;
        g.Dx = dx; g.Dy = dy;
    }
    else
    {
        (dx, dy) = (dy, dx);
        nx = g.X + dx; ny = g.Y + dy;
        if (IsValidGhostPos(nx, ny)) { g.X = nx; g.Y = ny; g.Dx = dx; g.Dy = dy; }
        else
        {
            nx = g.X - g.Dx; ny = g.Y - g.Dy;
            if (IsValidGhostPos(nx, ny)) { g.X = nx; g.Y = ny; }
        }
    }
}

bool IsValidGhostPos(int x, int y) =>
    x > 0 && x < fieldW - 1 && y > 0 && y < fieldH - 1 && !walls[x, y];

void EraseCell(int x, int y)
{
    var food = foods.Find(f => f.X == x && f.Y == y);
    if (food != null)
    {
        Console.SetCursorPosition(x, HUD_ROWS + y);
        Console.ForegroundColor = foodColors[food.Type];
        Console.Write(foodChars[food.Type]);
        Console.ResetColor();
    }
    else
    {
        Console.SetCursorPosition(x, HUD_ROWS + y);
        Console.Write(" ");
    }
}

void DrawAll()
{
    EraseCell(prevPx, prevPy);

    foreach (var g in ghosts)
        if (g.PrevX != g.X || g.PrevY != g.Y)
            EraseCell(g.PrevX, g.PrevY);

    DrawPlayer(powered ? ConsoleColor.Cyan : ConsoleColor.Yellow);

    foreach (var g in ghosts)
        DrawGhost(g);

    DrawHUD();
}

void DrawField()
{
    for (int y = 0; y < fieldH; y++)
    for (int x = 0; x < fieldW; x++)
    {
        Console.SetCursorPosition(x, HUD_ROWS + y);
        if (walls[x, y])
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("█");
        }
        else
        {
            Console.Write(" ");
        }
    }
    Console.ResetColor();
    foreach (var f in foods)
    {
        Console.SetCursorPosition(f.X, HUD_ROWS + f.Y);
        Console.ForegroundColor = foodColors[f.Type];
        Console.Write(foodChars[f.Type]);
    }
    Console.ResetColor();
}

void DrawPlayer(ConsoleColor color)
{
    Console.SetCursorPosition(px, HUD_ROWS + py);
    Console.ForegroundColor = color;
    Console.Write(playerChar);
    Console.ResetColor();
}

void DrawGhost(Ghost g)
{
    Console.SetCursorPosition(g.X, HUD_ROWS + g.Y);
    if (powered)
    {
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.Write(scaredChar);
    }
    else
    {
        Console.ForegroundColor = ghostColors[g.Id % ghostColors.Length];
        Console.Write(ghostChar);
    }
    Console.ResetColor();
}

void DrawHUD()
{
    Console.SetCursorPosition(0, 0);
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.Write(new string('─', Console.WindowWidth - 1));

    Console.SetCursorPosition(0, 0);
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write($" SCORE:{score,6}");
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.Write("  │  ");
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write($"HI:{hiScore,6}");
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.Write("  │  ");
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write($"NÍVEL:{level}");
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.Write("  │  ");

    for (int i = 0; i < 3; i++)
    {
        Console.ForegroundColor = i < lives ? ConsoleColor.Red : ConsoleColor.DarkGray;
        Console.Write("♥ ");
    }

    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.Write("  │  ");
    int barLen = 12;
    int filled = totalFoods > 0 ? (int)((double)collectedFood / totalFoods * barLen) : 0;
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("[");
    Console.Write(new string('█', filled));
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.Write(new string('░', barLen - filled));
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("]");
    Console.ResetColor();
    Console.SetCursorPosition(0, 1);
    Console.Write(new string(' ', Console.WindowWidth - 1));
    Console.SetCursorPosition(0, 1);
    if (powered)
    {
        int totalPower = 30 + level * 5;
        int pct = (int)((double)powerTicks / totalPower * 20);
        pct = Math.Max(0, Math.Min(20, pct));
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($" ★ PODER [{new string('█', pct)}{new string('░', 20 - pct)}]");
    }
    Console.ResetColor();
}

void WriteAt(int x, int y, string text, ConsoleColor color)
{
    if (x < 0 || y < 0 || y >= Console.WindowHeight || x >= Console.WindowWidth) return;
    Console.SetCursorPosition(Math.Max(0, x), Math.Max(0, y));
    Console.ForegroundColor = color;
    Console.Write(text);
    Console.ResetColor();
}

void EraseAt(int x, int y)
{
    Console.SetCursorPosition(x, HUD_ROWS + y);
    Console.Write(" ");
}

void FlashMessage(string msg, ConsoleColor color, int ms)
{
    int y = 1;
    Console.SetCursorPosition(0, y);
    Console.ForegroundColor = color;
    Console.Write(msg.PadRight(Console.WindowWidth - 1));
    Console.ResetColor();
    Thread.Sleep(ms);
    Console.SetCursorPosition(0, y);
    Console.Write(new string(' ', Console.WindowWidth - 1));
}

record Ghost(int Id)
{
    public int  X         { get; set; }
    public int  Y         { get; set; }
    public int  PrevX     { get; set; }  
    public int  PrevY     { get; set; }  
    public int  Dx        { get; set; } = 1;
    public int  Dy        { get; set; } = 0;
    public int  MoveDelay { get; set; } = 2;
    public int  TickCount { get; set; } = 0;
    public bool Alive     { get; set; } = true;
}

record Food(int X, int Y, int Type);