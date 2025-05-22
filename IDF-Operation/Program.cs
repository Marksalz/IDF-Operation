using IDF_Operation.Hamas;
using IDF_Operation.IDF;
using IDF_Operation.StrikeOptions;

namespace IDF_Operation
{
    internal class Program
    {
        static Random rnd = new Random();
        static List<string> weapons = ["Knife", "Gun", "M16", "AK47"];

        static List<string> names = new List<string> {
            "Ahmed Al-Masri", "Khaled Barakat", "Youssef Al-Qassem", "Mohammed Darwish",
            "Samir Al-Haddad", "Omar Nasser", "Bassam Jaber", "Tariq Al-Amin",
            "Nabil Farhat", "Fadi Khoury", "Majed Abu Salah", "Hassan Al-Zein",
            "Rami Suleiman", "Ibrahim Al-Atrash", "Adnan Marwan", "Wael Khalifa",
            "Amjad Al-Rawi", "Ziad Abu Hatem", "Salim Mansour", "Jamal Al-Tayeb"
        };

        static void Main(string[] args)
        {
            // Initialize the IDF
            var strikeOptions = generateStrikeOptions();
            var idf = new Idf("Eyal Zamir", strikeOptions);

            // Initialize the Hamas
            var commander = new Terrorist("El-Arory", 5, true, weapons);
            var hamas = new Hamas.Hamas(commander);
            var terrorists = generateTerroristList();
            hamas.addTeroristList(terrorists);

            // Initialize Aman reports
            var amanReports = generateRandomReports(terrorists);

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
        /// Generates a list of random terrorists.
        /// </summary>
        public static List<Terrorist> generateTerroristList()
        {
            var terrorists = new List<Terrorist>();
            for (int i = 0; i < 20; i++)
            {
                var shuffledWeapons = weapons.OrderBy(w => rnd.Next()).ToList();
                var terroristWeapons = new List<string>(shuffledWeapons.Take(rnd.Next(1, 5)).ToList());
                var currentTerrorist = new Terrorist(names[i], rnd.Next(1, 5), true, terroristWeapons);
                terrorists.Add(currentTerrorist);
            }
            return terrorists;
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

        /// <summary>
        /// Generates a list of random Aman intelligence reports.
        /// </summary>
        public static List<Aman> generateRandomReports(List<Terrorist> terrorists)
        {
            var messages = new List<Aman>();
            string[] locations = { "Building", "People", "Open area", "Vehicle" };
            for (int i = 0; i < 50; i++)
            {
                string location = locations[rnd.Next(locations.Length)];
                DateTime time = DateTime.Now.AddMinutes(rnd.Next(-1000, 1000));
                Terrorist? currentTerrorist = terrorists.Count > 0 ? terrorists[rnd.Next(terrorists.Count)] : null;

                if (currentTerrorist != null)
                {
                    var message = new Aman(currentTerrorist, location, time);
                    messages.Add(message);
                }
            }
            return messages;
        }
    }
}
