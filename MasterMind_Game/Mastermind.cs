using System;
using System.Collections;

namespace MasterMind_Game
{
    class Mastermind
    {

        //colours that are avaliable
        string[] Colours = { "Red", "Blue", "Green", "Yellow", "Orange", "Purple", "Pink", "Turquoise", "Lime" };
        //output as colours
        string[] outputColours;

        //max amount of colours
        int colours = 9;

        //number of guesses that the player has to guess the code
        int numofGuessesLeft = 0;

        //to track amount of guesses gone
        int counter = 0;
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
        int M = 0; //how long the user wants the code to be, 4 being easy, 8 being hard

        //bool to see if they are happy with the rules they selected
        bool RulesSet = false;

        // this array storex the generated code
        int[] code;
        int[] permcode;

        //the arrays of the inputs
        int[] inputValues;
        int[] permInput;

        //strings for outputs
        string[] userCode;//string array to hold the user's guess
        string[] userColours;//string array for the colours of user's guess


        class Queue<Item>//queue class
        {
            private Node Head = null;//first item
            private Node End = null;//last item
            public int Size;//size of queue

            public void add(Item itemtoAdd)//add item to queue
            {
                if (isEmpty())//if it is empty then make a new item and then make it the latest item and increase the size by 1
                {
                    Head = new Node(itemtoAdd);
                    End = Head;
                    Size++;
                    return;
                }
                Node temp = new Node(itemtoAdd); //this is the same process when its empty but its just adds a new item on the queue
                End.Next = temp;
                End = temp;
                Size++;
            }

            public Item takeaway()//take the top item away
            {
                Item temp = Head.Item;
                Head = Head.Next;
                Size--;
                return temp;
            }

            public bool isEmpty()//proves if the first item is empty
            {
                return Head == null;
            }

            private class Node//this is a item of the queue
            {
                public Item Item;
                public Node Next = null;
                public Node(Item item)
                {
                    Item = item;
                }
            }

        }

        //input how many colours are there available and how long the code is
        public void NInput()
        {
            //while this is true
            while (Ninput)
            {
                if (RulesSet == false)
                {
                    //ask for amount of guesses the user wants to have
                    Console.Write("\nPlease enter the number of guesses you want (Number between 1 and 20): ");
                    string input = Console.ReadLine(); //read user input
                    int inputValue; //integer for the input from parsing the input of string
                    bool success = int.TryParse(input, out inputValue); //bool to see if the input can be converted into an integer
                    bool valid = success && 1 <= inputValue && inputValue <= 20;//limits of what the integer can be
                    while (!valid) //if the integer is not in the limits then it will perform this while loop as the bool is false
                    {
                        Console.WriteLine("Invalid Input. Try again...");// asks for another input
                        Console.Write("Please enter the number of guesses you want (Number between 1 and 20): ");
                        input = Console.ReadLine(); // check
                        success = int.TryParse(input, out inputValue);
                        valid = success && 1 <= inputValue && inputValue <= 20;
                    }
                    numofGuessesLeft = inputValue; //set the inputvalue to the number of guesses
                    numofGuesses = inputValue;

                    Console.WriteLine($"Your input: {numofGuesses}"); //show value of N

                    //ask for amount of colours available
                    Console.Write("Please enter the number of colours (Number between 4 and 9): ");
                    input = Console.ReadLine(); //read second user input
                    success = int.TryParse(input, out inputValue); //bool to see if the input can be converted into an integer
                    valid = success && 4 <= inputValue && inputValue <= colours;//limits of what the integer can be
                    while (!valid) //if the integer is not in the limits then it will perform this while loop as the bool is false
                    {
                        Console.WriteLine("Invalid Input. Try again...");// asks for another input
                        Console.Write("Please enter the number of colours (Number between 4 and 9): ");
                        input = Console.ReadLine(); // check
                        success = int.TryParse(input, out inputValue);
                        valid = success && 4 <= inputValue && inputValue <= colours;
                    }
                    N = inputValue; //set the inputvalue to N
                    Console.WriteLine($"Your input: {N}"); //show value of N

                    //ask for length of code 
                    Console.Write("Please enter the number of positions (Number between 4 and 9): ");
                    input = Console.ReadLine(); //read third user input
                    success = int.TryParse(input, out inputValue); //bool to see if the input can be converted into an integer
                    valid = success && 4 <= inputValue && inputValue <= colours; //limits of the integer
                    while (!valid) //if the integer is not in the limits then it will perform this while loop as the bool is false
                    {
                        Console.WriteLine("Invalid Input. Try again..."); // asks for another input
                        Console.Write("Please enter the number of positions (Number between 4 and 9): ");
                        input = Console.ReadLine();// checks the second input
                        success = int.TryParse(input, out inputValue);
                        valid = success && 4 <= inputValue && inputValue <= colours;
                    }
                    M = inputValue; // set the inputvalue to M
                    Console.WriteLine($"Your input: {M}\n"); // show value of M
                    Console.WriteLine($"You chose:\nAmount of Guesses: {numofGuesses}\nNumbers to pick from: 1 to {N}\nLength of the code you have to guess: {M} digits"); // show both inputs
                    RulesSet = true;
                }
                Random rng = new Random(); //new random class

                code = new int[M]; //initialise an int array called code, size of M
                permcode = new int[M];//initialise permcode with size of M
                userCode = new string[M];//initialise userCode with size of M
                userColours = new string[M];//initialise userColours with size of M

                string RNGcode = string.Empty; // string of the rng code
                string RNGcolours = string.Empty; // string of the colours, corresponding to the numbers
                outputColours = new string[M]; //array to output the right colours

                for (int i = 0; i < M; i++) //for up to length M,
                {
                    //generate a number between 1 and N, N+1 being the limit
                    code[i] += rng.Next(1, N + 1);
                }
                permcode = code;
                Console.Write("\nPlease press the enter key to continue the game... or press B to go back ");//it will exit the game after a key is pressed
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.Enter)
                {
                    Ninput = false;//end this function
                    Console.Clear();
                }
                else if (key.Key == ConsoleKey.B)
                {
                    RulesSet = false;
                    NInput();
                    Console.Clear();
                }
                else//else it is an invalid answer
                {
                    RulesSet = true;
                    Console.WriteLine("\nInvalid answer, Try again...");
                }
            }
        }

