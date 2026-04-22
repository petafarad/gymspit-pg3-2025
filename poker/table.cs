using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    internal class table
    {
        public List<Card> CommunityCards { get; private set; }
        public int Pot { get; private set; }

        public table()
        {
            CommunityCards = new List<Card>();
            Pot = 0;
        }
        public void ResetTable()
        {
            CommunityCards.Clear();
            Pot = 0;
        }
        public void dealFlop(Deck deck)
        {
            for (int i = 0; i < 3; i++)
            {
                CommunityCards.Add(deck.DrawCard());
            }
        }

    }

}
