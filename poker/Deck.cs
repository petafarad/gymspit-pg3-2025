using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{

    internal class Deck
    {

        private List<Card> cards;


        private static readonly Random rng = new Random();


        public Deck()
        {
            cards = new List<Card>();

            foreach (Card.Suit suit in Enum.GetValues(typeof(Card.Suit)))
            {

                foreach (Card.Rank rank in Enum.GetValues(typeof(Card.Rank)))
                {

                    cards.Add(new Card(suit, rank));
                }
            }
        }


        public void Shuffle()
        {

            for (int i = cards.Count - 1; i > 0; i--)
            {

                int j = rng.Next(i + 1);


                var tmp = cards[i];
                cards[i] = cards[j];
                cards[j] = tmp;
            }
        }


        public Card DrawCard()
        {

            if (cards.Count == 0)
                throw new InvalidOperationException("Deck is empty!");


            Card topCard = cards[0];


            cards.RemoveAt(0);

            return topCard;
        }
    }
}