        //takes user guesses at each position
        public void readInput()
        {
            string input;//string for the user input
            int inputValue;//int for the input after parsing input
            bool success;//see if the input is can integer
            bool valid;//limits to the input

            Console.WriteLine("Colour Guide:\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" 1 = Red, ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(" 2 = Blue,");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" 3 = Green, ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" 4 = Yellow, ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(" 5 = Orange, ");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;  
            Console.WriteLine(" 6 = Purple, ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(" 7 = Pink, ");
            Console.ForegroundColor = ConsoleColor.Cyan; 
            Console.WriteLine(" 8 = Turqoise, ");
            Console.ForegroundColor = ConsoleColor.Green; 
            Console.WriteLine(" 9 = Lime.\n");
            Console.ResetColor();
            inputValues = new int[M];//int array of all the input values size of M
            permInput = new int[M];//this is the permanent storage of each guess which should be size of the code length

            for (int i = 0; i < M; i++)
            {
                Console.WriteLine($"Please enter your guess of the secret code at position: [{i + 1}] (Number between 1 and 9) ");//asks for input at position i, i+1, ...
                input = Console.ReadLine();//reads input
                success = int.TryParse(input, out inputValue);//try parsing the input to see if its an integer
                valid = success && 1 <= inputValue && inputValue <= colours; //define limits to the input
                while (!valid)//if it is not valid
                {
                    Console.WriteLine("Invalid Input. Try again...");//ask for another input
                    Console.Write($"Please enter your guess of the secret code at position: [{i + 1}] (Number between 1 and 9) ");
                    input = Console.ReadLine();//validate the next input until the user gives a valid input
                    success = int.TryParse(input, out inputValue);
                    valid = success && 1 <= inputValue && inputValue <= colours;
                }
                inputValues[i] += inputValue;//add each value to an array
                permInput[i] = inputValues[i];
            }
        }

        //processes the input and checks the user code 1 by 1
        public void guessInput()
        {
            Console.Clear();//clears the console
            black = 0;//set black, white and grey variables to 0 before checking input
            white = 0;
            grey = 0;
            numofGuessesLeft--;//deduct 1 fom number of guesses

            bool colourTrue = false; //make a new bool to see if the user inputted the correct values

            for(int i = 0; i < M; i++)
            { 
                if (inputValues[i] == code[i])//if input equals to the code, say they got it right and increase black, black is for correct input and position
                {
                    colourTrue = true;
                    black++;
                }
            }

            for (int i = 0; i < M; i++)//it'll check the next one across if its not in the current position to increase white, white is for correct input but wrong position
            {
                if (colourTrue == false)
                {
                    for (int j = 0; j < M; j++)
                    {
                        if (inputValues[j] == code[i])
                        {
                            white++;
                            //this is to stop duplicates
                            code[i] = -1;
                            inputValues[j] = -2;
                        }
                    }
                }
            }

            //make sure its back to the original code
            grey = M - black - white; //whatever is left from the black and white from the total is the grey, incorrect input and position

            if (black == M)//if they get it all right they won
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("You Won!");
                Console.ResetColor();
                addHistory();
                Console.WriteLine($"Number of Guesses Left: {numofGuessesLeft} \n");
                Console.WriteLine($"Black: {black}");
                Console.WriteLine($"White: {white}");
                Console.WriteLine($"Grey: {grey}");
                gameOver = true;
            }
            if(black != M && numofGuessesLeft != 0)//if not try again
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Oops, this code was incorrect, Try Again!\n");
                Console.ResetColor();
                addHistory();
                Console.WriteLine($"\nNumber of Guesses Left: {numofGuessesLeft}");
                Console.WriteLine($"Black: {black}");
                Console.WriteLine($"White: {white}");
                Console.WriteLine($"Grey: {grey}");
                black = 0;
                white = 0;
                grey = 0;

            }
            if (black != M && numofGuessesLeft == 0)//if they ran out of guesses and still not correct say they lost
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You Lost!");
                Console.ResetColor();
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
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"The code was: {RNGcode} - {RNGcolours}");

