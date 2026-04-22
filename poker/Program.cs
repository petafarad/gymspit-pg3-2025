using Poker;
using System;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Player player1 = new Player("Tom", 1000);
Player player2 = new Player("Mary", 1000);
int roundNumber = 1;
bool continuePlaying = true;

while (continuePlaying)
{
    Console.Clear();
    Console.WriteLine($"\n--- Round {roundNumber} ---");
    player1.ResetForNewRound();
    player2.ResetForNewRound();
    Deck deck = new Deck();
    deck.Shuffle();
    for (int i = 0; i < 2; i++)
    {
        player1.ReceiveCard(deck.DrawCard());
        player2.ReceiveCard(deck.DrawCard());
    }
    player1.ShowHand();
    player2.ShowHand();

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
    Console.Clear();
}