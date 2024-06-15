
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
