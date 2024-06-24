/**
* 
* Copyright (c) 2024-present, Cyballistics, LLC. All Rights Reserved.
* 
* License      The MIT License
* 
*-------------------------------------------------------------------------------------
* Author       carlos_diaz | @dfirence
* About        Applied Conceptual Usage of AMSI API.
* Purpose      Non Production grade, testing and teaching
*
* ------------------------------------------------------------------------------------
* 
* This program is released as a teaching instrument for practical usage.
* The student will learn the key aspects of initiating and disposing of resources 
* associated with the AMSI API - Antimalware Scanning Interface.
* 
* ------------------------------------------------------------------------------------
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
* ------------------------------------------------------------------------------------
*/
using System;
using AmsiWatcher.AmsiDefinition;

namespace AmsiWatcher
{
    static class Program
    {
        // Static fields for program metadata
        private static string Author { get; } = "carlos_diaz  - @dfirence";
        private static string Dashes { get; } = new string('-', 64);
        private static string License { get; } = "MIT";
        private static string ProgramName { get; } = "amsiwatcher.exe";

        // Main entry point of the program
        static void Main(string[] args)
        {
            // Check if any command-line arguments were passed
            if (args.Length == 0) { Banner(); }

            // Handle the first argument (switch) provided by the user
            switch (args[0].Trim().ToLower())
            {
                case "-t1":
                case "--test-hardcoded-string":
                    AMSIProviderBasic.RunTest();
                    break;

                case "-t2":
                case "--test-get-providers":
                    break;
                default:
                    Banner(); // If argument is not recognized, show banner
                    break;
            }
        }

        // Method to display the program banner
        static void Banner()
        {
            // Output the banner with program information
            Console.WriteLine(
            $@"

            {Dashes}
                org     :   Cyballistics - #DefenderSquad
                author  :   {Author}
                purpose :   Experiment AMSI Mechanism
                license :   {License}
            {Dashes}

                Usage:      {ProgramName} [SWITCH_OPTIONS]
            
                -t1, --test-harcoded-string     Run The Basic AMSIProvider
                -t2, --test-get-providers       Reads Windows Registry To List AMSI Registered Providers
            "
            );
            Environment.Exit(0);
        }
    }
}
