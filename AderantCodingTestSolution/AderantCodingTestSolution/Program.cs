using System;
using System.Collections.Generic;
using System.IO;

namespace AderantCodingTestSolution
{
    class Program
    {
            public static void Main(string[] args)
            {
                bool endApp = false;
                Console.WriteLine("Aderant Test Solution by Kritika Rathore:");
                Console.WriteLine("-----------------------------------------");
                while (!endApp)
                {
                    //Get the Input File Name existing in InputFragments Folder
                    var inputFile = @"..\..\InputFragments\";
                    Console.WriteLine("Please enter Input File Name (like Input1 or Input2) from :\n1.Input1 \n2.Input2 \n3.Input3 \n4.Input4 \n");
                    var fileName = Console.ReadLine();
                    inputFile += fileName + ".txt";

                    //Create a new dictionary to store all fragments in Input file
                    var fileString = new Dictionary<int, string>();
                    var obj = new Program();
                    var finalString = "";   //Final Output String
                    int count = 1;  // used to store unique Key in dictionary
                    try
                    {   // Open the text file using a stream reader and read the input
                        using (StreamReader sr = new StreamReader(inputFile))
                        {
                            // Read the stream to a string, and store in a dictionary with key value pair
                            string s;
                            //Read first Line of File
                            s = sr.ReadLine();
                            if (s == null)
                            {    //No Fragments in file
                                Console.WriteLine("No Input Fragments found.");
                            }
                            else               //Fragments existing
                            {
                                //store the value with key in dictionary and iterate for next line until no more fragments
                                do
                                {
                                    //Flag to identify if the string is substring of any existing String Fragment
                                    bool existFlag = false;
                                    //To store the value of Key whose string fragment is substring of current Fragment being read
                                    int existKey = 0;
                                    //Check for substring existing only if more than one fragment
                                    if (count > 1)
                                    {
                                        foreach (var pair in fileString)
                                        {
                                            if (pair.Value.Contains(s))     // when current string s is substring of any existing String Fragment
                                            {
                                                existFlag = true;           // set the flag true to take no action
                                            }
                                            else if (s.Contains(pair.Value)) // existing string fragment is substring of current Fragment being read
                                            {
                                                existKey = pair.Key;        // store the key to later update the value with larger string
                                                existFlag = true;
                                            }
                                        }
                                    }
                                    if (!existFlag)
                                    {
                                        fileString.Add(count++, s);     //store the current fragment in dictionary if not already there
                                    }
                                    else if (existKey > 0)
                                    {
                                        fileString[existKey] = s;       //update the existing fragment with larger string
                                    }
                                } while ((s = sr.ReadLine()) != null);
                                //----------------------------------------------------------------------------//
                                // Create object of Algorithm to construct the final string after overlap merged
                                var algoObj = new GreedyMatchMerge(fileString, count);
                                finalString = algoObj.GetFinalOutput();     //returns the final output string
                                                                            //Print the output
                                Console.WriteLine("Output String : {0} \n", finalString);
                            }

                        }
                    }
                    catch (IOException e)   //This will catch any exception raised due to StreamReader
                    {
                        Console.WriteLine("The file could not be read:");
                        Console.WriteLine(e.Message);
                    }
                    Console.WriteLine("Enter exit to close the application or press any key and enter to continue.");
                    var i = Console.ReadLine();
                    if (i.ToUpper() == "EXIT")
                    {
                        endApp = true;
                    }
                }
            }
    }
}
