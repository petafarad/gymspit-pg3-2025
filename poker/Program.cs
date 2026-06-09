using Poker;
using System;
using System.Linq;
using System.Collections.Generic;

Console.OutputEncoding = System.Text.Encoding.UTF8;

int numPlayers = 0;
while (true)
{
    Console.Write("Enter number of players (2-10): ");
    string? input = Console.ReadLine();
    if (int.TryParse(input, out numPlayers) && numPlayers >= 2 && numPlayers <= 10)
    {
        break;
    }
    Console.WriteLine("Error: Invalid input, please enter a valid number between 2 and 10.");
}

List<Player> players = new List<Player>();
for (int i = 1; i <= numPlayers; i++)
{
    Console.Write($"Enter name for Player {i}: ");
    string? name = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(name))
    {
        name = $"Player {i}";
    }
    players.Add(new Player(name, 1000));
}

Table table = new Table();
Pot pot = new Pot(0);
int roundNumber = 1;
bool continuePlaying = true;

void PassTurn(string nextPlayerName)
{
    Console.WriteLine("\nPress Enter to end your turn and hide your cards...");
    Console.ReadLine();
    Console.Clear();
    Console.WriteLine($"\n--- Passing turn to: {nextPlayerName} ---");
    Console.WriteLine("Press Enter when you are ready and no one else is looking.");
    Console.ReadLine();
    Console.Clear();
}

