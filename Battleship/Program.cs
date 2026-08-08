// Versão 1 
/*
using System;
using System.Collections.Generic;
using System.Threading;

namespace BattleshipConsole
{
    public enum CellState
    {
        Empty,  // Água não revelada
        Ship,   // Navio (escondido)
        Hit,    // Navio atingido (O)
        Miss    // Tiro na água (X)
    }

    public class Board
    {
        public const int Size = 5;
        private readonly CellState[,] _grid = new CellState[Size, Size];
        private readonly bool _isComputer;

        public Board(bool isComputer)
        {
            _isComputer = isComputer;
        }

        public bool PlaceShip(int row, int col)
        {
            if (_grid[row, col] == CellState.Ship) return false;
            _grid[row, col] = CellState.Ship;
            return true;
        }

        public CellState GetCellState(int row, int col) => _grid[row, col];

        public bool ReceiveShot(int row, int col)
        {
            if (_grid[row, col] == CellState.Ship)
            {
                _grid[row, col] = CellState.Hit;
                return true;
            }
            
            _grid[row, col] = CellState.Miss;
            return false;
        }

        public void Print()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    CellState state = _grid[i, j];
                    int cellNumber = i * Size + (j + 1);

                    switch (state)
                    {
                        case CellState.Hit:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("O  ");
                            break;

                        case CellState.Miss:
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("X  ");
                            break;

                        case CellState.Ship:
                            if (_isComputer)
                            {
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.Write($"{cellNumber,-3}");
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write("N  ");
                            }
                            break;

                        case CellState.Empty:
                        default:
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.Write($"{cellNumber,-3}");
                            break;
                    }
                }
                Console.WriteLine();
            }
            Console.ResetColor();
        }
    }

    public class BattleshipGame
    {
        private const int TotalShips = 5;
        private readonly Board _computerBoard = new Board(isComputer: true);
        private readonly Board _playerBoard = new Board(isComputer: false);
        private readonly List<int> _computerAvailableShots = new List<int>();
        private readonly Random _random = new Random();

        private int _playerSinked = 0;
        private int _computerSinked = 0;

        public void Start()
        {
            InitializeAvailableShots();
            SetupComputerShips();
            SetupPlayerShips();

            for (int turn = 0; turn < Board.Size * Board.Size; turn++)
            {
                PlayerTurn();
                if (_playerSinked == TotalShips)
                {
                    EndGame(true);
                    return;
                }

                ComputerTurn();
                if (_computerSinked == TotalShips)
                {
                    EndGame(false);
                    return;
                }
            }
        }

        private void InitializeAvailableShots()
        {
            for (int i = 1; i <= Board.Size * Board.Size; i++)
                _computerAvailableShots.Add(i);
        }

        private void SetupComputerShips()
        {
            int placed = 0;
            while (placed < TotalShips)
            {
                int rand = _random.Next(1, 26);
                int row = (rand - 1) / Board.Size;
                int col = (rand - 1) % Board.Size;

                if (_computerBoard.PlaceShip(row, col)) placed++;
            }
        }

        private void SetupPlayerShips()
        {
            for (int i = 0; i < TotalShips; i++)
            {
                Console.Clear();
                Console.WriteLine("POSICIONE OS SEUS NAVIOS");
                _playerBoard.Print();

                Console.WriteLine($"\nEscolha a posição para o seu navio {i + 1} (1-25):");
                int play;
                bool ok = int.TryParse(Console.ReadLine(), out play);
                int row = (play - 1) / Board.Size;
                int col = (play - 1) % Board.Size;

                while (!ok || play < 1 || play > 25 || _playerBoard.GetCellState(row, col) == CellState.Ship)
                {
                    Console.WriteLine("Posição inválida ou já ocupada! Escolha outra:");
                    ok = int.TryParse(Console.ReadLine(), out play);
                    row = (play - 1) / Board.Size;
                    col = (play - 1) % Board.Size;
                }

                _playerBoard.PlaceShip(row, col);
            }
        }

        private void PlayerTurn()
        {
            Console.Clear();
            Console.WriteLine("=== O SEU TURNO ===");
            Console.WriteLine("Tabuleiro do Computador (Os seus tiros):");
            _computerBoard.Print();

            Console.WriteLine($"\nEscolha uma posição para atirar (1-25):");
            int play;
            bool ok = int.TryParse(Console.ReadLine(), out play);
            int row = (play - 1) / Board.Size;
            int col = (play - 1) % Board.Size;

            while (!ok || play < 1 || play > 25 || 
                   _computerBoard.GetCellState(row, col) == CellState.Hit || 
                   _computerBoard.GetCellState(row, col) == CellState.Miss)
            {
                Console.WriteLine("Posição inválida ou já atingida! Escolha outra:");
                ok = int.TryParse(Console.ReadLine(), out play);
                row = (play - 1) / Board.Size;
                col = (play - 1) % Board.Size;
            }

            Console.Clear();
            if (_computerBoard.ReceiveShot(row, col))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Acertou num navio!");
                _playerSinked++;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Tiro na água!");
            }
            Console.ResetColor();

            _computerBoard.Print();
            Thread.Sleep(3000);
        }

        private void ComputerTurn()
        {
            Console.Clear();
            Console.WriteLine("TURNO DO COMPUTADOR");

            int index = _random.Next(_computerAvailableShots.Count);
            int compPlay = _computerAvailableShots[index];
            _computerAvailableShots.RemoveAt(index);

            int row = (compPlay - 1) / Board.Size;
            int col = (compPlay - 1) % Board.Size;

            if (_playerBoard.ReceiveShot(row, col))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"O computador atirou na posição {compPlay} e acertou no seu navio!");
                _computerSinked++;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\nO computador atirou na posição {compPlay} e falhou!");
            }
            Console.ResetColor();

            Console.WriteLine("\nO Seu Tabuleiro Atual:");
            _playerBoard.Print();
            Thread.Sleep(3000);
        }

        private void EndGame(bool playerWon)
        {
            Console.Clear();
            if (playerWon)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Parabéns! Afundou todos os navios do computador e venceu!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("O computador afundou todos os seus navios! Perdeu!");
            }
            Console.ResetColor();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            BattleshipGame game = new BattleshipGame();
            game.Start();
        }
    }
}*/

