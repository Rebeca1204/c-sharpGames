// Versão 1 - Jogo rápido de sorte 
// O jogador e o computador rolam dados em 10 rodadas e aquele que obtiver os maiores valores mais vezes é o vencedor
/*
using System;
using System.Threading;

int pw = 0;
int cw = 0;
Random random = new Random();

for (int i = 0; i < 10; i++)
{
    Console.Clear();
    Console.WriteLine($"--- RODADA {i + 1} DE 10 ---");
    Console.WriteLine($"Placar atual -> Jogador: {pw} | Computador: {cw}\n");

    Console.Write("Pressione qualquer tecla para rolar o dado...");
    Console.ReadKey(true); 
    Console.WriteLine("\n");

    int player = random.Next(1, 7);
    Console.WriteLine($"Você rolou: {player}");

    Thread.Sleep(500);

    int comp = random.Next(1, 7);
    Console.WriteLine($"Computador rolou: {comp}");
    Console.WriteLine();

    if (player > comp)
    {
        Console.WriteLine("Você venceu esta rodada!");
        pw++;
    }
    else if (player < comp)
    {
        Console.WriteLine("O Computador venceu esta rodada!");
        cw++;
    }
    else
    {
        Console.WriteLine("Empate nesta rodada!");
    }

    Console.WriteLine("\nPressione qualquer tecla para ir para a próxima rodada...");
    Console.ReadKey(true);
}

// Fim do jogo
Console.Clear();
Console.WriteLine("=== FIM DO JOGO ===");
Console.WriteLine($"Placar Final -> Jogador: {pw} x Computador: {cw}\n");

if (pw > cw)
    Console.WriteLine("Parabéns! Você é o grande vencedor!");
else if (pw < cw)
    Console.WriteLine("O Computador venceu o jogo. Mais sorte na próxima vez!");
else
    Console.WriteLine("O jogo terminou em um empate geral!");

Console.ReadLine();

*/
// Versão 2 - Simulador de cassino em texto
// Você gerencia seu saldo, define suas estratégias de aposta e usa itens especiais para tentar vencer a sorte e falir o computador

using System;
using System.Threading;

int rodadas = 5;
int dinheiro = 100;
int quantDados = 1;
bool usouSorte = false;

Console.Title = "SUPER CASINO DICE GAME";

Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("========================================");
Console.WriteLine("     BEM-VINDO AO SUPER CASINO DICE     ");
Console.WriteLine("========================================");
Console.ResetColor();

Console.Write("\nCom quantos dados você quer jogar? (1, 2 ou 3): ");
while (!int.TryParse(Console.ReadLine(), out quantDados) || quantDados < 1 || quantDados > 3)
{
    Console.Write("Entrada inválida. Escolha entre 1, 2 ou 3 dados: ");
}

Random random = new Random();

for (int i = 1; i <= rodadas; i++)
{
    if (dinheiro <= 0) break;

    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"=== RODADA {i} DE {rodadas} ===");
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"Seu Saldo Atual: ${dinheiro}");
    Console.ResetColor();
    Console.WriteLine("--------------------------------");

    int aposta = 0;
    Console.Write($"Quanto deseja apostar nesta rodada? (Disponível: $1 até ${dinheiro}): ");
    while (!int.TryParse(Console.ReadLine(), out aposta) || aposta < 1 || aposta > dinheiro)
    {
        Console.Write($"Valor inválido. Digite uma aposta entre 1 e {dinheiro}: ");
    }

    bool usarSorteRodada = false;
    if (!usouSorte)
    {
        Console.Write("Deseja usar seu item 'Sorte Grande' (+3 pontos no seu total)? (S/N): ");
        string resposta = Console.ReadLine().Trim().ToUpper();
        if (resposta == "S")
        {
            usarSorteRodada = true;
            usouSorte = true;
            Console.WriteLine("Sorte Grande Ativada! +3 pontos garantidos.");
        }
    }

    Console.Write("\nPressione qualquer tecla para rolar os dados...");
    Console.ReadKey(true);
    Console.WriteLine("\n");

    int totalJogador = 0;
    Console.Write("Você rolou: ");
    for (int d = 0; d < quantDados; d++)
    {
        int dado = random.Next(1, 7);
        totalJogador += dado;
        Console.Write($"[{dado}] ");
    }
    if (usarSorteRodada)
    {
        totalJogador += 3;
        Console.Write(" (+3 de Sorte)");
    }
    Console.WriteLine($"\n-> Seu Total: {totalJogador}");

    Console.Write("\nComputador rolando os dados");
    for (int p = 0; p < 3; p++) { Thread.Sleep(300); Console.Write("."); }
    Console.WriteLine("\n");

    int totalComputador = 0;
    Console.Write("Computador rolou: ");
    for (int d = 0; d < quantDados; d++)
    {
        int dado = random.Next(1, 7);
        totalComputador += dado;
        Console.Write($"[{dado}] ");
    }
    Console.WriteLine($"\n-> Total Computador: {totalComputador}\n");
    Console.WriteLine("--------------------------------");

    if (totalJogador > totalComputador)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Você venceu a rodada! Ganhou ${aposta}.");
        dinheiro += aposta;
    }
    else if (totalJogador < totalComputador)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"O Computador venceu a rodada. Você perdeu ${aposta}.");
        dinheiro -= aposta;
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Empate! Você recebe sua aposta de volta.");
    }
    Console.ResetColor();

    if (i < rodadas && dinheiro > 0)
    {
        Console.WriteLine("\nPressione qualquer tecla para a próxima rodada...");
        Console.ReadKey(true);
    }
}

Console.Clear();
Console.WriteLine("========================================");
Console.WriteLine("               FIM DE JOGO              ");
Console.WriteLine("========================================");

if (dinheiro > 100)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"Lucro total! Você saiu com ${dinheiro}! (Ganho de ${dinheiro - 100})");
}
else if (dinheiro <= 0)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Você faliu! O casino sempre ganha.");
}
else if (dinheiro < 100)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"Você terminou o jogo com ${dinheiro}. Teve um prejuízo de ${100 - dinheiro}.");
}
else
{
    Console.WriteLine("Você saiu exatamente com o que entrou: $100. Ok!");
}
Console.ResetColor();

Console.WriteLine("\nObrigado por jogar! Pressione Enter para fechar.");
Console.ReadLine();