                gameOver = true;
            }
        }

        //add History of Guesses by queues
        public void addHistory()
        {
            if (counter < numofGuesses)//when the game is running
            {
                int colourID = 0;
                string userColour = string.Empty;//string to convert code into colours
                string userGuess = string.Empty;//string to convert current user guess into one string
                Queue<int> guessID = new Queue<int>();//queue for guess ID
                Queue<string> CodeQ = new Queue<string>();//queue for user guess
                Queue<string> ColoursQ = new Queue<string>();//queue for colours equivalent to number
                for (int i = 0; i < M; i++)//for up to the code length convert all guesses to a string e.g 1,2,3,4 becomes 1234
                {
                    colourID = inputValues[i];
                    outputColours[i] = Colours[colourID - 1];//find the colour
                    userColour += outputColours[i].ToString() + ", ";// add colour to string
                    userGuess += inputValues[i].ToString();
                }
                userCode[counter] += userGuess;//add each string to the array based on how many goes there have been - evaluated by counter
                userColours[counter] += userColour;//add colour equivalent
                counter++;
                for (int i = 0; i < counter; i++)//for amount of guesses completed
                {
                    CodeQ.add(userCode[i]);//add each guess to the queue
                    ColoursQ.add(userColours[i]);//add each colur equivalent of code to queue
                    guessID.add(i + 1);//add guess ID to queue
                }
                Console.WriteLine("History of Guesses:\n");
                while (!CodeQ.isEmpty() && !ColoursQ.isEmpty())//when its not empty dequeue it 1 by 1 so its prints out current guesses on a game
                {
                    Console.WriteLine($"Guess {guessID.takeaway()}: " + CodeQ.takeaway() + " - " + ColoursQ.takeaway());
                }
            }
        }

        public void Replay()//replay function
        {
            Console.WriteLine("Do you want to play again?");//asks if they want to play again
            string input = Console.ReadLine();//takes input
            input = input.ToLower();//convert it to lower case
            if (input == "yes")//if it is yes then it'll reset the game and clear the console
            {
                Console.Clear();
                Main();
                Console.ResetColor();

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
        static void Main()//Main function
        {
            Console.ForegroundColor = ConsoleColor.White;
            Mastermind game = new Mastermind();//Create a new Mastermind class
            game.Start();//calls the start procedure
            while (game.gameStart)//when the game is running
            {
                Console.WriteLine("Hello and Welcome to my Mastermind Code Breaker!");

                while (game.gameOver == false)//when they haven't won do this
                {
                    if (game.Ninput)//for when they're inputting the rules
                    {
                        game.NInput();
                    }
                    game.readInput();//takes guess input
                    Console.WriteLine(Environment.NewLine);
                    game.guessInput();//outcome
                }
                game.Replay();//replay function
            }
            
        }
    }
}
