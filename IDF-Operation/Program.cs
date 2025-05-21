using System.Security.Cryptography.X509Certificates;
using IDF_Operation.Hamas;
using IDF_Operation.IDF;

namespace IDF_Operation
{
    internal class Program
    {
        static List<string> weapons = ["Knife", "Gun", "M16", "AK47"];
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
    }
}
    

