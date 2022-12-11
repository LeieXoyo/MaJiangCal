namespace MaJiangCal;

public class Card
{
    public int? Number { get; set; }

    public string Symbol { get; set; }

    public string Alias { get; set; }

    public string Id { get; }

    public string Name { get; }

    public static int Amount = 4;

    public Card(string symbol, string alias, int? number = null)
    {
        Symbol = symbol;
        Alias = alias;
        Number = number;
        Id = $"{Number}{Alias}";
        Name = $"{Number}{Symbol}";
    }

    public override string ToString()
    {
        return $"{Name}({Id})";
    }
}