//Versão 2 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BattleshipConsole
{

    public enum CellState { Empty, Ship, Hit, Miss }
    public enum Orientation { Horizontal, Vertical }

    public class Ship
    {
        public string Name    { get; }
        public int    Size    { get; }
        public int    HitCount { get; private set; }
        public bool   IsSunk  => HitCount >= Size;

        public Ship(string name, int size) { Name = name; Size = size; }
        public void RegisterHit() => HitCount++;
    }

    public class Board
    {
        public const int Size = 10;

        private readonly CellState[,] _grid     = new CellState[Size, Size];
        private readonly Ship[,]      _shipGrid = new Ship[Size, Size]; // qual navio ocupa cada célula
        private readonly bool         _hideShips;

        public Board(bool hideShips) => _hideShips = hideShips;

        public bool PlaceShip(Ship ship, int row, int col, Orientation orientation)
        {
            var cells = GetCells(ship.Size, row, col, orientation);
            if (cells == null) return false;
            if (cells.Any(c => _grid[c.r, c.c] == CellState.Ship)) return false;

            foreach (var (r, c) in cells)
            {
                _grid[r, c]     = CellState.Ship;
                _shipGrid[r, c] = ship;
            }
            return true;
        }

        public (bool hit, Ship? sunk) ReceiveShot(int row, int col)
        {
            if (_grid[row, col] == CellState.Ship)
            {
                _grid[row, col] = CellState.Hit;
                var ship = _shipGrid[row, col];
                ship.RegisterHit();
                return (true, ship.IsSunk ? ship : null);
            }
            _grid[row, col] = CellState.Miss;
            return (false, null);
        }

        public bool AlreadyShot(int row, int col)
        {
            var s = _grid[row, col];
            return s == CellState.Hit || s == CellState.Miss;
        }

        public CellState GetCell(int row, int col) => _grid[row, col];

        public void PrintLines(bool revealShips = false)
        {
            for (int i = 0; i < Size; i++)
            {
                Console.Write($" {(char)('A' + i)} │");
                for (int j = 0; j < Size; j++)
                    WriteCell(_grid[i, j], revealShips);
                Console.WriteLine();
            }
        }

        private void WriteCell(CellState state, bool reveal)
        {
            switch (state)
            {
                case CellState.Hit:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(" ● ");
                    break;
                case CellState.Miss:
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write(" · ");
                    break;
                case CellState.Ship when !_hideShips || reveal:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(" ■ ");
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(" ~ ");
                    break;
            }
            Console.ResetColor();
        }

        private static List<(int r, int c)>? GetCells(int size, int row, int col, Orientation ori)
        {
            var cells = new List<(int, int)>();
            for (int i = 0; i < size; i++)
            {
                int r = ori == Orientation.Vertical   ? row + i : row;
                int c = ori == Orientation.Horizontal ? col + i : col;
                if (r >= Size || c >= Size) return null;
                cells.Add((r, c));
            }
            return cells;
        }
    }

    public class AiController
    {
        private enum AiMode { Hunt, Target }

        private AiMode         _mode     = AiMode.Hunt;
        private (int r, int c) _firstHit;                        
        private (int r, int c) _lastHit;                      
        private Orientation?   _lockedOri;                        
        private bool           _reverseDir;                        

        private readonly bool[,]        _shot  = new bool[Board.Size, Board.Size];
        private readonly List<(int,int)> _hunt  = new();            // fila de Hunt (xadrez)
        private readonly Random          _rng   = new();

        public AiController()
        {
            for (int r = 0; r < Board.Size; r++)
                for (int c = (r % 2); c < Board.Size; c += 2)
                    _hunt.Add((r, c));

            for (int i = _hunt.Count - 1; i > 0; i--)
            {
                int j = _rng.Next(i + 1);
                (_hunt[i], _hunt[j]) = (_hunt[j], _hunt[i]);
            }
        }

        public (int row, int col) ChooseShot(Board playerBoard)
        {
            return _mode == AiMode.Hunt
                ? HuntShot()
                : TargetShot(playerBoard);
        }

        public void RegisterResult(int row, int col, bool hit, bool sunk)
        {
            _shot[row, col] = true;

            if (sunk)
            {
                _mode      = AiMode.Hunt;
                _lockedOri = null;
                _reverseDir = false;
                return;
            }

            if (hit)
            {
                if (_mode == AiMode.Hunt)
                {
                    _mode     = AiMode.Target;
                    _firstHit = (row, col);
                    _lockedOri = null;
                    _reverseDir = false;
                }
                _lastHit = (row, col);
            }
            else if (_mode == AiMode.Target && !_reverseDir)
            {
                _reverseDir = true;
                _lastHit    = _firstHit;
            }
        }

        private (int, int) HuntShot()
        {
            while (_hunt.Count > 0)
            {
                var (r, c) = _hunt[0];
                _hunt.RemoveAt(0);
                if (!_shot[r, c]) return (r, c);
            }
            return RandomUnshotCell();
        }

        private (int, int) TargetShot(Board board)
        {
            var candidates = GetTargetCandidates();

            foreach (var (r, c) in candidates)
            {
                if (r < 0 || r >= Board.Size || c < 0 || c >= Board.Size) continue;
                if (_shot[r, c]) continue;
                return (r, c);
            }

            _mode = AiMode.Hunt;
            return HuntShot();
        }

        private IEnumerable<(int, int)> GetTargetCandidates()
        {
            int lr = _lastHit.r, lc = _lastHit.c;
            int fr = _firstHit.r, fc = _firstHit.c;

            if (_lockedOri == null)
            {
                yield return (lr - 1, lc);
                yield return (lr + 1, lc);
                yield return (lr, lc - 1);
                yield return (lr, lc + 1);
            }
            else if (_lockedOri == Orientation.Vertical)
            {
                int dir = _reverseDir ? -1 : 1;
                yield return (lr + dir, lc);
                yield return (fr - dir, lc);
            }
            else
            {
                int dir = _reverseDir ? -1 : 1;
                yield return (lr, lc + dir);
                yield return (fr, fc - dir);
            }
        }

        public void LockOrientation(int prevR, int prevC, int newR, int newC)
        {
            if (_mode != AiMode.Target) return;
            if (_lockedOri != null) return;

            if (newR != prevR) _lockedOri = Orientation.Vertical;
            else               _lockedOri = Orientation.Horizontal;
        }

        private (int, int) RandomUnshotCell()
        {
            for (int r = 0; r < Board.Size; r++)
                for (int c = 0; c < Board.Size; c++)
                    if (!_shot[r, c]) return (r, c);
            return (0, 0);
        }
    }

    public class BattleshipGame
    {
        private static readonly (string name, int size)[] Fleet =
        {
            ("Porta-aviões",  5),
            ("Encouraçado",   4),
            ("Cruzador",      3),
            ("Submarino",     3),
            ("Patrulha",      2),
        };

        private readonly Board         _playerBoard   = new Board(hideShips: false);
        private readonly Board         _computerBoard = new Board(hideShips: true);
        private readonly List<Ship>    _playerFleet   = new();
        private readonly List<Ship>    _computerFleet = new();
        private readonly AiController  _ai            = new AiController();
        private readonly Random        _rng           = new Random();

        public void Start()
        {
            PrintBanner();
            Thread.Sleep(1000);

            PlaceComputerShips();
            PlacePlayerShips();
            GameLoop();
        }

        private void PlaceComputerShips()
        {
            foreach (var (name, size) in Fleet)
            {
                var ship = new Ship(name, size);
                _computerFleet.Add(ship);
                PlaceRandom(_computerBoard, ship);
            }
        }

        private void PlacePlayerShips()
        {
            int shipIndex = 0;
            foreach (var (name, size) in Fleet)
            {
                var ship = new Ship(name, size);
                _playerFleet.Add(ship);
                shipIndex++;

                while (true)
                {
                    Console.Clear();
                    PrintTitle($"POSICIONE SEUS NAVIOS  [{shipIndex}/{Fleet.Length}]");
                    PrintBothBoards(placingPhase: true);

                    Console.WriteLine($"\n  Navio: {name}  (tamanho {size})");
                    Console.Write("  Linha (A-J): ");
                    int row = ReadRow();

                    Console.Write("  Coluna (1-10): ");
                    int col = ReadCol();

                    Console.Write("  Orientação — H (horizontal) / V (vertical): ");
                    var ori = ReadOrientation();

                    if (_playerBoard.PlaceShip(ship, row, col, ori))
                        break;

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n  ✗ Posição inválida ou sobreposta! Tente novamente.");
                    Console.ResetColor();
                    Thread.Sleep(1500);
                }
            }
        }

        private void PlaceRandom(Board board, Ship ship)
        {
            while (true)
            {
                int row = _rng.Next(Board.Size);
                int col = _rng.Next(Board.Size);
                var ori = _rng.Next(2) == 0 ? Orientation.Horizontal : Orientation.Vertical;
                if (board.PlaceShip(ship, row, col, ori)) return;
            }
        }

        private void GameLoop()
        {
            while (true)
            {
                PlayerTurn();
                if (_computerFleet.All(s => s.IsSunk)) { EndGame(playerWon: true);  return; }

                ComputerTurn();
                if (_playerFleet.All(s => s.IsSunk))   { EndGame(playerWon: false); return; }
            }
        }

        private void PlayerTurn()
        {
            while (true)
            {
                Console.Clear();
                PrintTitle("SEU TURNO — escolha onde atirar");
                PrintBothBoards(placingPhase: false);
                PrintFleetStatus();

                Console.Write("\n  Linha (A-J): ");
                int row = ReadRow();
                Console.Write("  Coluna (1-10): ");
                int col = ReadCol();

                if (_computerBoard.AlreadyShot(row, col))
                {
                    Warn("Você já atirou aqui! Escolha outra posição.");
                    continue;
                }

                var (hit, sunk) = _computerBoard.ReceiveShot(row, col);
                Console.Clear();
                PrintBothBoards(placingPhase: false);

                if (sunk != null)
                {
                    Highlight(ConsoleColor.Green,
                        $" AFUNDOU o {sunk.Name} do computador!");
                }
                else if (hit)
                {
                    Highlight(ConsoleColor.Yellow, "Acertou em cheio!");
                }
                else
                {
                    Highlight(ConsoleColor.DarkCyan, "Água! Tiro errou.");
                }

                Thread.Sleep(2000);
                return;
            }
        }

        private void ComputerTurn()
        {
            Console.Clear();
            PrintTitle("TURNO DO COMPUTADOR");

            var (row, col) = _ai.ChooseShot(_playerBoard);
            int prevR = row, prevC = col;

            var (hit, sunk) = _playerBoard.ReceiveShot(row, col);

            if (hit && sunk == null)
                _ai.LockOrientation(prevR, prevC, row, col);

            _ai.RegisterResult(row, col, hit, sunk != null);

            PrintBothBoards(placingPhase: false);

            Console.Write($"\n  Computador atirou em {RowLabel(row)}{col + 1}... ");
            if (sunk != null)
                Highlight(ConsoleColor.Red, $"AFUNDOU seu {sunk.Name}!");
            else if (hit)
                Highlight(ConsoleColor.Red, "ACERTOU em um dos seus navios!");
            else
                Highlight(ConsoleColor.DarkCyan, "Errou! Tiro na água.");

            Thread.Sleep(2500);
        }

        private void PrintBothBoards(bool placingPhase)
        {
            Console.WriteLine();
            Console.WriteLine($"  {"SEU TABULEIRO", -38} TABULEIRO DO INIMIGO");
            Console.WriteLine();

            string col = "   1  2  3  4  5  6  7  8  9  10";

            Console.WriteLine($"  {col, -38} {col}");

            Console.WriteLine();

            for (int i = 0; i < Board.Size; i++)
            {
                Console.Write($" {(char)('A' + i)} │");
                for (int j = 0; j < Board.Size; j++)
                    WriteCellInline(_playerBoard.GetCell(i, j), reveal: true);

                Console.Write("      ");

                Console.Write($" {(char)('A' + i)} │");
                for (int j = 0; j < Board.Size; j++)
                    WriteCellInline(_computerBoard.GetCell(i, j), reveal: false);

                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private static void WriteCellInline(CellState state, bool reveal)
        {
            switch (state)
            {
                case CellState.Hit:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(" X ");
                    break;
                case CellState.Miss:
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write(" o ");
                    break;
                case CellState.Ship when reveal:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(" ■ ");
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(" ~ ");
                    break;
            }
            Console.ResetColor();
        }

        private void PrintFleetStatus()
        {
            Console.WriteLine("  ─────────────────────────────────────────────────────────");
            Console.Write("  Sua frota:       ");
            foreach (var s in _playerFleet)
            {
                Console.ForegroundColor = s.IsSunk ? ConsoleColor.DarkRed : ConsoleColor.Green;
                Console.Write($"[{s.Name[..3]}] ");
            }
            Console.ResetColor();
            Console.WriteLine();

            Console.Write("  Frota inimiga:   ");
            foreach (var s in _computerFleet)
            {
                Console.ForegroundColor = s.IsSunk ? ConsoleColor.DarkRed : ConsoleColor.DarkGray;
                Console.Write($"[{s.Name[..3]}] ");
            }
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("  ─────────────────────────────────────────────────────────");
        }

        private void EndGame(bool playerWon)
        {
            Console.Clear();
            if (playerWon)
            {
                PrintBothBoards(placingPhase: false);
                Highlight(ConsoleColor.Green,
                    "\n VITÓRIA! Você afundou toda a frota inimiga!");
            }
            else
            {
                Console.WriteLine("\n  Os navios do computador estavam em:");
                for (int i = 0; i < Board.Size; i++)
                {
                    Console.Write($" {(char)('A' + i)} │");
                    for (int j = 0; j < Board.Size; j++)
                    {
                        var state = _computerBoard.GetCell(i, j);
                        WriteCellInline(state, reveal: true);
                    }
                    Console.WriteLine();
                }
                Highlight(ConsoleColor.Red,
                    "\n DERROTA! O computador afundou toda a sua frota.");
            }
        }

        private static int ReadRow()
        {
            while (true)
            {
                string? input = Console.ReadLine()?.Trim().ToUpper();
                if (!string.IsNullOrEmpty(input) && input.Length == 1)
                {
                    int r = input[0] - 'A';
                    if (r >= 0 && r < Board.Size) return r;
                }
                Console.Write("  ✗ Linha inválida. Digite de A a J: ");
            }
        }

        private static int ReadCol()
        {
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int c) && c >= 1 && c <= Board.Size)
                    return c - 1;
                Console.Write("  ✗ Coluna inválida. Digite de 1 a 10: ");
            }
        }

        private static Orientation ReadOrientation()
        {
            while (true)
            {
                string? input = Console.ReadLine()?.Trim().ToUpper();
                if (input == "H") return Orientation.Horizontal;
                if (input == "V") return Orientation.Vertical;
                Console.Write("  ✗ Digite H ou V: ");
            }
        }

        private static void PrintBanner()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
  ██████╗  █████╗ ████████╗████████╗██╗     ███████╗███████╗██╗  ██╗██╗██████╗ 
  ██╔══██╗██╔══██╗╚══██╔══╝╚══██╔══╝██║     ██╔════╝██╔════╝██║  ██║██║██╔══██╗
  ██████╔╝███████║   ██║      ██║   ██║     █████╗  ███████╗███████║██║██████╔╝
  ██╔══██╗██╔══██║   ██║      ██║   ██║     ██╔══╝  ╚════██║██╔══██║██║██╔═══╝ 
  ██████╔╝██║  ██║   ██║      ██║   ███████╗███████╗███████║██║  ██║██║██║     
  ╚═════╝ ╚═╝  ╚═╝   ╚═╝      ╚═╝   ╚══════╝╚══════╝╚══════╝╚═╝  ╚═╝╚═╝╚═╝     
");
            Console.ResetColor();
            Console.WriteLine("  Afunde a frota inimiga antes que ela afunde a sua!\n");
        }

        private static void PrintTitle(string title)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n{title.ToUpper()}");
            Console.ResetColor();
        }

        private static void Warn(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n {msg}");
            Console.ResetColor();
            Thread.Sleep(1500);
        }

        private static void Highlight(ConsoleColor color, string msg)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ResetColor();
        }

        private static string RowLabel(int r) => ((char)('A' + r)).ToString();
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                var game = new BattleshipGame();
                game.Start();

                Console.WriteLine("\n  Jogar novamente? (S/N): ");
                string? resp = Console.ReadLine()?.Trim().ToUpper();
                if (resp != "S") break;
            }

            Console.WriteLine("\n  Obrigado por jogar!");
        }
    }
}