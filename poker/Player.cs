using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    internal class Player
    {
        public string Name { get; }
        public int Chips { get; private set; }
        public List<Card> Hand { get; }
        public bool IsFolded { get; private set; }
        public int BetThisRound { get; private set; }

        public Player(string name, int StartChips)
        {
            Name = name;
            Chips = StartChips;
            Hand = new List<Card>();
            IsFolded = false;
            BetThisRound = 0;
        }

        public void ReceiveCard(Card card)
        {
            Hand.Add(card);
        }

        public void ShowHand()
        {
            Console.WriteLine($"{Name}'s hand:");
            foreach (var card in Hand)
            {
                Console.Write("  [");
                Console.Write(card.getRankSymbol());
                Console.ForegroundColor = card.GetForegroundColor();
                Console.Write(card.getSuitSymbol());
                Console.ResetColor();
                Console.WriteLine("]");
            }
        }

        public int Bet(int amount)
        {
            if (amount > Chips)
                amount = Chips;
            Chips -= amount;
            BetThisRound += amount;
            return amount;
        }

        public void Fold()
        {
            IsFolded = true;
        }

        public void WinPot(int amount)
        {
            Chips += amount;
        }

        public void ResetForNewRound()
        {
            Hand.Clear();
            IsFolded = false;
            BetThisRound = 0;
        }
    }
}
