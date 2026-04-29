using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    internal class Card
    {
        public enum Suit
        {
            Hearts,
            Diamonds,
            Clubs,
            Spades
        }

        public enum Rank
        {
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Ten,
            Jack,
            Queen,
            King,
            Ace
        }
        public enum HandRank
        {
            HighCard,
            OnePair,
            TwoPair,
            ThreeOfAKind,
            Straight,
            Flush,
            FullHouse,
            FourOfAKind,
            StraightFlush,
            RoyalFlush
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
