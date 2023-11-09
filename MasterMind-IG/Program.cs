//Author: Sierro Félix
//Date: 09.11.2023
//Place: ETML - Vennes
//Description: code for the game MasterMind

using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MasterMind
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int userChoice = 0;

            //Welcome message and askes the user's name
            Console.Write("Bienvenue sur Mastermind! Veuillez entrer votre prénom: ");
            string name = Console.ReadLine();
            Console.WriteLine("\nBonjour " + name + " que voulez-vous faire?\n");

            //Allows the alignement of the * with the congratulation message 
            string output = new string('*', name.Length);

            //Allows to go to the main menu
            do
            {
                userChoice = ShowMenu();
                PerformChoice(userChoice);
            } while (userChoice != 4);

            int ShowMenu()

            {
                int choice;

                //Shows the main menu
                Console.WriteLine("[1] Mode facile");
                Console.WriteLine("[2] Mode difficile");
                Console.WriteLine("[3] Règles");
                Console.WriteLine("[4] Quitter\n");

                //Allows to choose between 1, 2, 3 and nothing else
                while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 4)
                {
                    Console.WriteLine("\nVeuillez entrer un chiffre entre 1 et 4\n");
                }
                return choice;
            }

            void PerformChoice(int choice)
            {
                switch (choice)
                {
                    case 1:
                        Game(4);
                        break;
                    case 2:
                        Game(6);
                        break;
                    case 3:
                        Rules();
                        break;
                    case 4:
                        Environment.Exit(0);
                        break;
                }
            }

            void Rules()
            {
                //Clears the UI
                Console.Clear();

                //Shows the rules
                Console.WriteLine("\n***********************************************************************************\n***********************************************************************************" +
                                  "\nVous devez trouver la combinaison de couleurs secrètes avec les couleurs suivantes : \nG (vert), Y (jaune), W (blanc), R (rouge), B (bleu), M (magenta) et C (cyan)\n\nCertains codes peuvent comporter plusieurs fois la même couleur." +
                                  "\n\nVous avez plusieurs essais, mais attention, vous n'en avez que 10.\n" +
                                  "\nSi vous placez une ou plusieurs couleurs correctement, la console vous l'indiquera \navec le terme *ok* suivi du nombre de couleurs bien placées.\n" +
                                  "\nSi vous placez une ou plusieurs bonnes couleurs mais pas au bon endroit, la console \nvous l'indiquera avec le terme *presque* suivi du nombre de couleurs à corriger.\n" +
                                  "\nDans le mode facile [1], vous devez trouver la bonne combinaison de 4 couleurs.\nDans le mode difficile [2], vous devez trouver la bonne combinaison de 6 couleurs." +
                                  "\n***********************************************************************************\n***********************************************************************************\n");
            }

            //Game
            void Game(int n)
            {
                //Clears the UI
                Console.Clear();

                if (n == 4)
                    Console.Write("\n\nTrouvez le code en 4 couleurs\n");
                else if (n == 6)
                    Console.Write("\n\nTrouvez le code en 6 couleurs\n");

                //Creates the random code with the allowed inputs
                var random = new Random();
                char[] colors = { 'G', 'Y', 'W', 'R', 'B', 'M', 'C' };

                string secret = "";

                //Lenght of the secret code
                for (int i = 0; i < n; i++)
                {
                    var symbolIndex = random.Next(colors.Length);
                    secret += colors[symbolIndex];
                }

                //Shows the secret code for the tests
                //Console.WriteLine($"secret is:{secret}");

                //Limits the number of tries
                int g = 10;
                String input;

                //Shows the remaining number of tries
                for (int j = 0; j < g; j++)
                {
                    bool isValidInput;

                    //Limits the number of inputs to 4 and makes it possible for the input to be written in lower caps
                    do
                    {
                        //Adds one stars for the 10th essay
                        string stars = (j == 9) ? "*" : "";
                        if (n == 4)
                            Console.Write("\n******************" + stars + "\nEssaie no " + (j + 1) + " : ");
                        else if (n == 6)
                            Console.Write("\n********************" + stars + "\nEssaie no " + (j + 1) + " : ");
                        input = Console.ReadLine().ToUpper();

                        //Checks if the input is valid
                        isValidInput = input.Length == n && input.All(c => colors.Contains(c));

                    } while (input.Length != n);

                    //Checks if the input is valid without showing the hint if the user has put a correct input
                    if (!isValidInput)
                    {
                        if (n == 4) Console.Write("\n\n******************\n\n");
                        else if (n == 6) Console.Write("\n\n********************\n\n");
                        Console.WriteLine("\nVeuillez utiliser uniquement les couleurs à disposition.\n");
                        j--;
                    }

                    //Congratulation message
                    else if (secret == input)
                    {

                        //Adds a * in the 10th try 
                        string stars = (j == 9) ? "*" : "";
                        if (n == 4) Console.Write("\n******************" + stars + "\n\n");
                        else if (n == 6) Console.Write("\n********************" + stars + "\n\n");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n**********************************" + output + "\n Félicitations " + name + ", vous avez gagné !\n**********************************" + output + "\n\n");
                        Console.ResetColor();
                        Console.WriteLine("Que voulez-vous faire maintenant ?\n");
                        break;
                    }

                    else if (secret == input && j == 9)
                    {
                        if (n == 4) Console.Write("\n*********************\n\n");
                        else if (n == 6) Console.Write("\n***********************\n\n");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n**********************************" + output + "\n Félicitation " + name + ", vous avez gagné !\n**********************************" + output + "\n\n");
                        Console.ResetColor();
                        Console.WriteLine("Que voulez-vous faire maintenant ?\n");
                        break;
                    }

                    //Losing message
                    else if (j == 9)
                    {
                        Console.WriteLine("\n\nPas de chance, vous avez perdu. Le code secret était " + secret + ".\n\nQue voulez-vous faire maintenant ?\n");
                    }

                    else
                    {
                        int ok = 0;
                        int notOk = 0;

                        //Table that stocks the number of instances of every letters in the code and the user's input
                        int[] secretLetterCounts = new int[colors.Length];
                        int[] inputLetterCounts = new int[colors.Length];

                        //Counts the number of letters in every letters in the code
                        foreach (char letter in secret)
                        {
                            secretLetterCounts[Array.IndexOf(colors, letter)]++;
                        }

                        //Counts the number of instances of every letters in the user's input
                        foreach (char letter in input)
                        {
                            //Checks if the input is a letter and if it is in the table
                            if (char.IsLetter(letter) && Array.Exists(colors, element => element == char.ToUpper(letter)))
                            {
                                inputLetterCounts[Array.IndexOf(colors, char.ToUpper(letter))]++;
                            }
                        }

                        for (int i = 0; i < secret.Length; i++)
                        {
                            //Right color and right place
                            if (input[i] == secret[i])
                            {                              
                                Console.Write(input[i]);
                                Console.ResetColor();
                                ok++;
                            }

                            //Wrong color
                            else if (input[i] != secret[i])
                            {
                                Console.Write("_");
                                Console.ResetColor();
                            }
                        }
                        //Right color but in the wrong place
                        for (int i = 0; i < colors.Length; i++)
                        {
                            notOk += Math.Min(secretLetterCounts[i], inputLetterCounts[i]);
                        }

                        notOk -= ok;
                        Console.WriteLine("\nok : " + ok);
                        if (n == 4) Console.WriteLine("presque : " + notOk + "\n******************\n\n");
                        else if (n == 6) Console.WriteLine("presque : " + notOk + "\n********************\n\n");
                        Console.ResetColor();
                    }
                }
            }
        }
    }
}
