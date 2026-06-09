using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    public class Card
    {
        public enum Suit
        {
            Hearts = 0,
            Diamonds = 1,
            Clubs = 2,
            Spades = 3
        }

        public enum Rank
        {
            Two = 0,
            Three = 1,
            Four = 2,
            Five = 3,
            Six = 4,
            Seven = 5,
            Eight = 6,
            Nine = 7,
            Ten = 8,
            Jack = 9,
            Queen = 10,
            King = 11,
            Ace = 12
        }

        public enum HandRank
        {
            HighCard = 0,
            OnePair = 1,
            TwoPair = 2,
            ThreeOfAKind = 3,
            Straight = 4,
            Flush = 5,
            FullHouse = 6,
            FourOfAKind = 7,
            StraightFlush = 8,
            RoyalFlush = 9
        }

        public Suit CardSuit { get; }
        public Rank CardRank { get; }

        public Card(Suit suit, Rank rank)
        {
            CardSuit = suit;
            CardRank = rank;
        }

        public char getSuitSymbol()
        {
            return CardSuit switch
            {
                Suit.Hearts => '♡',
                Suit.Diamonds => '♢',
                Suit.Clubs => '♧',
                Suit.Spades => '♤',
                _ => '?'
            };
        }

        public string getRankSymbol()
        {
            return CardRank switch
            {
                Rank.Two => "2",
                Rank.Three => "3",
                Rank.Four => "4",
                Rank.Five => "5",
                Rank.Six => "6",
                Rank.Seven => "7",
                Rank.Eight => "8",
                Rank.Nine => "9",
                Rank.Ten => "10",
                Rank.Jack => "J",
                Rank.Queen => "Q",
                Rank.King => "K",
                Rank.Ace => "A",
                _ => "?"
            };
        }

        public ConsoleColor GetForegroundColor()
        {
            return CardSuit switch
            {
                Suit.Hearts => ConsoleColor.Red,
                Suit.Diamonds => ConsoleColor.Red,
                Suit.Clubs => ConsoleColor.Gray,
                Suit.Spades => ConsoleColor.Gray,
                _ => ConsoleColor.White
            };
        }

        public override string ToString()
        {
            return $"[{getRankSymbol()}{getSuitSymbol()}]";
        }
    }
}
