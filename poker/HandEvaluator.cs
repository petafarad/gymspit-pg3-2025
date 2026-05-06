using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    public static class HandEvaluator
    {
        public static HandEvaluation Evaluate(List<Card> cards)
        {
            if (cards == null) throw new ArgumentNullException(nameof(cards));
            if (cards.Count < 5) throw new ArgumentException("Musí být alespoň 5 karet pro vyhodnocení.", nameof(cards));

            // map enum ranks (Two..Ace) -> values 2..14 (Ace = 14)
            var rankValues = cards.Select(c => (int)c.CardRank + 2).Distinct().OrderByDescending(v => v).ToList();

            // groups by rank
            var byRank = cards.GroupBy(c => (int)c.CardRank)
                .Select(g => new { Rank = g.Key + 2, Count = g.Count(), Cards = g.OrderByDescending(c => (int)c.CardRank).ToList() })
                .OrderByDescending(g => g.Count)
                .ThenByDescending(g => g.Rank)
                .ToList();

            // flush
            var flushGroup = cards.GroupBy(c => c.CardSuit).FirstOrDefault(g => g.Count() >= 5);
            List<Card> flushCards = flushGroup?.OrderByDescending(c => (int)c.CardRank).ToList();

            // helper: find highest straight in a set of rank ints (2..14), support wheel (A-2-3-4-5)
            int? FindStraightHighCard(List<int> distinctRanksDesc)
            {
                if (distinctRanksDesc == null || distinctRanksDesc.Count == 0) return null;
                var ranks = distinctRanksDesc.Distinct().OrderByDescending(x => x).ToList();
                if (ranks.Contains(14) && !ranks.Contains(1)) ranks.Add(1); // Ace as low for wheel
                ranks = ranks.Distinct().OrderByDescending(x => x).ToList();

                int bestHigh = 0;
                int run = 1;
                for (int i = 1; i < ranks.Count; i++)
                {
                    if (ranks[i - 1] - 1 == ranks[i])
                    {
                        run++;
                    }
                    else
                    {
                        run = 1;
                    }
                    if (run >= 5)
                    {
                        bestHigh = Math.Max(bestHigh, ranks[i - 4] == 1 ? 5 : ranks[i - 4 + 0]); // high card of the straight segment
                    }
                }

                // handle case when first 5 consecutive from top (edge when straight starts at index 0)
                if (ranks.Count >= 5)
                {
                    // check windows explicitly to reliably get high card
                    for (int start = 0; start <= ranks.Count - 5; start++)
                    {
                        bool ok = true;
                        for (int j = 0; j < 4; j++)
                        {
                            if (ranks[start + j] - 1 != ranks[start + j + 1]) { ok = false; break; }
                        }
                        if (ok)
                        {
                            int high = ranks[start];
                            if (high == 14 && ranks[start + 4] == 1) high = 5; // wheel -> high is 5
                            bestHigh = Math.Max(bestHigh, high);
                        }
                    }
                }

                return bestHigh == 0 ? (int?)null : bestHigh;
            }

            // detect straight flush / royal flush
            if (flushCards != null)
            {
                var flushRanks = flushCards.Select(c => (int)c.CardRank + 2).Distinct().OrderByDescending(x => x).ToList();
                if (flushRanks.Contains(14)) flushRanks.Add(1);
                var sfHigh = FindStraightHighCard(flushRanks);
                if (sfHigh.HasValue)
                {
                    if (sfHigh.Value == 14) // 10..14 straight flush -> royal
                    {
                        return new HandEvaluation
                        {
                            Rank = Card.HandRank.RoyalFlush,
                            CardValues = new List<int> { 14 } // highest card
                        };
                    }
                    return new HandEvaluation
                    {
                        Rank = Card.HandRank.StraightFlush,
                        CardValues = new List<int> { sfHigh.Value }
                    };
                }
            }

            // Four of a kind
            var four = byRank.FirstOrDefault(g => g.Count == 4);
            if (four != null)
            {
                var kicker = cards.Where(c => (int)c.CardRank + 2 != four.Rank).OrderByDescending(c => (int)c.CardRank).First();
                return new HandEvaluation
                {
                    Rank = Card.HandRank.FourOfAKind,
                    CardValues = new List<int> { four.Rank, (int)kicker.CardRank + 2 }
                };
            }

            // Full house (three + pair) — prefer highest 3 then highest pair
            var three = byRank.Where(g => g.Count == 3).ToList();
            var pairs = byRank.Where(g => g.Count == 2).ToList();
            if (three.Count >= 1 && (pairs.Count >= 1 || three.Count >= 2))
            {
                int threeRank = three.Max(g => g.Rank);
                int pairRank;
                // if another three exists, use second three as pair
                var otherThrees = three.Where(g => g.Rank != threeRank).OrderByDescending(g => g.Rank).ToList();
                if (otherThrees.Any())
                {
                    pairRank = otherThrees.First().Rank;
                }
                else
                {
                    pairRank = pairs.Max(g => g.Rank);
                }
                return new HandEvaluation
                {
                    Rank = Card.HandRank.FullHouse,
                    CardValues = new List<int> { threeRank, pairRank }
                };
            }

            // Flush
            if (flushCards != null)
            {
                var top5 = flushCards.Select(c => (int)c.CardRank + 2).OrderByDescending(v => v).Take(5).ToList();
                return new HandEvaluation
                {
                    Rank = Card.HandRank.Flush,
                    CardValues = top5
                };
            }

            // Straight
            var straightHigh = FindStraightHighCard(rankValues);
            if (straightHigh.HasValue)
            {
                return new HandEvaluation
                {
                    Rank = Card.HandRank.Straight,
                    CardValues = new List<int> { straightHigh.Value }
                };
            }

            // Three of a kind
            if (three.Any())
            {
                int threeRank = three.Max(g => g.Rank);
                var kickers = cards.Where(c => (int)c.CardRank + 2 != threeRank)
                                   .Select(c => (int)c.CardRank + 2)
                                   .OrderByDescending(v => v)
                                   .Take(2)
                                   .ToList();
                var vals = new List<int> { threeRank };
                vals.AddRange(kickers);
                return new HandEvaluation
                {
                    Rank = Card.HandRank.ThreeOfAKind,
                    CardValues = vals
                };
            }

            // Two pair
            if (pairs.Count >= 2)
            {
                var topPairs = pairs.OrderByDescending(p => p.Rank).Take(2).Select(p => p.Rank).ToList();
                var kicker = cards.Where(c => !topPairs.Contains((int)c.CardRank + 2))
                                  .Select(c => (int)c.CardRank + 2)
                                  .OrderByDescending(v => v)
                                  .First();
                var vals = new List<int>(topPairs);
                vals.Add(kicker);
                return new HandEvaluation
                {
                    Rank = Card.HandRank.TwoPair,
                    CardValues = vals
                };
            }

            // One pair
            if (pairs.Count == 1)
            {
                int pairRank = pairs.Max(p => p.Rank);
                var kickers = cards.Where(c => (int)c.CardRank + 2 != pairRank)
                                   .Select(c => (int)c.CardRank + 2)
                                   .OrderByDescending(v => v)
                                   .Take(3)
                                   .ToList();
                var vals = new List<int> { pairRank };
                vals.AddRange(kickers);
                return new HandEvaluation
                {
                    Rank = Card.HandRank.OnePair,
                    CardValues = vals
                };
            }

            // High card
            var highCards = cards.Select(c => (int)c.CardRank + 2).OrderByDescending(v => v).Take(5).ToList();
            return new HandEvaluation
            {
                Rank = Card.HandRank.HighCard,
                CardValues = highCards
            };
        }
    }
}
