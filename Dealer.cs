using System.Text.RegularExpressions;

namespace MaJiangCal;

public class Dealer
{
    public List<(string, string, int?)> CardNameList = new List<(string, string, int?)>();

    public List<Card> Cards = new List<Card>();

    public void GenerateCards()
    {   
        foreach (var (symbol, alias, number) in CardNameList)
        {
            foreach (var _ in Enumerable.Range(0, Card.Amount))
            {
                Cards.Add(new Card(symbol, alias, number));
            }
        }
    }

    public List<Card> AnalyzeStartingCards(string startingCardsStr)
    {
        var results = new List<Card>();
        var numsList = new List<int>();
        foreach (var letter in startingCardsStr)
        {
            if (Regex.IsMatch(letter.ToString(), @"\d"))
            {
                numsList.Add(int.Parse(letter.ToString()));
            }
            else
            {
                foreach (var i in numsList)
                {
                    var card = Cards.First(c => c.Id == $"{i}{letter}");
                    Cards.Remove(card);
                    results.Add(card);
                }
                numsList.Clear();
            }
        }
        return results;
    }

    public void Peng(Card card)
    {
        foreach (var _ in Enumerable.Range(0, 2))
        {
            Cards.Remove(Cards.First(c => c.Id == card.Id));
        }
    }

    public void Gang(Card card)
    {
        Cards.RemoveAll(c => c.Id == card.Id);
    }

    public void Hint(Player player)
    {
        player.ShowCards();

        for (var i = 0; i < player.HandCards.Count - 1; i++)
        {
            var putOutCard = player.HandCards[i];
            if (i > 0 && player.HandCards[i].Id == player.HandCards[i - 1].Id)
            {
                continue;
            }
            var handCardsWithOutPutOutCards = player.HandCards.Where((c, index) => index != i).ToList();

            var listenedCards = GetListenedCards(handCardsWithOutPutOutCards);

            if (listenedCards.Count != 0)
            {
                Console.WriteLine($"打{putOutCard.Name}, 听 {string.Join(", ", listenedCards.Select(c => $"{c.Number}{c.Symbol}:[{Cards.Count(card => card.Id == c.Id) - SameAsPutOutCard(c, putOutCard)}]"))}");
            }
        }
    }

    private List<Card> GetListenedCards(List<Card> toCalCards)
    {
        var listenedCards = new List<Card>();
        foreach (var (symbol, alias) in new (string, string)[] {("万", "w"), ("条", "t"), ("饼", "b")})
        {
            foreach (var number in Enumerable.Range(1, 9))
            {
                var tryCard = new Card(symbol, alias, number);
                toCalCards.Add(tryCard);
                var toCheckCards = toCalCards.OrderBy(c => c.Alias).ThenBy(c => c.Number).ToList();

                if (IsHu(toCheckCards))
                {
                    listenedCards.Add(tryCard);
                }
                toCalCards.Remove(tryCard);
            }
        }
        return listenedCards;
    }

    private bool IsHu(List<Card> toCheckCards)
    {
        var pairCards = toCheckCards.Where(cw => toCheckCards.Count(cc => cc.Id == cw.Id) >= 2).DistinctBy(cd => cd.Id);
        foreach (var pairCard in pairCards)
        {
            var tempCards = toCheckCards.ToList();

            foreach (var _ in Enumerable.Range(0, 2))
            {
                tempCards.Remove(tempCards.First(c => c.Id == pairCard.Id));
            }

            if (IsFinished(tempCards))
            {
                return true;
            }
        }
        return false;
    }
    
    private bool IsFinished(List<Card> cards)
    {
        var wCards = cards.Where(c => c.Alias == "w").ToList();
        var tCards = cards.Where(c => c.Alias == "t").ToList();
        var bCards = cards.Where(c => c.Alias == "b").ToList();
        return CheckSingleFinished(wCards) && CheckSingleFinished(tCards) && CheckSingleFinished(bCards);
    }

    private bool CheckSingleFinished(List<Card> cards)
    {
        switch (cards.Count)
        {
            case 0:
                return true;
            case 3:
                return (cards[0].Number == cards[1].Number && cards[1].Number == cards[2].Number)
                    ||  (cards[0].Number + 1 == cards[1].Number && cards[1].Number + 1 == cards[2].Number);
            case 6:
                return (CheckSingleFinished(cards.Take(3).ToList()) && CheckSingleFinished(cards.Skip(3).ToList()))
                    || (cards[0].Number == cards[1].Number && cards[1].Number + 1 == cards[2].Number
                        && cards[2].Number == cards[3].Number && cards[3].Number + 1 == cards[4].Number
                        && cards[4].Number == cards[5].Number)
                    || (cards[0].Number + 1 == cards[1].Number && cards[1].Number == cards[2].Number
                        && cards[2].Number + 1 == cards[3].Number && cards[3].Number == cards[4].Number
                        && cards[4].Number + 1 == cards[5].Number);
            case 9:
                return (CheckSingleFinished(cards.Take(3).ToList()) && CheckSingleFinished(cards.Skip(3).ToList())
                    || CheckSingleFinished(cards.Take(6).ToList()) && CheckSingleFinished(cards.Skip(6).ToList()));
            case 12:
                return (CheckSingleFinished(cards.Take(3).ToList()) && CheckSingleFinished(cards.Skip(3).ToList())
                    || CheckSingleFinished(cards.Take(6).ToList()) && CheckSingleFinished(cards.Skip(6).ToList())
                    || CheckSingleFinished(cards.Take(9).ToList()) && CheckSingleFinished(cards.Skip(9).ToList()));
            default:
                return false;
        }
    }

    private int SameAsPutOutCard(Card listenedCard, Card putOutCard)
    {
        if (Cards.Count(c => putOutCard.Id == c.Id) == 0)
        {
            return 0;
        }
        return listenedCard.Id == putOutCard.Id ? 1 : 0;
    }
}