using System;
/* Implementation of a console game like battleship with a board 5x5 and 5 ships*/
Random random = new Random();
int[ , ] boardComputer = new int[5,5];
int[ , ] boardPlayer = new int[5,5];
int[ , ] places = new int[5,5];
int[ , ] places2 = new int[5,5];
int compSinked = 0;
int playerSinked = 0;

InitializeGame();
for (int i = 0; i < 25; i++){
    Console.Clear();
    // Player's turn
    Console.WriteLine("Your turn");
    PrintPlaces(); // player's view of the computer board
    int play;
    // Shoots at choosen place
    Console.WriteLine("\nChoose a space to shoot: (1-25)");
    bool ok = int.TryParse(Console.ReadLine(), out play);
    while (!ok || play < 1 || play > 25 || places[(play - 1) / 5, (play - 1) % 5] == 0){
        Console.WriteLine("Index not available! Choose another space:");
        ok = int.TryParse(Console.ReadLine(), out play);
    }

    Console.Clear();

    // There is a ship
    if (boardComputer[(play - 1) / 5, (play - 1) % 5] == 1){
        Console.WriteLine("You found a ship!");
        playerSinked++;
        // marks for the player that there is a ship there on the computer board
        places[(play - 1) / 5, (play - 1) % 5] = 1;
        PrintPlaces();
        Thread.Sleep(5000);
        Console.Clear();
    }
    else{
        Console.WriteLine("You missed!");
        places[(play - 1) / 5, (play - 1) % 5] = 0;
        PrintPlaces();
        Thread.Sleep(5000);
        Console.Clear();
    }

    // If all 5 ships are already found by the player
    if (playerSinked == 5){
        Console.WriteLine("You found all 5 ships!");
        Console.WriteLine("You win!");
        break;
    }

    Console.Clear();
    Console.WriteLine("Computer's turn");
    // Computer chooses a space to play
    play = random.Next(1,25);
    // There is a ship
    if (boardPlayer[(play - 1) / 5, (play - 1) % 5] == 1){
        Console.WriteLine($"Computer found a ship at {play}!");
        compSinked++;
        // marks for the computer that there is a ship there on the player board
        places2[(play - 1) / 5, (play - 1) % 5] = 1;
        PrintPlaces2();
        Thread.Sleep(5000);
    }else{
        Console.WriteLine($"Computer missed at {play}!");
        places2[(play - 1) / 5, (play - 1) % 5] = 0;
        PrintPlaces2();
        Thread.Sleep(5000);
    }

    Console.Clear();

    // If all 5 ships are already found by the computer
    if (compSinked == 5){
        Console.WriteLine("Computer found all of your 5 ships!");
        Console.WriteLine("Computer wins! You lose!");
        break;
    }
}

void InitializeGame(){
    Console.Clear();
    // Creates a blank 5x5 boardComputer for the computer and player
    for (int i = 0; i < 5; i++){
        for (int j = 0; j < 5; j++){
            boardComputer[i,j] = 0;
            boardPlayer[i,j] = 0;
            places[i,j] = (i) * 5 + (j+1);
            places2[i,j] = (i) * 5 + (j+1);
        }
    }

    // Chooses at random 5 places to put ships in the computer board
    int cont = 0;
    while (cont !=5){
        int rand = (random.Next(1,26)) -1;
        if (boardComputer[(rand) / 5, (rand) % 5] == 0){
            boardComputer[(rand) / 5, (rand) % 5] = 1;
            cont++;
        }
    }

    // Asks the user and places 5 ships in the player board
    PrintPlaces();
    for (int i = 0; i < 5; i++){
        Console.WriteLine($"\nChoose a space to put your ship {i+1}: (1-25)");
        int play;
        bool ok = int.TryParse(Console.ReadLine(), out play);
        while (!ok || play < 1 || play > 25 || boardPlayer[(play - 1) / 5, (play - 1) % 5] == 1){
            Console.WriteLine("Index not available! Choose another space:");
            ok = int.TryParse(Console.ReadLine(), out play);
        }
        boardPlayer[(play - 1) / 5, (play - 1) % 5] = 1;
    }

}

// Print the board show for the player
void PrintPlaces(){

    for (int i = 0; i < 5; i++){
        for (int j = 0; j < 5; j++){
            Console.Write($"{places[i,j]} ");
        }
        Console.WriteLine("");
    }
}

// Print the board show for the computer
void PrintPlaces2(){
    
    for (int i = 0; i < 5; i++){
        for (int j = 0; j < 5; j++){
            Console.Write($"{places2[i,j]} ");
        }
        Console.WriteLine("");
    }
}