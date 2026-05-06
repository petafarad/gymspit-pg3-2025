using Poker;
using System;
using System.Linq;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Player player1 = new Player("Tom", 1000);
Player player2 = new Player("Mary", 1000);
Table table = new Table();
int roundNumber = 1;
bool continuePlaying = true;

while (continuePlaying)
{
    Console.WriteLine($"\n--- Round {roundNumber} ---");
    player1.ResetForNewRound();
    player2.ResetForNewRound();
    table.ResetTable();
    Deck deck = new Deck();
    deck.Shuffle();

    for (int i = 0; i < 2; i++)
    {
        player1.ReceiveCard(deck.DrawCard());
        player2.ReceiveCard(deck.DrawCard());
    }
    player1.ShowHand();
    player2.ShowHand();

    table.DealFlop(deck);
    table.ShowCommunityCards();
    for (int i = 0; i < 2; i++)
    {
        table.DealTurnOrRiver(deck);
        table.ShowCommunityCards();
    }

    // vyhodnocení vítěze kola
    var combined1 = player1.Hand.Concat(table.CommunityCards).ToList();
    var combined2 = player2.Hand.Concat(table.CommunityCards).ToList();

    var eval1 = HandEvaluator.Evaluate(combined1);
    var eval2 = HandEvaluator.Evaluate(combined2);

    int CompareEvaluations(HandEvaluation a, HandEvaluation b)
    {
        int r = a.Rank.CompareTo(b.Rank);
        if (r != 0) return r;
        int max = Math.Max(a.CardValues.Count, b.CardValues.Count);
        for (int i = 0; i < max; i++)
        {
            int av = i < a.CardValues.Count ? a.CardValues[i] : 0;
            int bv = i < b.CardValues.Count ? b.CardValues[i] : 0;
            if (av != bv) return av.CompareTo(bv);
        }
        return 0;
    }

    int cmp = CompareEvaluations(eval1, eval2);
    if (cmp > 0)
    {
        Console.WriteLine($"Hráč {player1.Name} vyhrál kolo s kombinací {eval1.Rank}.");
    }
    else if (cmp < 0)
    {
        Console.WriteLine($"Hráč {player2.Name} vyhrál kolo s kombinací {eval2.Rank}.");
    }
    else
    {
        Console.WriteLine($"Remíza: oba hráči mají kombinaci {eval1.Rank}.");
    }

    Console.Write("Stiskněte Enter pro další kolo nebo napište 'n' pro ukončení: ");
    string? answer = Console.ReadLine();
    if (answer != null && answer.Trim().Equals("n", StringComparison.OrdinalIgnoreCase))
    {
        continuePlaying = false;
        Console.WriteLine("Thanks for playing!");
    }
    else
    {
        roundNumber++;
    }
}