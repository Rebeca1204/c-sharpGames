using System;
/* Implementation of a console game like rock, paper or scissors against the computer*/
Random random = new Random();
// Possible choices
string[] choices = {"rock", "paper", "scissors"};
// Scoreboard variables
int wins = 0, loses = 0, draws = 0;
int play;
do {
    Console.Clear();
    // Player's choice
    Console.WriteLine("\nChoose [r]ock, [p]aper or [s]cissors: ");
    Console.WriteLine("Choose any other key to finish: ");
    string choice = Console.ReadLine().Trim().ToLower();
    switch (choice[0]){
        case 'r':
            play = 0;
            break;
        case 'p':
            play = 1;
            break;
        case 's':
            play = 2;
            break;
        default:
            play = 3;
            break;
    }
    if (play == 3) break;

    // Computer's choice
    int comp = random.Next(0,3);
    Console.WriteLine($"Computer choose:  {choices[comp]}");

    // Verifying results
    if (play == comp){
        Console.WriteLine("Draw!");
        draws++;
    }
    else if ( play == 1 && comp == 0 || play == 2 && comp == 1 || play == 0 && comp == 2){
        Console.WriteLine($"You win! {choices[play]} wins {choices[comp]}");
        wins++;
    }
    else{
        Console.WriteLine($"You lose!{choices[comp]} wins {choices[play]}");
        loses++;
    }
    Thread.Sleep(2000);
}while(true);

Console.WriteLine($"Scoreboard: {wins} wins, {loses} loses and {draws} draws.");
