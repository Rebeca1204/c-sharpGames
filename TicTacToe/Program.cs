/* Implementation of a console game like tic tac toe */
char[] matriz = new char[9];
string player, computer;
Random random = new Random();

InitializeGame();
for (int i = 0; i < 5; i++){
    Console.Clear();
    PrintMatriz();
    Console.WriteLine("\nChoose a space to play: (1-9)");
    
    // Player's turn
    int play;
    bool ok = int.TryParse(Console.ReadLine(), out play);
    while (!ok || play < 1 || play > 9 || matriz[play - 1] == player[0] || matriz[play - 1] == computer[0]){
        Console.WriteLine("Index not available! Choose another space:");
        ok = int.TryParse(Console.ReadLine(), out play);
    }
    matriz[play - 1] = player[0];

    if (i != 4){
        // Computer's turn
        play = random.Next(1,10);
        while (matriz[play - 1] == player[0] || matriz[play - 1] == computer[0]){
            play = random.Next(1,10);
        }
        matriz[play - 1] = computer[0];
    }

    if (Win()){
        PrintMatriz();
        Console.WriteLine("Win!!");
        break;
    }
    
}
if (!Win())
    Console.WriteLine("Draw!");



// Returns true if one player has three marks in a row, column or diagonal line
bool Win(){
    return (matriz[0] == matriz[1] && matriz[1] == matriz[2]) ||
        (matriz[3] == matriz[4] && matriz[4] == matriz[5]) ||
        (matriz[6] == matriz[7] && matriz[7] == matriz[8]) ||
        (matriz[0] == matriz[3] && matriz[3] == matriz[6]) || 
        (matriz[1] == matriz[4] && matriz[4] == matriz[7]) ||
        (matriz[2] == matriz[5] && matriz[5] == matriz[8]) ||
        (matriz[0] == matriz[4] && matriz[4] == matriz[8]) ||
        (matriz[2] == matriz[4] && matriz[4] == matriz[6]);
}

// Prints the game board
void PrintMatriz(){
    Console.WriteLine($"{matriz[0]} | {matriz[1]} | {matriz[2]}");
    Console.WriteLine("-----------");
    Console.WriteLine($"{matriz[3]} | {matriz[4]} | {matriz[5]}");
    Console.WriteLine("-----------");
    Console.WriteLine($"{matriz[6]} | {matriz[7]} | {matriz[8]}");
}

// Clears the console, chooses the player marks and initialize the game board
void InitializeGame(){
    Console.Clear();
    for (int i =0; i < 9; i++){
        int x = i+1;
        matriz[i] = (char)(x + '0');
    }

    Console.WriteLine("Which player do you want to be: (X ou O)");
    player =  Console.ReadLine();
    while (player != "X" && player != "O"){
        Console.WriteLine("Which player do you want to be: (X ou O)");
        player =  Console.ReadLine();   
    }
    computer = player.Equals("X") ? "O" : "X";
}