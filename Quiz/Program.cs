using System;
/* Implementation of a console game like a quiz about the ASCII table */
Random random = new Random();
char[] chars = {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' ,'H' ,'I', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'};
bool win;
int i =0;
do {
    Console.Clear();
    // Question with a random letter
    int x = random.Next(0,24);
    Console.WriteLine($"Question {i+1}: What is the number in the ASCII table equivalent to: {chars[x]} ");
    int answer;
    // Read the answer as a integer
    bool ok = int.TryParse(Console.ReadLine(), out answer);
    while (!ok){
        Console.WriteLine("Answer must be a number. Try again:");
        ok = int.TryParse(Console.ReadLine(), out answer);
    }
    // If the answer is correct, go to the next
    if (answer == (int)chars[x]){
        Console.WriteLine("Correct!");
        Thread.Sleep(500);
        win = true;
        i++;
    }else {
        Console.WriteLine($"Wrong answer! Correct: {(int) chars[x]}");
        Thread.Sleep(500);
        win = false;
    }

}while (win);