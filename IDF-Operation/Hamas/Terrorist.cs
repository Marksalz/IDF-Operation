using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace IDF_Operation.Hamas
{
    internal class Terrorist
    {
        private string name;
        private int rank;
        private bool aleve;
        private List<string> weapons;

        public Terrorist(string name, int rank, bool aleve, List<string> weapons)
        {
            this.name = name;
            this.rank = rank;
            this.aleve = aleve;
            this.weapons = weapons;
        }

        public string getName()
        { 
            return this.name; 
        }    
        public int getRank()
        {
            return this.rank;
        }

        public bool getAleve()
        {
            return this.aleve;
        }

        public void setAleveToDeath()
        {
            this.aleve = false;
        }

        public List<string> getWeapons()
        {
            return this.weapons;
        }

        public int getScore()
        {
            int weaponsScore = 0;
            foreach (var weapon in this.weapons)
            {
                switch (weapon) {
                    case "Knife":
                        weaponsScore += 1;
                        break;
                    case "Gun":
                        weaponsScore += 2;
                        break;
                    default:
                        weaponsScore += 3;
                        break;
                }
            }
            return this.rank * weaponsScore;
        }


        public override string ToString()
        {
            return $"Name: {this.name}, Rank: {this.rank}, Is aleve: {this.aleve}, Weapons: {string.Join(",", this.weapons)}";
        }



    }
}
