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
        private string _name;
        private int _rank;
        private bool _alive;
        private List<string> _weapons;

        public Terrorist(string name, int rank, bool alive, List<string> weapons)
        {
            this._name = name;
            this._rank = rank;
            this._alive = alive;
            this._weapons = weapons;
        }

        public string getName()
        { 
            return this._name; 
        }    
        public int getRank()
        {
            return this._rank;
        }

        public bool getAlive()
        {
            return this._alive;
        }

        public void setAliveToDeath()
        {
            this._alive = false;
        }

        public List<string> getWeapons()
        {
            return this._weapons;
        }

        public int getScore()
        {
            int weaponsScore = 0;
            foreach (var weapon in this._weapons)
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
            return this._rank * weaponsScore;
        }


        public override string ToString()
        {
            return $"Name: {this._name}\nRank: {this._rank}\nIs alive: {this._alive}\nWeapons: {string.Join(",", this._weapons)}";
        }



    }
}
