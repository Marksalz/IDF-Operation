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
                terrorist.ToString();
            }

            
        }

        public static List<Terrorist> generateTerroristList()
        {
            Random rnd = new Random();
            List<Terrorist> terorist = new List<Terrorist>();
            List<String> nams = new List<string> {
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
                List<String> weaponsAvailabol = new List<string>(weapons);
                List<String> teroristWeapons = new List<string>();

                for (int j = 0; j < rnd.Next(1, 4); j++)
                {
                    int randomIndex = rnd.Next(weaponsAvailabol.Count);
                    teroristWeapons.Add(weaponsAvailabol[randomIndex]);
                    teroristWeapons.RemoveAt(randomIndex);
                }
                Terrorist currentTerorist = new Terrorist(nams[i], rnd.Next(1,5), true, teroristWeapons);
                nams.RemoveAt(i);
                terorist.Add(currentTerorist);
            }

            return terorist;


        }
    }
}
    

