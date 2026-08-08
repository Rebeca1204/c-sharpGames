using System;
using System.Collections.Generic;
using System.Threading;

namespace SimonSaysGame
{
    class Program
    {
        static ConsoleColor[] cores = {
            ConsoleColor.Gray,   
            ConsoleColor.Cyan,   
            ConsoleColor.Yellow, 
            ConsoleColor.Green,  
            ConsoleColor.Red      
        };

        static string[] options = {
            @"           ╔══════╗        " + '\n' +
            @"           ║      ║        " + '\n' +
            @"           ╚╗    ╔╝        " + '\n' +
            @"    ╔═══╗   ╚╗  ╔╝   ╔═══╗ " + '\n' +
            @"    ║   ╚═══╗╚══╝╔═══╝   ║ " + '\n' +
            @"    ║       ║    ║       ║ " + '\n' +
            @"    ║   ╔═══╝╔══╗╚═══╗   ║ " + '\n' +
            @"    ╚═══╝   ╔╝  ╚╗   ╚═══╝ " + '\n' +
            @"           ╔╝    ╚╗        " + '\n' +
            @"           ║      ║        " + '\n' +
            @"           ╚══════╝        ",
            @"           ╔══════╗        " + '\n' +
            @"           ║██████║        " + '\n' +
            @"           ╚╗████╔╝        " + '\n' +
            @"    ╔═══╗   ╚╗██╔╝   ╔═══╗ " + '\n' +
            @"    ║   ╚═══╗╚══╝╔═══╝   ║ " + '\n' +
            @"    ║       ║    ║       ║ " + '\n' +
            @"    ║   ╔═══╝╔══╗╚═══╗   ║ " + '\n' +
            @"    ╚═══╝   ╔╝  ╚╗   ╚═══╝ " + '\n' +
            @"           ╔╝    ╚╗        " + '\n' +
            @"           ║      ║        " + '\n' +
            @"           ╚══════╝        ",
            @"           ╔══════╗        " + '\n' +
            @"           ║      ║        " + '\n' +
            @"           ╚╗    ╔╝        " + '\n' +
            @"    ╔═══╗   ╚╗  ╔╝   ╔═══╗ " + '\n' +
            @"    ║   ╚═══╗╚══╝╔═══╝███║ " + '\n' +
            @"    ║       ║    ║███████║ " + '\n' +
            @"    ║   ╔═══╝╔══╗╚═══╗███║ " + '\n' +
            @"    ╚═══╝   ╔╝  ╚╗   ╚═══╝ " + '\n' +
            @"           ╔╝    ╚╗        " + '\n' +
            @"           ║      ║        " + '\n' +
            @"           ╚══════╝        ",
            @"           ╔══════╗        " + '\n' +
            @"           ║      ║        " + '\n' +
            @"           ╚╗    ╔╝        " + '\n' +
            @"    ╔═══╗   ╚╗  ╔╝   ╔═══╗ " + '\n' +
            @"    ║   ╚═══╗╚══╝╔═══╝   ║ " + '\n' +
            @"    ║       ║    ║       ║ " + '\n' +
            @"    ║   ╔═══╝╔══╗╚═══╗   ║ " + '\n' +
            @"    ╚═══╝   ╔╝██╚╗   ╚═══╝ " + '\n' +
            @"           ╔╝████╚╗        " + '\n' +
            @"           ║██████║        " + '\n' +
            @"           ╚══════╝        ",
            @"           ╔══════╗        " + '\n' +
            @"           ║      ║        " + '\n' +
            @"           ╚╗    ╔╝        " + '\n' +
            @"    ╔═══╗   ╚╗  ╔╝   ╔═══╗ " + '\n' +
            @"    ║███╚═══╗╚══╝╔═══╝   ║ " + '\n' +
            @"    ║███████║    ║       ║ " + '\n' +
            @"    ║███╔═══╝╔══╗╚═══╗   ║ " + '\n' +
            @"    ╚═══╝   ╔╝  ╚╗   ╚═══╝ " + '\n' +
            @"           ╔╝    ╚╗        " + '\n' +
            @"           ║      ║        " + '\n' +
            @"           ╚══════╝        ",
        };

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            Random random = new Random();
            bool querJogar = true;

