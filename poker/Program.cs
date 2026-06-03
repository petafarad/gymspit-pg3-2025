using Poker;
using System;
using System.Linq;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Player player1 = new Player("Tom", 1000);
Player player2 = new Player("Mary", 1000);
Table table = new Table();
Pot pot = new Pot(0);
int roundNumber = 1;
bool continuePlaying = true;

while (continuePlaying)
{
    Console.WriteLine($"\n--- Round {roundNumber} ---");
    player1.ResetForNewRound();
    player2.ResetForNewRound();
    table.ResetTable();
    pot.resetPot();
    Deck deck = new Deck();
    deck.Shuffle();

    for (int i = 0; i < 2; i++)
    {
        player1.ReceiveCard(deck.DrawCard());
        player2.ReceiveCard(deck.DrawCard());
    }
    player1.ShowHand();
    

    Console.Write(player1.Name + ", chose your action ");
    Console.Write("\n1 - check \n2 - bet \n3 - fold\n");
    string? choice1 = Console.ReadLine();
    switch(choice1)
    {
        case "1":
            Console.WriteLine($"{player1.Name} checks.");
            break;
        case "2":
            int betAmount1 = 100; // fixed bet amount for simplicity
            pot.AddToPot(player1.Bet(betAmount1));
            Console.WriteLine($"{player1.Name} bets {betAmount1} chips.");
            break;
        case "3":
            player1.Fold();
            Console.WriteLine($"{player1.Name} folds. {player2.Name} wins the pot of {pot.Total} chips.");
            roundNumber++;
            continue;
        default:
            Console.WriteLine("Invalid choice, treating as check.");
            break;
    }
    player2.ShowHand();

    Console.Write(player2.Name + ", chose your action ");
    Console.Write("\n1 - check \n2 - bet \n3 - fold\n");
    string? choice2 = Console.ReadLine();
    switch (choice2)
    {
        case "1":
            Console.WriteLine($"{player2.Name} checks.");
            break;
        case "2":
            int betAmount1 = 100; // fixed bet amount for simplicity
            pot.AddToPot(player2.Bet(betAmount1));
            Console.WriteLine($"{player2.Name} bets {betAmount1} chips.");
            break;
        case "3":
            player1.Fold();
            Console.WriteLine($"{player2.Name} folds. {player1.Name} wins the pot of {pot.Total} chips.");
            roundNumber++;
            continue;
        default:
            Console.WriteLine("Invalid choice, treating as check.");
            break;
    }

    table.DealFlop(deck);
    table.ShowCommunityCards();

    Console.Write(player1.Name + ", chose your action ");
    Console.Write("\n1 - check \n2 - bet \n3 - fold\n");
    string? choice11 = Console.ReadLine();
    switch (choice1)
    {
        case "1":
            Console.WriteLine($"{player1.Name} checks.");
            break;
        case "2":
            int betAmount1 = 100; // fixed bet amount for simplicity
            pot.AddToPot(player1.Bet(betAmount1));
            Console.WriteLine($"{player1.Name} bets {betAmount1} chips.");
            break;
        case "3":
            player1.Fold();
            Console.WriteLine($"{player1.Name} folds. {player2.Name} wins the pot of {pot.Total} chips.");
            roundNumber++;
            continue;
        default:
            Console.WriteLine("Invalid choice, treating as check.");
            break;
    }

    Console.Write(player2.Name + ", chose your action ");
    Console.Write("\n1 - check \n2 - bet \n3 - fold\n");
    string? choice22 = Console.ReadLine();
    switch (choice2)
    {
        case "1":
            Console.WriteLine($"{player2.Name} checks.");
            break;
        case "2":
            int betAmount1 = 100; // fixed bet amount for simplicity
            pot.AddToPot(player2.Bet(betAmount1));
            Console.WriteLine($"{player2.Name} bets {betAmount1} chips.");
            break;
        case "3":
            player1.Fold();
            Console.WriteLine($"{player2.Name} folds. {player1.Name} wins the pot of {pot.Total} chips.");
            roundNumber++;
            continue;
        default:
            Console.WriteLine("Invalid choice, treating as check.");
            break;
    }

    table.DealTurnOrRiver(deck);//turn
    table.ShowCommunityCards();

    Console.Write(player1.Name + ", chose your action ");
    Console.Write("\n1 - check \n2 - bet \n3 - fold\n");
    string? choice111 = Console.ReadLine();
    switch (choice1)
    {
        case "1":
            Console.WriteLine($"{player1.Name} checks.");
            break;
        case "2":
            int betAmount1 = 100; // fixed bet amount for simplicity
            pot.AddToPot(player1.Bet(betAmount1));
            Console.WriteLine($"{player1.Name} bets {betAmount1} chips.");
            break;
        case "3":
            player1.Fold();
            Console.WriteLine($"{player1.Name} folds. {player2.Name} wins the pot of {pot.Total} chips.");
            roundNumber++;
            continue;
        default:
            Console.WriteLine("Invalid choice, treating as check.");
            break;
    }
    Console.Write(player2.Name + ", chose your action ");
    Console.Write("\n1 - check \n2 - bet \n3 - fold\n");
    string? choice222 = Console.ReadLine();
    switch (choice2)
    {
        case "1":
            Console.WriteLine($"{player2.Name} checks.");
            break;
        case "2":
            int betAmount1 = 100; // fixed bet amount for simplicity
            pot.AddToPot(player1.Bet(betAmount1));
            Console.WriteLine($"{player1.Name} bets {betAmount1} chips.");
            break;
        case "3":
            player1.Fold();
            Console.WriteLine($"{player2.Name} folds. {player1.Name} wins the pot of {pot.Total} chips.");
            roundNumber++;
            continue;
        default:
            Console.WriteLine("Invalid choice, treating as check.");
            break;
    }
    table.DealTurnOrRiver(deck);//river
    table.ShowCommunityCards();
    Console.Write(player1.Name + ", chose your action ");
    Console.Write("\n1 - check \n2 - bet \n3 - fold\n");
    string? choice1111 = Console.ReadLine();
    switch (choice1)
    {
        case "1":
            Console.WriteLine($"{player1.Name} checks.");
            break;
        case "2":
            int betAmount1 = 100; // fixed bet amount for simplicity
            pot.AddToPot(player1.Bet(betAmount1));
            Console.WriteLine($"{player1.Name} bets {betAmount1} chips.");
            break;
        case "3":
            player1.Fold();
            Console.WriteLine($"{player1.Name} folds. {player2.Name} wins the pot of {pot.Total} chips.");
            roundNumber++;
            continue;
        default:
            Console.WriteLine("Invalid choice, treating as check.");
            break;
    }

    Console.Write(player2.Name + ", chose your action ");
    Console.Write("\n1 - check \n2 - bet \n3 - fold\n");
    string? choice2222 = Console.ReadLine();
    switch (choice2)
    {
        case "1":
            Console.WriteLine($"{player2.Name} checks.");
            break;
        case "2":
            int betAmount1 = 100; // fixed bet amount for simplicity
            pot.AddToPot(player1.Bet(betAmount1));
            Console.WriteLine($"{player1.Name} bets {betAmount1} chips.");
            break;
        case "3":
            player1.Fold();
            Console.WriteLine($"{player2.Name} folds. {player1.Name} wins the pot of {pot.Total} chips.");
            roundNumber++;
            continue;
        default:
            Console.WriteLine("Invalid choice, treating as check.");
            break;
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
        Console.WriteLine($"Hráč {player1.Name} vyhrál kolo s kombinací {eval1.Rank} a vyhrává {pot.Total}.");
       player1.WinPot(pot.Total);
    }
    else if (cmp < 0)
    {
        Console.WriteLine($"Hráč {player2.Name} vyhrál kolo s kombinací {eval2.Rank} a vyhrává {pot.Total}.");
        player2.WinPot(pot.Total);
    }
    else
    {
        Console.WriteLine($"Remíza: oba hráči mají kombinaci {eval1.Rank} a rozdeluji si {pot.Total}.");
        player1.WinPot(pot.Total/2);
        player1.WinPot(pot.Total/2);
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
        Console.WriteLine(player1.Name + " má " + player1.Chips + " žetonů, " + player2.Name + " má " + player2.Chips + " žetonů.");
    }
}