using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    internal class Table
    {
        public List<Card> CommunityCards { get; private set; }
        public int Pot { get; private set; }

        public Table()
        {
            CommunityCards = new List<Card>();
            Pot = 0;
        }
        public void ResetTable()
        {
            CommunityCards.Clear();
            Pot = 0;
        }
        public void DealFlop(Deck deck)
        {
            for (int i = 0; i < 3; i++)
            {
                CommunityCards.Add(deck.DrawCard());
            }
        }
        public void ShowCommunityCards()
        {
            Console.WriteLine("Community Cards:");
            foreach (var card in CommunityCards)
            {
                Console.Write("  [");
                Console.Write(card.getRankSymbol());
                Console.ForegroundColor = card.GetForegroundColor();
                Console.Write(card.getSuitSymbol());
                Console.ResetColor();
                Console.WriteLine("]");
            }
        }
        public void DealTurnOrRiver(Deck deck)
        {
            CommunityCards.Add(deck.DrawCard());
        }

    }

}
