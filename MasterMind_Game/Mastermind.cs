using System;

namespace MasterMind_Game
{
    class Mastermind
    {
        //colours that are avaliable
        string[] Colours = { "Red", "Blue", "Green", "Yellow", "Orange", "Purple", "Pink", "Turquoise" };
        //output as colours
        string[] outputColours;
        string userHistory = string.Empty; 

        int colours = 8;

        //number of guesses that player 2 has to guess the code
        int numofGuesses = 0;

        int black = 0; //correct colour and position
        int white = 0; //incorrect colour and position
        int grey = 0; //correct colour but position wrong

        //bool to state if the game is finished or not
        bool gameOver = false;// to see if the user has beat the game
        bool Ninput = true; //to see if the user has inputted the rules set for the game
        bool gameStart = false; //to see if the game has started
        
        //integer to turn the current input into a integer from a console.readline()
        int N = 0; //amount of options to choose from e.g 4 being easiest to guess whereas 9 has 9 options, so 1/9 to be correct
        int M = 0; //how long the user wants the code to be, 4 being easy, 8 being hard

        // this array storex the generated code
        int[] code;

        //the arrays of the inputs
        int[] inputValues;

        //bool array of how many hits
        bool[] colourHits;

        //array to hold history of guesses
        string[] history;

        //input how many colours are there available and how long the code is
        public void NInput()
        {
            //while this is true
            while (Ninput)
            {

                //ask for amount of guesses the user wants to have
                Console.Write("Please enter the number of guesses you want (Number between 1 and 5): ");
                string input = Console.ReadLine(); //read user input
                int inputValue; //integer for the input from parsing the input of string
                bool success = int.TryParse(input, out inputValue); //bool to see if the input can be converted into an integer
                bool valid = success && 1 <= inputValue && inputValue <= 5;//limits of what the integer can be
                while (!valid) //if the integer is not in the limits then it will perform this while loop as the bool is false
                {
                    Console.WriteLine("Invalid Input. Try again...");// asks for another input
                    Console.Write("Please enter the number of guesses you want (Number between 1 and 5): ");
                    input = Console.ReadLine(); // check
                    success = int.TryParse(input, out inputValue);
                    valid = success && 4 <= inputValue && inputValue <= colours;
                }
                numofGuesses = inputValue; //set the inputvalue to N
                Console.WriteLine($"Your input: {numofGuesses}"); //show value of N

                //ask for amount of colours available
                Console.Write("Please enter the number of colours (Number between 4 and 8): ");
                input = Console.ReadLine(); //read second user input
                success = int.TryParse(input, out inputValue); //bool to see if the input can be converted into an integer
                valid = success && 4 <= inputValue && inputValue <= colours;//limits of what the integer can be
                while (!valid) //if the integer is not in the limits then it will perform this while loop as the bool is false
                {
                    Console.WriteLine("Invalid Input. Try again...");// asks for another input
                    Console.Write("Please enter the number of colours (Number between 4 and 8): ");
                    input = Console.ReadLine(); // check
                    success = int.TryParse(input, out inputValue);
                    valid = success && 4 <= inputValue && inputValue <= colours;
                }
                N = inputValue; //set the inputvalue to N
                Console.WriteLine($"Your input: {N}"); //show value of N

                //ask for length of code 
                Console.Write("Please enter the number of positions (Number between 4 and 8): "); 
                input = Console.ReadLine(); //read third user input
                success = int.TryParse(input, out inputValue); //bool to see if the input can be converted into an integer
                valid = success && 4 <= inputValue && inputValue <= 8; //limits of the integer
                while (!valid) //if the integer is not in the limits then it will perform this while loop as the bool is false
                {
                    Console.WriteLine("Invalid Input. Try again..."); // asks for another input
                    Console.Write("Please enter the number of positions (Number between 4 and 8): ");
                    input = Console.ReadLine();// checks the second input
                    success = int.TryParse(input, out inputValue);
                    valid = success && 4 <= inputValue && inputValue <= 8;
                }
                M = inputValue; // set the inputvalue to M
                Console.WriteLine($"Your input: {M}"); // show value of M
                Console.WriteLine($"Inputs: ({numofGuesses}-{N}-{M}): "); // show both inputs

                Random rng = new Random(); //new random class
                code = new int[M]; //make an int array called code, size of M
                string RNGcode = string.Empty; // string of the rng code
                string RNGcolours = string.Empty; // string of the colours, corresponding to the numbers
                outputColours = new string[M]; //array to output the right colours
                for (int i = 0; i < M; i++) //for up to length M,
                {
                    //generate a number between 1 and N, N+1 being the limit
                    code[i] += rng.Next(1, N + 1);
                    RNGcode += code[i].ToString();//add each number to a string
                    outputColours[i] = Colours[code[i] - 1]; //add corresponding colour
                    RNGcolours += outputColours[i].ToString() + ", ";// put each colour to a string
                }
                Ninput = false;//end this function
            }
        }

