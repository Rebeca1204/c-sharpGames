/* Implementation of a console dice game where the winner of the most rounds of dice wins */
using System;
// Counting player wins
int pw = 0;
// Counting computer wins
int cw = 0;

Random random = new Random();
for (int i = 0; i < 10; i++){
    Console.Clear();
    // Player throws the dice
    int player = random.Next(1,7);
    Console.Write("Press any key to roll the dice...");
	Console.ReadKey(true);
    Console.WriteLine($"{player}");

    // Computer throws the dice
    int comp = random.Next(1,7);
    Console.WriteLine($"Computer rolled: {comp}");

    // The round winner is who got a higher number on the dice, gets a point 
    if (player > comp){
        Console.WriteLine("Player wins this round!");
        pw++;
    }else if (player < comp){
        Console.WriteLine("Computer wins this round!");
        cw++;
    }
    else{
        Console.WriteLine("Draw!");
    }
    Thread.Sleep(1000);
}
Console.Clear();
// The overall winner is who got a higher number of rounds won 
if (pw > cw)
    Console.WriteLine($"Player wins! ({pw} x {cw})");
else if (pw < cw)
    Console.WriteLine($"Computer wins! You lose! ({cw} x {pw})");
else
    Console.WriteLine($"Draw. Nobody wins! ({pw})");