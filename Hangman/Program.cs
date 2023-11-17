using System;
/* Implementation of a console game like hangman */

string[] hangs = {
    @"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"          ║   " + '\n' +
	@"          ║   " + '\n' +
	@"     ███  ║   " + '\n' +
	@"    ══════╩═══", 
    @"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"          ║   " + '\n' +
	@"     ███  ║   " + '\n' +
	@"    ══════╩═══",
    @"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"      |   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"     ███  ║   " + '\n' +
	@"    ══════╩═══",
    @"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"      |\  ║   " + '\n' +
	@"          ║   " + '\n' +
	@"     ███  ║   " + '\n' +
	@"    ══════╩═══",
    @"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"     /|\  ║   " + '\n' +
	@"          ║   " + '\n' +
	@"     ███  ║   " + '\n' +
	@"    ══════╩═══",
    @"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"     /|\  ║   " + '\n' +
	@"       \  ║   " + '\n' +
	@"     ███  ║   " + '\n' +
	@"    ══════╩═══",
    @"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"     /|\  ║   " + '\n' +
	@"     / \  ║   " + '\n' +
	@"     ███  ║   " + '\n' +
	@"    ══════╩═══",};

string[] deathHangs = {@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"     /|\  ║   " + '\n' +
	@"     / \  ║   " + '\n' +
	@"     ███  ║   " + '\n' +
	@"    ══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"     /|\  ║   " + '\n' +
	@"     / \  ║   " + '\n' +
	@"          ║   " + '\n' +
	@"    ══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      o>  ║   " + '\n' +
	@"     /|   ║   " + '\n' +
	@"      >\  ║   " + '\n' +
	@"          ║   " + '\n' +
	@"    ══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"     /|\  ║   " + '\n' +
	@"     / \  ║   " + '\n' +
	@"          ║   " + '\n' +
	@"    ══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"     <o   ║   " + '\n' +
	@"      |\  ║   " + '\n' +
	@"     /<   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"    ══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"     /|\  ║   " + '\n' +
	@"     / \  ║   " + '\n' +
	@"          ║   " + '\n' +
	@"    ══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"     /|\  ║   " + '\n' +
	@"     / \  ║   " + '\n' +
	@"          ║   " + '\n' +
	@"    ══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      o   ║   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      |   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"    ══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      o   ║   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      |   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"    ══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      o   ║   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      |   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"    ══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      |   ║   " + '\n' +
	@"    ══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"      /   ║   " + '\n' +
	@"      \   ║   " + '\n' +
	@"    ══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"      '   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"    |__   ║   " + '\n' +
	@"    ══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"      .   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"    \__   ║   " + '\n' +
	@"    ══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"      '   ║   " + '\n' +
	@"   ____   ║   " + '\n' +
	@"    ══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"      '   ║   " + '\n' +
	@"      .   ║   " + '\n' +
	@"    __    ║   " + '\n' +
	@"   /══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"      .   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"    _ '   ║   " + '\n' +
	@"  _/══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"      '   ║   " + '\n' +
	@"      _   ║   " + '\n' +
	@" __/══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"      '   ║   " + '\n' +
	@"      .   ║   " + '\n' +
	@"          ║   " + '\n' +
	@" __/══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"      .   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"      '   ║   " + '\n' +
	@" __/══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"      '   ║   " + '\n' +
	@"      _   ║   " + '\n' +
	@" __/══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"      '   ║   " + '\n' +
	@"      .   ║   " + '\n' +
	@"          ║   " + '\n' +
	@" __/══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"      .   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"      '   ║   " + '\n' +
	@" __/══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"      '   ║   " + '\n' +
	@"      _   ║   " + '\n' +
	@" __/══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"      .   ║   " + '\n' +
	@"          ║   " + '\n' +
	@" __/══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"          ║   " + '\n' +
	@"      '   ║   " + '\n' +
	@" __/══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"          ║   " + '\n' +
	@"      _   ║   " + '\n' +
	@" __/══════╩═══",};

int currentHang = 0;

// Chooses the hang word
Random random = new Random();
string[] words = {"WORD", "PHRASE", "FRIEND", "PLACE", "VEHICLE", "EXAMPLE", "ORDENATION"};
string word = words[random.Next(0,words.Length)];
char[] wordChars = word.ToCharArray();

// Set an array for the guesses
char[] guess = new char[wordChars.Length];
for (int i = 0; i < wordChars.Length; i++){
    guess[i] = '_';
}

do {
    Console.Clear();
    DrawHang();

    // Ask for a letter
    Console.WriteLine("\nDigite uma letra: ");
    string l = Console.ReadLine().ToUpper();
    while (!Char.IsLetter(l[0])){
        Console.WriteLine("Digite uma letra: ");
        l = Console.ReadLine().ToUpper();
    }

    // Search for the letter in the word 
    int count = 0;
    for(int i = 0; i < wordChars.Length; i++){
        // If found puts in the guess at the right positions
        if (wordChars[i] == l[0]){
            guess[i] = l[0];
        }
        else count++;
    }
    // If the letter is not in the word, increments the hangman
    if (count == wordChars.Length) currentHang++;

    // If completes the hangman
    if (currentHang ==  6){
        for (int i = 0; i < deathHangs.Length; i++){
            Console.Clear();
            Console.Write(deathHangs[i]);
            Thread.Sleep(500);
        }
        Console.WriteLine("You Lose!");
        Console.WriteLine($"The word was {word}");
        break;
    }

    // Counting the number of correct letters
    count = 0;
    for (int i =0; i< guess.Length; i++){
        if (wordChars[i] == guess[i]) count++;
    }
    // If all correct letters
    if (count == wordChars.Length){
        DrawHang();
        Console.WriteLine("You win!");
        break;
    } 

}while(true);

// Displays the hang
void DrawHang(){
    Console.Write(hangs[currentHang]);
    foreach (char letter in guess){
        Console.Write($"{letter} ");
    }
}
