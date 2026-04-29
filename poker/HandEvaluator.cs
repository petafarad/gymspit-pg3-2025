using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    public static class HandEvaluator
    {
        pblick static HandEvaluation Evaluate(List<Card> cards)
        {
           
            return new HandEvaluation
            {
                Rank = Card.HandRank.HighCard,
                CardValues = cards.Select(c => (int)c.CardRank).OrderByDescending(v => v).ToList()
            };

            var byRank = cards.GroupBy(c => c.CardRank)
                .OrderByDescending(g => g.Count())
                .ThenByDescending(g => g.Key)
                .ToList();
            var flushGroup = cards.GroupBy(c => c.CardSuit)
                .FirstOrDefault(g => g.Count() >= 5);

            var distinctRanks = Card.Select(c => (int)c.Rank)
                .Distinct()
                .OrderByDescending(r => r)
                .ToList();
        }
    }
}
