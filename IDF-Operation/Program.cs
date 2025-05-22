using System.Security.Cryptography.X509Certificates;
using IDF_Operation.Hamas;
using IDF_Operation.IDF;
using IDF_Operation.StrikeOptions;

namespace IDF_Operation
{
    internal class Program
    {
        static List<string> weapons = ["Knife", "Gun", "M16", "AK47"];

        static void Main(string[] args)
        {
            // intialize the IDF
            List<StrikeOptions.StrikeOptions> strikeOptions = generateStrikeOptions();
            Idf idf = new Idf("Eyal Zamir", strikeOptions);

            // initialize the Hamas
            Terrorist commander = new Terrorist("El-Arory", 5, true, weapons);
            Hamas.Hamas hamas = new Hamas.Hamas(commander);
            List<Terrorist> terrorists = generateTerroristList();
            hamas.addTeroristList(terrorists);
        }

        public static List<Terrorist> generateTerroristList()
        {
            Random rnd = new Random();
            List<Terrorist> terorists = new List<Terrorist>();
            List<string> names = new List<string> {
                "Ahmed Al-Masri",
                "Khaled Barakat",
                "Youssef Al-Qassem",
                "Mohammed Darwish",
                "Samir Al-Haddad",
                "Omar Nasser",
                "Bassam Jaber",
                "Tariq Al-Amin",
                "Nabil Farhat",
                "Fadi Khoury",
                "Majed Abu Salah",
                "Hassan Al-Zein",
                "Rami Suleiman",
                "Ibrahim Al-Atrash",
                "Adnan Marwan",
                "Wael Khalifa",
                "Amjad Al-Rawi",
                "Ziad Abu Hatem",
                "Salim Mansour",
                "Jamal Al-Tayeb"
            };

           
            for (int i = 0; i < 20; i++)
           
            {
                List<string> shuffledWeapons = weapons.OrderBy(w => rnd.Next()).ToList();
                List<string> teroristWeapons = new List<string>(shuffledWeapons.Take(rnd.Next(1,5)).ToList());
                Terrorist currentTerorist = new Terrorist(names[i], rnd.Next(1,5), true, teroristWeapons);
                terorists.Add(currentTerorist);

            }

            return terorists;
        }

        public static List<StrikeOptions.StrikeOptions> generateStrikeOptions()
        {
            // Example strike options
            var options = new List<StrikeOptions.StrikeOptions>
            {
                new F16("F-16", 8, 500.0, new List<string> { "building" }, "1 Ton"),
                new Zik("Zik", 3, 100.0, new List<string> { "people", "vehicles" }),
                new M109("M109", 40, 50.0, new List<string> { "open areas" }, "Explosive shells")
            };
            return options;
        }



    }
}
    

