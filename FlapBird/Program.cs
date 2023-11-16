/* Implementation of a console game like flappy bird */
Random random = new Random();
Console.CursorVisible = false;
int height = Console.WindowHeight - 1;
int width = Console.WindowWidth - 5;
bool shouldExit = false;

// Inicial points
int points = 0;

// Console position of the bird and pipe
int birdX = width / 8;
int birdY = height / 2;
int pipeX = width;
int pipeHeight = random.Next(3, height - 5);

// Available birds
string[] states = { @"~(^')>", @"~(v')>" };

string bird = states[0];

InitializeGame();
while (!shouldExit)
{
    if (TerminalResized())
    {
        Console.Clear();
        Console.Write("Console was resized. Program exiting.");
        shouldExit = true;
    }
    else
    {
        Move();
        ShowPipe();

        if (TouchEnd())
        {
            Console.Clear();
            Console.WriteLine("Game Over!");
            Console.WriteLine($"You have {points} points!");
            shouldExit = true;
        }
        Thread.Sleep(100);
    }
}

// Returns true if the Terminal was resized 
bool TerminalResized()
{
    return height != Console.WindowHeight - 1 || width != Console.WindowWidth - 5;
}

// Displays random pipe with random height
void ShowPipe()
{
    // Display the pipe up to the choosen height
    for (int i = 0; i <= pipeHeight; i++)
    {
        Console.SetCursorPosition(pipeX, height - i);
        Console.Write('|');
    }

    // Clear the pipe at the previous position
    for (int i = 0; i <= height; i++)
    {
        Console.SetCursorPosition(pipeX + 1, i);

        Console.Write(" ");
    }

}

// Returns true if the bird location matches the pipe location or if the bird gets out of bounds
bool TouchEnd()
{
    return birdY == 0 || birdY == height || (birdX == pipeX && birdY >= height - pipeHeight);
}

// Changes the bird (flaps his wings)
void ChangePlayer(int x)
{
    bird = states[x];
}


// Reads directional input from the Console and moves the player
void Move()
{
    if (pipeX <= birdX)
    {
        // Clear the pipe at the previous position
        for (int i = 0; i <= height; i++)
        {
            Console.SetCursorPosition(pipeX, i);

            Console.Write(" ");
        }

        points++;
        // Set new pipe
        pipeX = width;
        pipeHeight = random.Next(3, height - 5);
    }

    // Clear the bird at the previous position
    Console.SetCursorPosition(birdX, birdY);
    for (int i = 0; i < bird.Length; i++)
    {
        Console.Write(" ");
    }

    // If space pressed, bird flaps his wings
    if (Console.KeyAvailable)
    {
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);

        if (keyInfo.Key == ConsoleKey.Spacebar)
        {
            birdY--;
            ChangePlayer(0);
        }
    }
    else
    {
        // bird falls
        birdY++;
        ChangePlayer(1);
    }

    // Draw the bird at the new location
    Console.SetCursorPosition(birdX, birdY);
    Console.Write(bird);

    // Move the pipe closer
    if (pipeX > birdX)
        pipeX -= 1;
}

// Clears the console, displays the bird
void InitializeGame()
{
    Console.Clear();
    Console.SetCursorPosition(birdX, birdY);
    Console.Write(bird);
}