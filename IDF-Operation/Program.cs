using System.Security.Cryptography.X509Certificates;
using IDF_Operation.Hamas;
using IDF_Operation.IDF;

namespace IDF_Operation
{
    internal class Program
    {
        static Random rnd = new Random();
        static List<string> weapons = ["Knife", "Gun", "M16", "AK47"];

        static List<string> names = new List<string> {
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
        static void Main(string[] args)
        {
           

            //Idf idf = new Idf("Ben-Gurion");
            Terrorist commander = new Terrorist("El-Arory", 5, true, weapons);
            Hamas.Hamas hamas = new Hamas.Hamas(commander);


            List<Terrorist> terrorists = generateTerroristList();
            hamas.addTeroristList(terrorists);

            foreach (Terrorist terrorist in terrorists)
            {
                Console.WriteLine(terrorist);
            }

            
        }

        public static List<Terrorist> generateTerroristList()
        {
            
            List<Terrorist> terorists = new List<Terrorist>();
           

           
            for (int i = 0; i < 20; i++)
           
            {
                List<string> shuffledWeapons = weapons.OrderBy(w => rnd.Next()).ToList();
                List<string> teroristWeapons = new List<string>(shuffledWeapons.Take(rnd.Next(1,5)).ToList());
                Terrorist currentTerorist = new Terrorist(names[i], rnd.Next(1,5), true, teroristWeapons);
                terorists.Add(currentTerorist);

            }

            return terorists;
        }

        public static List<Aman> generateRandomMessage()
        {
            List<Aman> messages = new List<Aman>();
            string[] locations = { "Building", "Streat", "Open erea", "Vehicle" };
            for (int i = 0; i < 20; i++)
            {
                string name = names[rnd.Next(names.Count)];
                string location = locations[rnd.Next(locations.Length)];
                DateTime time = DateTime.Now;

                Aman message = new Aman(name, location, time);
                messages.Add(message);
            }
           
            return messages;
        }
    }
}
    

