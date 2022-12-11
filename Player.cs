namespace MaJiangCal;

public class Player
{
    public List<Card> HandCards { get; set; } = new List<Card>();

    public List<Card> PengCards { get; set; } = new List<Card>();

    public List<Card> GangCards { get; set; } = new List<Card>();

    public void Peng(Card card)
    {
        if (HandCards.Count(c => c.Id == card.Id) >= 2)
        {
            foreach (var _ in Enumerable.Range(0, 2))
            {
                var c = HandCards.First(c=> c.Id == card.Id);
                HandCards.Remove(c);
            }
            PengCards.Add(card);
        }
        else
        {
            throw new Exception("手牌中没有足够的牌可以碰");
        }
    }

    public void Gang(Card card)
    {
        if (HandCards.Count(c => c.Id == card.Id) >= 3)
        {
            HandCards.RemoveAll(c => c.Id == card.Id);

            GangCards.Add(card);
        }
        else if (PengCards.First(c => c.Id == card.Id) != null)
        {
            PengCards.RemoveAll(c => c.Id == card.Id);

            HandCards.RemoveAll(c => c.Id == card.Id);

            GangCards.Add(card);
        }
        else
        {
            throw new Exception("手牌中没有足够的牌可以杠");
        }
    }

    public void ShowCards()
    {
        Console.WriteLine("手牌为:");
        var handCards = HandCards.OrderBy(c => c.Alias).ThenBy(c => c.Number).ToList();
        var wall = new string('=', handCards.Count * 4);
        Console.WriteLine(wall);
        
        foreach (var card in handCards)
        {
            Console.Write($"{card.Name} ");
        }
        if (PengCards.Count != 0)
        {
            Console.WriteLine("\n碰的牌为:");
            foreach (var card in PengCards)
            {
                Console.Write($"{card.Name} ");
            }
        }
        if (GangCards.Count != 0)
        {
            Console.WriteLine("\n杠的牌为:");
            foreach (var card in GangCards)
            {
                Console.Write($"{card.Name} ");
            }
        }

        Console.WriteLine($"\n{wall}");
    }
}