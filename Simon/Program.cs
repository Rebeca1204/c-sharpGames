using System;
/* Implementation of a console game like simon, for repetition of patterns */
Random random = new Random();
string[] options = {
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
int score = 0;


    
InitializeGame();
do {
     // Making a pattern bigger by 1 everytime and showing
    int[] pattern = new int[score + 1];
    for (int i = 0; i < pattern.Length; i++){
        pattern[i] = random.Next(1,5);
        Console.WriteLine(options[pattern[i]]);
        Thread.Sleep(1000);
        // Pause between
        Console.Clear();
        Thread.Sleep(100);
    }

    // Checking player input
    int correct = 0;
    for (int i = 0; i < pattern.Length; i++){
        int play = 0;
        switch (Console.ReadKey(true).Key){
        case ConsoleKey.UpArrow:
            Console.WriteLine(options[1]);
            play = 1;
            break;
        case ConsoleKey.DownArrow:
            Console.WriteLine(options[3]);
            play = 3;
            break;
        case ConsoleKey.LeftArrow:
            Console.WriteLine(options[4]);
            play = 4;
            break;
        case ConsoleKey.RightArrow:
            Console.WriteLine(options[2]);
            play = 2;
            break;
        default:
            break;
        }

        // Exit if any other keys are pressed
        if (play == 0) break;

        // Checking the pattern
        if (play == pattern[i]){
            correct++;
            Thread.Sleep(1000);
            Console.Clear();
        }else{
            Console.WriteLine("Wrong pattern! You lose!");
            break;
        }
    }
    // If the whole pattern is correct
    if (correct == score + 1){
        // Increments score and size of next pattern
        score++;
        Console.WriteLine("Correct! Next level!");
        Thread.Sleep(1000);
        Console.Clear();
    }else break;
}while(true);

// Showing score
Console.WriteLine($"Score: {score}");
   
void InitializeGame(){
    Console.WriteLine(options[0]);
    Thread.Sleep(1000);
    Console.Clear();
}

