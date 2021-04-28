using System;
using System.Collections;

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

        //number of guesses that the player has to guess the code
        int numofGuesses = 0;

        int black = 0; //correct colour and position
        int white = 0; //incorrect colour and position
        int grey = 0; //ino

        //bool to state if the game is finished or not
        bool gameOver = false;// to see if the user has beat the game
        bool Ninput = true; //to see if the user has inputted the rules set for the game
        bool gameStart = false; //to see if the game has started
        
        //integer to turn the current input into a integer from a console.readline()
        int N = 0; //amount of options to choose from e.g 4 being easiest to guess whereas 9 has 9 options, so 1/9 to be correct
        static int M = 0; //how long the user wants the code to be, 4 being easy, 8 being hard

        // this array storex the generated code
        int[] code;

        //the arrays of the inputs
        int[] inputValues;

        //bool array of how many hits
        bool[] colourHits;

        class queue
        {
            public int first;                      // youngest entry
            public int[] guesses = new int[M]; // N would be more flexible than 100 ...
        }

        static void add(queue q, int i)
        {
            if (q.first == M - 1)
            {
                System.Console.WriteLine("ERROR"); // better error handling?
            }
            else
            {
                q.first = q.first + 1;
                q.guesses[q.first] = i;
            }
        }



        //input how many colours are there available and how long the code is
        public void NInput()
        {
            //while this is true
            while (Ninput)
            {

                //ask for amount of guesses the user wants to have
                Console.Write("Please enter the number of guesses you want (Number between 1 and 10): ");
                string input = Console.ReadLine(); //read user input
                int inputValue; //integer for the input from parsing the input of string
                bool success = int.TryParse(input, out inputValue); //bool to see if the input can be converted into an integer
                bool valid = success && 1 <= inputValue && inputValue <= 10;//limits of what the integer can be
                while (!valid) //if the integer is not in the limits then it will perform this while loop as the bool is false
                {
                    Console.WriteLine("Invalid Input. Try again...");// asks for another input
                    Console.Write("Please enter the number of guesses you want (Number between 1 and 10): ");
                    input = Console.ReadLine(); // check
                    success = int.TryParse(input, out inputValue);
                    valid = success && 1 <= inputValue && inputValue <= 10;
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
                Console.WriteLine($"The code was: {RNGcode}");
                Console.WriteLine($"Colours were: {RNGcolours}");
                Ninput = false;//end this function
            }
        }

        //takes user guesses at each position
        public void readInput()
        {
            queue q = new queue();
            int count;
            q.first = 0;
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
                    Console.Write($"Please enter your guess of the secret code at position: [{i + 1}] (Number between 1 and 8) ");
                    input = Console.ReadLine();//validate the next input until the user gives a valid input
                    success = int.TryParse(input, out inputValue);
                    valid = success && 1 <= inputValue && inputValue <= 8;
                }
                inputValues[i] += inputValue;//add each value to an array
                for(i = 0; i < M; i++)
                {
                    add(q, i);
                }
            }
        }



        //process the input
        public void guessInput()
        {
            Console.Clear();//clears the console
            black = 0;//set black, white and grey variables to 0 before checking input
            white = 0;
            grey = 0;
            colourHits = new bool[colours]; //make a new bool array the size of the input to see if the user inputted the correct values
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

            for(int i = 0; i < M; i++)//it'll check the next one across if its not in the current position to increase white, white is for correct input but wrong position
            {
                if (inputValues[i] == code[i])//if input equals to the code, say they got it right and increase black, black is for correct input and position
                {
                    continue;
                }
                for (int j = 0; j < M; j++)
                {
                    if (inputValues[i] == code[j])
                    {
                        white++;
                    }
                }
            }
            grey = M - black - white; //whatever is left from the black and white from the total is the grey, incorrect input and position
            if (black == M)//if they get it all right they won
            {
                Console.WriteLine($"Number of Guesses Left: {numofGuesses} \n");
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
            Console.WriteLine("Do you want to play again?");//asks if they want to play again
            string input = Console.ReadLine();//takes input
            input.ToLower();//convert it to lower case
            if (input == "yes")//if it is yes then it'll reset the game and clear the console
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
            else if (input == "no")//if the answer is no
            {
                Console.WriteLine("Thank you for playing the game!\n");
                Console.Write("Please press any key to exit the game... ");//it will exit the game after a key is pressed
                Console.ReadKey(gameStart = false);
            }
            else//else it is an invalid answer
            {
                gameStart = false;
                Console.WriteLine("Invalid answer, Try again...");
                Replay();
            }
        }

        public void Start()//start procedure
        {
            Console.WriteLine("Type 'play' to start the game");//it'l'l ask the user to type 'play' to start the game
            string input = Console.ReadLine();//converts input to lower case after reading it
            input.ToLower();
            if (input == "play")//if the user has typed 'play' then it'll start the game
            {
                gameStart = true;
            }
            else//else it'll say it was an invalid input and recall the function
            {
                gameStart = false;
                Console.WriteLine("Invalid answer, Try again...");
                Start();
            }
        }
        static void Main(string[] args)//Main function
        {
            Mastermind game = new Mastermind();//Create a new Mastermind class
            game.Start();//calls the start procedure
            while (game.gameStart == true)//when the game is running
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
