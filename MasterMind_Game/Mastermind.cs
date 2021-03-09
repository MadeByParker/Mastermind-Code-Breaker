using System;

namespace MasterMind_Game
{
    class Mastermind
    {
        //colours that are avaliable
        string[] Colours = { "Red", "Blue", "Green", "Yellow", "Orange", "Purple", "Pink", "Turquoise" };
        string[] outputColours = new string[9];

        //number of guesses that player 2 has to guess the code
        int numofGuesses = 5;

        int black = 0; //colour wrong and position wrong
        int white = 0; //colour correct but position wrong
        int red = 0; //colour and position correct

        //bool to state if the game is finished or not
        bool gameOver = false;
        bool Ninput = true;
        bool gameStart = true;
        
        //integer to turn the current input into a integer from a console.readline()
        int N = 0;
        int M = 0;
        int[] code;

        //the arrays of the inputs
        int[] inputValues;
        int[] guessValues;


        public void NInput()
        {
            while (Ninput)
            {
                Console.Write("Please enter the number of colours: ");
                string input = Console.ReadLine();
                int inputValue;
                bool success = int.TryParse(input, out inputValue);
                bool valid = success && 4 <= inputValue && inputValue <= 9;
                while (!valid)
                {
                    Console.WriteLine("Invalid Input. Try again...");
                    Console.Write("Please enter the number of colours: ");
                    input = Console.ReadLine();
                    success = int.TryParse(input, out inputValue);
                    valid = success && 4 <= inputValue && inputValue <= 9;
                }
                N = inputValue;
                Console.WriteLine($"Your input: {N}");

                Console.Write("Please enter the number of positions: ");
                input = Console.ReadLine();
                success = int.TryParse(input, out inputValue);
                valid = success && 4 <= inputValue && inputValue <= 8;
                while (!valid)
                {
                    Console.WriteLine("Invalid Input. Try again...");
                    Console.Write("Please enter the number of positions: ");
                    input = Console.ReadLine();
                    success = int.TryParse(input, out inputValue);
                    valid = success && 4 <= inputValue && inputValue <= 8;
                }
                M = inputValue;
                Console.WriteLine($"Your input: {M}");
                Console.WriteLine($"Inputs: ({N}-{M}): ");

                Random rng = new Random();
                code = new int[M];
                string RNGcode = string.Empty;
                for (int i = 0; i < M; i++)
                {
                    code[i] += rng.Next(1, N + 1);
                    RNGcode += code[i].ToString();
                }
                Console.WriteLine($"The code is: {RNGcode}");
                Ninput = false;
            }
            
        }

        public bool TryParseIntegerList(string input, out int value)
        {
            string[] splits = new string[4];
            for (int i = 0; i < 4; i++)
            {

                splits[i] += input;
            }
            int[] result = new int[splits.Length];
            for (int i = 0; i < splits.Length; i++)
            {
                if (!int.TryParse(splits[i], out value))
                {
                    return false;
                }
            }
            value = int.Parse(input);
            if (value < 1 || value > 8)
            {
                return false;
            }
            return true;
        }


        public void readInput()
        {
            string input;
            int inputValue;
            bool success;
            bool valid;
            inputValues = new int[M];
            for (int i = 0; i < M; i++)
            {
                Console.Write($"Please enter your guess of the secret code at position: [{i}] ");
                input = Console.ReadLine();
                success = int.TryParse(input, out inputValue);
                valid = success && 1 <= inputValue && inputValue <= 8;
                while (!valid)
                {
                    Console.WriteLine("Invalid Input. Try again...");
                    Console.Write($"Please enter your guess of the secret code at position: [{i}]  ");
                    input = Console.ReadLine();
                    success = int.TryParse(input, out inputValue);
                    valid = success && 1 <= inputValue && inputValue <= 8;
                }
                inputValues[i] += inputValue;

            }
            string userCode = string.Empty;
            for (int i = 0; i < M; i++)
            {
                userCode += inputValues[i].ToString();
            }
            Console.WriteLine($"Your code: {userCode}");
        }

        public void guessInput()
        {
            for(int i = 0; i < M; i++)
            {
                if(inputValues[i] == code[i])
                {
                    Console.WriteLine("Correct Colour and Position");
                    black++;
                }
                else
                {
                    Console.WriteLine("Incorrect colour and position");
                    white++;
                }
            }
            if(black == M)
            {
                gameOver = true;
            }
            else
            {
                Console.WriteLine("Oops, try again!");
            }
        }

        public void Replay()
        {
            Console.WriteLine("Do you want to play again?");
            string input = Console.ReadLine();
            if (input == "yes")
            {
                Console.Clear();
                gameOver = false;
                gameStart = true;
                Ninput = true;
                code = new int[0];
                inputValues = new int[0];
                red = 0;
                white = 0;
                black = 0;

            }
            else if (input == "no")
            {
                Environment.Exit(0);
            }
            else
            {
                gameStart = false;
                Console.WriteLine("Invalid answer, Try again...");
                Console.WriteLine("Do you want to play again?");
                Console.ReadLine();
            }
        }
        static void Main(string[] args)
        {
            
            Mastermind game = new Mastermind();
            while (game.gameStart == true)
            {
                Console.WriteLine("Hello and Welcome to my Mastermind Code Breaker!");

                Console.WriteLine(Environment.NewLine);

                while (game.gameOver == false)
                {
                    if (game.Ninput == true)
                    {
                        game.NInput();
                    }
                    game.readInput();
                    Console.WriteLine(Environment.NewLine);
                    game.guessInput();
                    game.numofGuesses--;
                    if (game.numofGuesses == 0 && game.gameOver == false)
                    {
                        Console.WriteLine("You lost!");
                        game.gameOver = true;
                    }
                    Console.WriteLine("You Won!");
                    game.gameOver = true;
                }
                game.Replay();
            }
            
        }
    }
}
