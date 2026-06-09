using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    internal class Pot
    {

        public int Total { get; private set; }


        public Pot(int total)
        {
            Total = total;
        }


        public void AddToPot(int amount)
        {
            Total += amount;
        }


        public void resetPot()
        {
            Total = 0;
        }
    }
}