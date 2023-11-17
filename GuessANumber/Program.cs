using System;
// Implementation of a console game to guess a random number
bool ok;
int guessed;
Random random = new Random();
// Choosing random number
int value = random.Next(1,101);



Console.Clear();
do {
    // Asking for a guess
    Console.WriteLine("Guess a number: (1-100)");
    ok = int.TryParse(Console.ReadLine(), out guessed);
    if (!ok) 
        Console.WriteLine("Invalid");
    // Giving feedback on the guess (high or low)
    else if (guessed != value)
        Console.WriteLine($"Wrong. Value too {(guessed > value ? "High" : "Low")}");
} while (!ok || value != guessed);

Console.WriteLine("Correct guess!!");