        //takes user guesses at each position
        public void readInput()
        {
            string input;//string for the user input
            int inputValue;//int for the input after parsing input
            bool success;//see if the input is can integer
            bool valid;//limits to the input
            Console.WriteLine("Colour Guide: \n1 = Red\n2 = Blue\n3 = Green\n4 = Yellow\n5 = Orange\n6 = Purple\n7 = Pink\n8 = Turqoise");
            inputValues = new int[M];//int array of all the input values size of M
            for (int i = 0; i < M; i++)
            {
                Console.Write($"Please enter your guess of the secret code at position: [{i + 1}] (Number between 1 and 8) ");//asks for input at position i, i+1, ...
                input = Console.ReadLine();//reads input
                success = int.TryParse(input, out inputValue);//try parsing the input to see if its an integer
                valid = success && 1 <= inputValue && inputValue <= 8; //define limits to the input
                while (!valid)//if it is not valid
                {
                    Console.WriteLine("Invalid Input. Try again...");//ask for another input
                    Console.Write($"Please enter your guess of the secret code at position: [{i + 1}] (Number between 1 and 8)  ");
                    input = Console.ReadLine();//validate the next input until the user gives a valid input
                    success = int.TryParse(input, out inputValue);
                    valid = success && 1 <= inputValue && inputValue <= 8;
                }
                inputValues[i] += inputValue;//add each value ot an array
            }
        }



        //process the input
        public void guessInput()
        {
            Console.Clear();
            black = 0;
            white = 0;
            grey = 0;
            colourHits = new bool[colours];
            history = new string[M];
            string userCode = string.Empty;//string to hold the user's guess
            string userColours = string.Empty;//string to convert code into colours
            for (int i = 0; i < M; i++)//length of M do this
            {
                outputColours[i] = Colours[inputValues[i] - 1];//find the colour
                userColours += outputColours[i].ToString() + ", ";// add colour to string
                userCode += inputValues[i].ToString();//add each number to string
            }
            userHistory += userCode + ", ";
            Console.WriteLine($"History of your guesses: {userHistory}");
            Console.WriteLine($"Your guess: {userCode}");//output both to console
            Console.WriteLine($"Guess Colours: {userColours}\n");
            numofGuesses--;//deduct 1 fom number of guesses
            for (int i = 0; i < colours; i++)//check each digit
            {

                colourHits[i] = false;
            }

            for(int i = 0; i < M; i++)
            { 
                if (inputValues[i] == code[i])//if input equals to the code, say they got it right and increase black, black is for correct input and position
                {
                    colourHits[code[i]] = true;
                    black++;
                }
            }

            for(int i = 0; i < M; i++)
            {
                for (int j = i + 1; j < M; j++)
                {
                    if (inputValues[i] == code[j] && !colourHits[code[j]])
                    {
                        white++;
                    }
                }
            }
            grey = M - black - white;
            if (black == M)//if they get it all right they won
            {
                Console.WriteLine($"\nNumber of Guesses Left: {numofGuesses} \n");
                Console.WriteLine($"Black: {black} \n");
                Console.WriteLine($"White: {white} \n");
                Console.WriteLine($"Grey: {grey} \n");
                Console.WriteLine("You Won!");
                gameOver = true;
            }
            if(black != M)//if not try again
            {
                Console.WriteLine($"\nNumber of Guesses Left: {numofGuesses} \n");
                Console.WriteLine($"Black: {black} \n");
                Console.WriteLine($"White: {white} \n");
                Console.WriteLine($"Grey: {grey} \n");
                black = 0;
                white = 0;
                grey = 0;
                Console.WriteLine("Oops, this code was incorrect, Try Again!\n");
            }
            if (black != M && numofGuesses == 0)//if they ran out of guesses and still not correct say they lost
            {
                Console.WriteLine("You Lost!");
                string RNGcode = string.Empty; // string of the rng code
                string RNGcolours = string.Empty; // string of the colours, corresponding to the numbers
                outputColours = new string[M]; //array to output the right colours
                for (int i = 0; i < M; i++) //for up to length M,
                {
                    //generate a number between 1 and N, N+1 being the limit
                    RNGcode += code[i].ToString();//add each number to a string
                    outputColours[i] = Colours[code[i] - 1]; //add corresponding colour
                    RNGcolours += outputColours[i].ToString() + ", ";// put each colour to a string
                }
                Console.WriteLine($"The code was: {RNGcode}");
                Console.WriteLine($"Colours were: {RNGcolours}");
                gameOver = true;
            }
        }

        public void Replay()//replay function
        {
            Console.WriteLine("Do you want to play again?");
            string input = Console.ReadLine();
            input.ToLower();
            if (input == "yes")
            {
                Console.Clear();
                gameOver = false;
                gameStart = true;
                Ninput = true;
                code = new int[0];
                inputValues = new int[0];
                grey = 0;
                white = 0;
                black = 0;
                userHistory = string.Empty;

            }
            else if (input == "no")
            {
                Console.WriteLine("Thank you for playing the game!\n");
                Console.Write("Please press any key to exit the game");
                Console.ReadKey(gameStart = false);
            }
            else
            {
                gameStart = false;
                Console.WriteLine("Invalid answer, Try again...");
                Replay();
            }
        }

        public void Start()
        {
            Console.WriteLine("Type 'play' to start the game");
            string input = Console.ReadLine();
            input.ToLower();
            if (input == "play")
            {
                gameStart = true;
            }
            else
            {
                gameStart = false;
                Console.WriteLine("Invalid answer, Try again...");
                Start();
            }
        }
        static void Main(string[] args)
        {
            Mastermind game = new Mastermind();
            game.Start();
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
                }
                game.Replay();
            }
            
        }
    }
}