bool RunBettingRound(List<Player> activePlayers, Table currentTable, Pot currentPot, bool isPreFlop = false)
{
    if (!isPreFlop)
    {
        foreach (var p in activePlayers)
        {
            p.ResetBetThisRound();
        }
    }

    int currentHighestBet = activePlayers.Count > 0 ? activePlayers.Max(p => p.BetThisRound) : 0;

    bool bettingFinished = false;
    int playersActed = 0;

    while (!bettingFinished)
    {
        bettingFinished = true;

        foreach (var player in activePlayers)
        {
            if (player.IsFolded || player.Chips == 0) continue;

            if (player.BetThisRound < currentHighestBet || playersActed < activePlayers.Count(p => !p.IsFolded && p.Chips > 0))
            {
                playersActed++;
                PassTurn(player.Name);

                Console.WriteLine($"\n--- {player.Name}'s turn (Chips: {player.Chips}) ---");
                Console.WriteLine($"Pot size: {currentPot.Total}");
                currentTable.ShowCommunityCards();

                Console.WriteLine("\n--- Player Statuses ---");
                foreach (var p in activePlayers)
                {
                    if (p.Chips == 0 && !p.IsFolded)
                    {
                        Console.WriteLine($"{p.Name}: All-in (Bet: {p.BetThisRound})");
                    }
                    else if (p.IsFolded)
                    {
                        Console.WriteLine($"{p.Name}: Folded");
                    }
                    else
                    {
                        Console.WriteLine($"{p.Name}: {p.LastAction} (Bet: {p.BetThisRound})");
                    }
                }
                Console.WriteLine("-----------------------\n");

                player.ShowHand();

                int amountToCall = currentHighestBet - player.BetThisRound;

                while (true)
                {
                    if (currentHighestBet == 0)
                    {
                        Console.WriteLine("\nOptions:");
                        Console.WriteLine("1 - Check");
                        Console.WriteLine("2 - Bet");
                        Console.WriteLine("3 - Fold");
                    }
                    else
                    {
                        Console.WriteLine($"\nCurrent highest bet is {currentHighestBet}. You need {amountToCall} to call.");
                        Console.WriteLine("Options:");
                        Console.WriteLine($"1 - Call ({amountToCall} chips)");
                        Console.WriteLine("2 - Raise");
                        Console.WriteLine("3 - Fold");
                    }

                    Console.Write("Enter your choice (1/2/3): ");
                    string? choice = Console.ReadLine();

                    if (choice == "1") // Check / Call
                    {
                        if (currentHighestBet == 0)
                        {
                            player.LastAction = "Check";
                            Console.WriteLine($"{player.Name} checks.");
                        }
                        else
                        {
                            int callAmount = player.Bet(amountToCall);
                            currentPot.AddToPot(callAmount);
                            player.LastAction = $"Call {callAmount}";
                            Console.WriteLine($"{player.Name} calls {callAmount}.");
                        }
                        break;
                    }
                    else if (choice == "2") // Bet / Raise
                    {
                        int minTotalBet = currentHighestBet + 1;


                        if (player.Chips + player.BetThisRound <= currentHighestBet)
                        {
                            Console.WriteLine("Error: You don't have enough chips to raise. You can only Call or Fold.");
                            continue;
                        }

                        int desiredTotalBet = 0;
                        while (true)
                        {
                            Console.Write($"Enter total amount you want to commit to the pot this round (Min: {minTotalBet}, Max: {player.Chips + player.BetThisRound}): ");
                            string? betInput = Console.ReadLine();

                            if (int.TryParse(betInput, out desiredTotalBet))
                            {
                                if (desiredTotalBet < minTotalBet)
                                {
                                    Console.WriteLine($"Error: You must bet at least {minTotalBet}.");
                                }
                                else if (desiredTotalBet > player.Chips + player.BetThisRound)
                                {
                                    Console.WriteLine("Error: You don't have that many chips.");
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Error: Invalid input. Please enter a valid number.");
                            }
                        }

                        int actualAdded = player.Bet(desiredTotalBet - player.BetThisRound);
                        currentPot.AddToPot(actualAdded);
                        currentHighestBet = player.BetThisRound; /


                        if (amountToCall == 0)
                        {
                            player.LastAction = $"Bet {actualAdded}";
                            Console.WriteLine($"{player.Name} bets {actualAdded}.");
                        }
                        else
                        {
                            player.LastAction = $"Raise to {currentHighestBet}";
                            Console.WriteLine($"{player.Name} raises to {currentHighestBet} (added {actualAdded}).");
                        }

                        bettingFinished = false;
                        break;
                    }
                    else if (choice == "3") // Fold
                    {
                        player.Fold();
                        player.LastAction = "Fold";
                        Console.WriteLine($"{player.Name} folds.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Error: Invalid choice, please enter 1, 2, or 3.");
                    }
                }

                int activeCount = activePlayers.Count(p => !p.IsFolded);
                if (activeCount <= 1)
                {
                    return false;
                }
            }
        }
    }

    return true;
}


while (continuePlaying)
{
    Console.Clear();
    Console.WriteLine($"\n====== ROUND {roundNumber} ======");


    foreach (var p in players)
    {
        p.ResetForNewRound();
    }
    table.ResetTable();
    pot.resetPot();


    var playersInGame = players.Where(p => p.Chips > 0).ToList();

    if (playersInGame.Count < 2)
    {
        Console.WriteLine("Not enough players with chips left. Game over!");
        break;
    }

    Deck deck = new Deck();
    deck.Shuffle();


    foreach (var p in playersInGame)
    {
        p.ReceiveCard(deck.DrawCard());
        p.ReceiveCard(deck.DrawCard());
    }


    foreach (var p in playersInGame)
    {
        p.ResetBetThisRound();
    }


    int blindAmount = 10;
    int blindPlayerIndex = (roundNumber - 1) % playersInGame.Count;
    Player blindPlayer = playersInGame[blindPlayerIndex];
    int actualBlind = blindPlayer.Bet(blindAmount);
    pot.AddToPot(actualBlind);
    blindPlayer.LastAction = $"Blind {actualBlind}";
    Console.WriteLine($"{blindPlayer.Name} posts a blind of {actualBlind} chips.");
    Console.WriteLine("Press Enter to continue to the betting round...");
    Console.ReadLine();


    bool continueRound = RunBettingRound(playersInGame, table, pot, true);


    if (continueRound)
    {
        table.DealFlop(deck);
        continueRound = RunBettingRound(playersInGame, table, pot);
    }


    if (continueRound)
    {
        table.DealTurnOrRiver(deck);
        continueRound = RunBettingRound(playersInGame, table, pot);
    }

    if (continueRound)
    {
        table.DealTurnOrRiver(deck);
        continueRound = RunBettingRound(playersInGame, table, pot);
    }


    Console.Clear();
    Console.WriteLine("\n====== ROUND RESULTS ======");
    table.ShowCommunityCards();
    Console.WriteLine();

    foreach (var p in playersInGame)
    {
        if (!p.IsFolded)
            p.ShowHand();
    }
    Console.WriteLine();

    int activeCount = playersInGame.Count(p => !p.IsFolded);

    if (activeCount == 1)
    {
        var winner = playersInGame.First(p => !p.IsFolded);
        Console.WriteLine($"Everyone else folded.");
        Console.WriteLine($"{winner.Name} wins the pot of {pot.Total} chips!");
        winner.WinPot(pot.Total);
    }
    else
    {

        var activePlayers = playersInGame.Where(p => !p.IsFolded).ToList();


        var evaluations = new Dictionary<Player, HandEvaluation>();
        foreach (var p in activePlayers)
        {
            var combined = p.Hand.Concat(table.CommunityCards).ToList();
            evaluations[p] = HandEvaluator.Evaluate(combined);
        }

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

        List<Player> winners = new List<Player> { activePlayers[0] };

        for (int i = 1; i < activePlayers.Count; i++)
        {
            Player currentPlayer = activePlayers[i];
            int cmp = CompareEvaluations(evaluations[currentPlayer], evaluations[winners[0]]);

            if (cmp > 0)
            {
                winners.Clear();
                winners.Add(currentPlayer);
            }
            else if (cmp == 0)
            {
                winners.Add(currentPlayer);
            }
        }

        if (winners.Count == 1)
        {
            Player winner = winners[0];
            Console.WriteLine($"{winner.Name} wins the round with {evaluations[winner].Rank} and takes {pot.Total} chips.");
            winner.WinPot(pot.Total);
        }
        else
        {
            Console.WriteLine($"It's a tie between {winners.Count} players! They all have {evaluations[winners[0]].Rank}.");
            int split = pot.Total / winners.Count;
            int remainder = pot.Total % winners.Count;

            foreach (var w in winners)
            {
                w.WinPot(split);
            }

            winners[0].WinPot(remainder);

            Console.WriteLine($"Splitting the pot of {pot.Total} chips.");
        }
    }

    Console.WriteLine($"\nCurrent chips:");
    foreach (var p in players)
    {
        Console.WriteLine($"{p.Name}: {p.Chips}");
    }

    Console.Write("\nPress Enter to play the next round or type 'n' to quit: ");
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