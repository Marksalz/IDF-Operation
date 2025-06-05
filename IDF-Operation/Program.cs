using IDF_Operation.DTOs;
using IDF_Operation.Hamas;
using IDF_Operation.IDF;
using IDF_Operation.StrikeOptions;
using System.Text.Json;

namespace IDF_Operation
{
    internal class Program
    {
        static Random rnd = new Random();
        static List<string> weapons = ["Knife", "Gun", "M16", "AK47"];

        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("You must pass the API key as a command-line argument.");
                return;
            }

            string apiKey = args[0];

            // Call the Gemini API to generate the terrorist and Aman reports lists
            var (terrorists, amanReports) = await generateTerroristAmanList(apiKey);


            // Initialize the IDF
            var strikeOptions = generateStrikeOptions();
            var idf = new Idf("Eyal Zamir", strikeOptions);

            // Initialize the Hamas
            var commander = new Terrorist("El-Arory", 5, true, weapons);
            var hamas = new Hamas.Hamas(commander);
            hamas.AddTerroristListAsync(terrorists);


            bool running = true;
            while (running)
            {
                PrintMenu();
                int choice = GetMenuChoice(0, 5);

                Console.WriteLine();
                switch (choice)
                {
                    case 0:
                        var mostReported = intelAnalysis(amanReports);
                        if (mostReported != null)
                            Console.WriteLine($"Most reported terrorist: {mostReported}");
                        else
                            Console.WriteLine("No terrorist found in reports.");
                        break;
                    case 1:
                        var available = strikeAvailability(idf);
                        Console.WriteLine("Available Strike Options:");
                        foreach (var opt in available)
                            Console.WriteLine(opt.printStrikeOptionsNameAndCapicaty());
                        break;
                    case 2:
                        try
                        {
                            var target = chooseTarget(hamas);
                            Console.WriteLine($"Most dangerous terrorist: {target}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case 3:
                        try
                        {
                            var target = chooseTarget(hamas);
                            strikeExecution(target, amanReports, idf);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case 4:
                        PrintMenuHelp();
                        break;
                    case 5:
                        running = false;
                        Console.WriteLine("Exiting program.");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please select a valid option.");
                        break;
                }
            }
        }

        /// <summary>
        /// This method calls the Gemini API service to generate content based on the provided prompt.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="prompt"></param>
        /// <returns></returns>
        public static async Task<string> geminiApiService(string apiKey, string prompt)
        {
            try
            {
                var geminiService = new GeminiApiService(apiKey);
                string response = await geminiService.GenerateContentAsync(prompt);

                //Console.WriteLine("Gemini API Response:");
                //Console.WriteLine(response);

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calling Gemini API: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Prints the main menu.
        /// </summary>
        public static void PrintMenu()
        {
            Console.WriteLine("\n--- IDF Operation Menu ---");
            Console.WriteLine("0. Intel Analysis (Find most reported terrorist)");
            Console.WriteLine("1. Show Available Strike Options");
            Console.WriteLine("2. Choose Target (Most dangerous terrorist)");
            Console.WriteLine("3. Execute Strike (on most dangerous terrorist)");
            Console.WriteLine("4. Help");
            Console.WriteLine("5. Exit");
        }

        /// <summary>
        /// Prints help information for the menu.
        /// </summary>
        public static void PrintMenuHelp()
        {
            Console.WriteLine("\nHelp Menu:");
            Console.WriteLine("0: Analyze intelligence reports to find the most reported terrorist.");
            Console.WriteLine("1: List all available strike options.");
            Console.WriteLine("2: Identify the most dangerous terrorist (highest score, alive).");
            Console.WriteLine("3: Execute a strike on the most dangerous terrorist.");
            Console.WriteLine("4: Show this help menu.");
            Console.WriteLine("5: Exit the program.");
        }

        /// <summary>
        /// Gets a validated menu choice from the user.
        /// </summary>
        public static int GetMenuChoice(int min, int max)
        {
            while (true)
            {
                Console.Write("Enter your choice: ");
                var input = Console.ReadLine();
                if (int.TryParse(input, out int choice) && choice >= min && choice <= max)
                    return choice;
                Console.WriteLine($"Invalid input. Please enter a number between {min} and {max}.");
            }
        }

        /// <summary>
        /// Analyzes Aman reports to find the most reported terrorist.
        /// </summary>
        public static Terrorist intelAnalysis(List<Aman> reports)
        {
            var terroristScores = new Dictionary<Terrorist, int>();
            foreach (var report in reports)
            {
                var terrorist = report.GetTerrorist();
                if (terrorist != null)
                {
                    if (terroristScores.ContainsKey(terrorist))
                        terroristScores[terrorist]++;
                    else
                        terroristScores[terrorist] = 1;
                }
            }
            return terroristScores.OrderByDescending(kvp => kvp.Value).FirstOrDefault().Key;
        }

        /// <summary>
        /// Returns a list of available strike options for the IDF.
        /// </summary>
        public static List<StrikeOptions.StrikeOptions> strikeAvailability(Idf idf)
        {
            var idfStrikeOptions = idf.GetStrikeOptions();
            var availableUnits = new List<StrikeOptions.StrikeOptions>();
            foreach (var option in idfStrikeOptions)
            {
                if (option.isAvailable())
                    availableUnits.Add(option);
            }
            return availableUnits;
        }

        /// <summary>
        /// Chooses the most dangerous (highest score, alive) terrorist from Hamas.
        /// </summary>
        public static Terrorist chooseTarget(Hamas.Hamas hamas)
        {
            Terrorist? mostDangerousTerrorist = null;
            var terrorists = hamas.GetTerrorists();
            int maxTerroristScore = 0;
            foreach (var terrorist in terrorists)
            {
                if (terrorist.getAlive())
                {
                    int temp = terrorist.getScore();
                    if (temp > maxTerroristScore)
                    {
                        maxTerroristScore = temp;
                        mostDangerousTerrorist = terrorist;
                    }
                }
            }
            return mostDangerousTerrorist ?? throw new InvalidOperationException("No terrorists found.");
        }

        /// <summary>
        /// Executes a strike on the specified terrorist using the latest intelligence.
        /// </summary>
        public static void strikeExecution(Terrorist terrorist, List<Aman> intelligence, Idf idf)
        {
            var latestReport = intelligence
                .Where(report => report.GetTerrorist() != null && report.GetTerrorist() == terrorist)
                .OrderByDescending(report => report.GetTimestamp())
                .FirstOrDefault();

            if (latestReport == null)
            {
                Console.WriteLine("No valid intelligence report found for the specified terrorist. Strike execution aborted.");
                return;
            }

            var strikeOptions = idf.GetStrikeOptions();
            StrikeOptions.StrikeOptions? usedStrikeOption = null;
            string? location = latestReport.getLocation();
            if (location == null)
            {
                Console.WriteLine("No valid location found in the latest report. Strike execution aborted.");
                return;
            }

            switch (location)
            {
                case "Building":
                    usedStrikeOption = strikeOptions.FirstOrDefault(opt => opt.getName() == "F-16");
                    if (usedStrikeOption != null)
                    {
                        Console.WriteLine($"Strike executed at: {DateTime.Now}\n" +
                            $"Target: {latestReport.GetTerrorist()}\n" +
                            "Ammunition used: F16 1 ton bomb\n" +
                            "Officer name: Mark Salzberg\n" +
                            $"Latest intelligence: {latestReport}");
                        terrorist.setAliveToDeath();
                    }
                    break;
                case "People":
                case "Vehicle":
                    usedStrikeOption = strikeOptions.FirstOrDefault(opt => opt.getName() == "Zik");
                    if (usedStrikeOption != null)
                    {
                        Console.WriteLine($"Strike executed at: {DateTime.Now}\n" +
                            $"Target: {latestReport.GetTerrorist()}\n" +
                            "Ammunition used: Zik missile\n" +
                            "Officer name: Mark Salzberg\n" +
                            $"Latest intelligence: {latestReport}");
                        terrorist.setAliveToDeath();
                    }
                    break;
                case "Open area":
                    usedStrikeOption = strikeOptions.FirstOrDefault(opt => opt.getName() == "M109");
                    if (usedStrikeOption != null)
                    {
                        Console.WriteLine($"Strike executed at: {DateTime.Now}\n" +
                            $"Target: {latestReport.GetTerrorist()}\n" +
                            "Ammunition used: Explosive shells\n" +
                            "Officer name: Mark Salzberg\n" +
                            $"Latest intelligence: {latestReport}");
                        terrorist.setAliveToDeath();
                    }
                    break;
                default:
                    Console.WriteLine("No attack was executed, location unknown");
                    break;
            }

            if (usedStrikeOption != null)
            {
                usedStrikeOption.updateCapacity();
            }
        }

        /// <summary>
        /// Generates a list of random terrorists and Aman reports.
        /// </summary>
        public static async Task<(List<Terrorist> Terrorists, List<Aman> AmanReports)> generateTerroristAmanList(string apiKey)
        {
            string prompt = @"
                Generate a fictional intelligence report for a simulation. 

                1. Create a list of 20 **fictional terrorists** (not real people), each represented as:
                - name: a unique, fake arabic name
                - rank: an integer from 1 to 5 (5 is most dangerous)
                - alive: true or false
                - weapons: a list of 1–4  weapons from this list: [Knife, Gun, M16, AK47]

                2. Generate 60 Aman intelligence report with:
                - terrorist: the name of the terrorist from above
                - location: from this list: Building, People, Open area, Vehicle
                - timestamp: a randomly generated date and time in the last 30 days
                  A terrorist can have multiple aman reports

                Format the data clearly in json format so it can be parsed into objects of the following classes:

                class Terrorist {
                string Name;
                int Rank;
                bool Alive;
                List<string> Weapons;
                }

                class Aman {
                Terrorist Terrorist;
                string Location;
                DateTime Timestamp;
                }

                Note: This is for simulation and software testing purposes only.";

            string response = await geminiApiService(apiKey, prompt);

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            RootDTO? root = JsonSerializer.Deserialize<RootDTO>(response, options);

            // Convert DTOs to actual Terrorist objects
            Dictionary<string, Terrorist> terroristMap = new Dictionary<string, Terrorist>();
            List<Terrorist> terroristsList = new List<Terrorist>();
            List<Aman> amanReportsList = new List<Aman>();
            if (root != null)
            {
                foreach (var dto in root.terrorists)
                {
                    var terrorist = new Terrorist(dto.name, dto.rank, dto.alive, dto.weapons);
                    terroristsList.Add(terrorist);
                    terroristMap[dto.name] = terrorist;
                }
                
                foreach (var report in root.aman_reports)
                {
                    terroristMap.TryGetValue(report.terrorist, out var terrorist);
                    var aman = new Aman(terrorist, report.location, report.timestamp);
                    amanReportsList.Add(aman);
                }
            }

            return (terroristsList, amanReportsList);
        }

        /// <summary>
        /// Generates a list of example strike options.
        /// </summary>
        public static List<StrikeOptions.StrikeOptions> generateStrikeOptions()
        {
            return new List<StrikeOptions.StrikeOptions>
            {
                new F16("F-16", 8, 500.0, new List<string> { "Building" }, "1 Ton"),
                new Zik("Zik", 3, 100.0, new List<string> { "People", "Vehicle" }),
                new M109("M109", 40, 50.0, new List<string> { "Open area" }, "Explosive shells")
            };
        }
    }
}