            while (querJogar)
            {
                int score = 0;
                List<int> pattern = new List<int>();
                bool gameRunning = true;

                ExibirBannerInicial();

                while (gameRunning)
                {
                    pattern.Add(random.Next(1, 5));

                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"\n ╔═════════════════════════════════════╗");
                    Console.WriteLine($" ║     NÍVEL {score + 1} - PREPARE-SE!           ║");
                    Console.WriteLine($" ╚═════════════════════════════════════╝");
                    Thread.Sleep(1200);
                    Console.Clear();

                    foreach (int step in pattern)
                    {
                        Console.ForegroundColor = cores[step];
                        Console.WriteLine(options[step]);
                        Thread.Sleep(500); 
                        Console.Clear();
                        Thread.Sleep(100); 
                    }

                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("SUA VEZ! Use as SETAS do teclado:");
                    
                    for (int i = 0; i < pattern.Count; i++)
                    {
                        int play = 0;
                        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                        
                        switch (keyInfo.Key)
                        {
                            case ConsoleKey.UpArrow:    play = 1; break;
                            case ConsoleKey.RightArrow: play = 2; break;
                            case ConsoleKey.DownArrow:  play = 3; break;
                            case ConsoleKey.LeftArrow:  play = 4; break;
                            default:                    play = -1; break; 
                        }

                        if (play != pattern[i])
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(" ╔═════════════════════════════════════╗");
                            Console.WriteLine(" ║        ERROU! PADRÃO INCORRETO.     ║");
                            Console.WriteLine(" ╚═════════════════════════════════════╝");
                            gameRunning = false;
                            break;
                        }

                        Console.Clear();
                        Console.ForegroundColor = cores[play];
                        Console.WriteLine(options[play]);
                        Thread.Sleep(250); 
                        Console.Clear();
                    }

                    if (gameRunning)
                    {
                        score++;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Muito bem! Sequência correta.");
                        Thread.Sleep(1000);
                        Console.Clear();
                    }
                }

                // Tela de Game Over Estilizada
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n ╔═════════════════════════════════════╗");
                Console.WriteLine(" ║              GAME OVER              ║");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($" ║         PONTUAÇÃO FINAL: {score}          ║");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" ╚═════════════════════════════════════╝");
                
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\n Deseja jogar novamente? (Pressione ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("'n'");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" para sair ou qualquer outra tecla para continuar): ");
                
                ConsoleKeyInfo resposta = Console.ReadKey(true);
                
                if (resposta.KeyChar == 'n' || resposta.KeyChar == 'N')
                {
                    querJogar = false;
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n Obrigado por jogar Simon Says! Até a próxima!\n");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Thread.Sleep(2000);
                }
                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(" Reiniciando o jogo... Se prepare!");
                    Thread.Sleep(1500);
                    Console.Clear();
                }
            }
        }

        static void ExibirBannerInicial()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"  _____                           _____                     ");
            Console.WriteLine(@" / ____|                         / ____|                    ");
            Console.WriteLine(@"| (___  _   _ _ __ ___   ___  _ _| (___   __ _ _   _ ___    ");
            Console.WriteLine(@" \___ \| | | | '_ ` _ \ / _ \| '_ \___ \ / _` | | | / __|   ");
            Console.WriteLine(@" ____) | |_| | | | | | | (_) | | | |__) | (_| | |_| \__ \   ");
            Console.WriteLine(@"|_____/ \__, |_| |_| |_|\___/|_| |_|____/\__,_|\__, |___/   ");
            Console.WriteLine(@"         __/ |                                  __/ |       ");
            Console.WriteLine(@"        |___/                                  |___/       ");
            Console.WriteLine("\n");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(options[0]);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" >>> Pressione QUALQUER TECLA para começar o desafio... <<<");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.ReadKey(true);
            Console.Clear();
        }
    }
}