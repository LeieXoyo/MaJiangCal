using MaJiangCal;

var dealer = new Dealer();

foreach (var (symbol, alias) in new (string, string)[] {("万", "w"), ("条", "t"), ("饼", "b")})
{
    foreach (var number in Enumerable.Range(1, 9))
    {
        dealer.CardNameList.Add((symbol, alias, number));
    }
}

dealer.GenerateCards();
var player = new Player();

Console.WriteLine("\n请输入起始牌:");
var startingCardsStr = Console.ReadLine();
player.HandCards = dealer.AnalyzeStartingCards(startingCardsStr!);

while (true)
{
    start:
    Console.WriteLine("\n请输入指令:");
    var command = Console.ReadLine()!;
    if (command.Length != 4)
    {
        Console.WriteLine("指令长度不正确");
        goto start;
    }
    var oprNum = command[0];
    var oprFunc = command[1];
    var oprCardName = command[2..];
    Card inputCard, outputCard, pengCard, gangCard;
    switch (oprFunc)
    {
        case 'm':
            inputCard = dealer.Cards.First(c => c.Id == oprCardName);
            dealer.Cards.Remove(inputCard);
            player.HandCards.Add(inputCard);
            dealer.Hint(player);
            break;
        case 'd':
            switch (oprNum)
            {
                case '1':
                    outputCard = player.HandCards.First(c => c.Id == oprCardName);
                    player.HandCards.Remove(outputCard);
                    break;
                case '3':
                    outputCard = dealer.Cards.First(c => c.Id == oprCardName);
                    dealer.Cards.Remove(outputCard);
                    break;
                default:
                    goto error;
            }
            break;
        case 'p':
            switch (oprNum)
            {
                case '1':
                    pengCard = player.HandCards.First(c => c.Id == oprCardName);
                    player.Peng(pengCard);
                    break;
                case '3':
                    pengCard = dealer.Cards.First(c => c.Id == oprCardName);
                    dealer.Peng(pengCard);
                    break;
                default:
                    goto error;
            }
            break;
        case 'g':
            switch (oprNum)
            {
                case '1':
                    gangCard = player.HandCards.Find(c => c.Id == oprCardName) ?? player.PengCards.First(c => c.Id == oprCardName);
                    player.Gang(gangCard);
                    break;
                case '3':
                    gangCard = dealer.Cards.First(c => c.Id == oprCardName);
                    dealer.Gang(gangCard);
                    break;
                default:
                    goto error;
            }
            break;
        default:
            error:
            Console.WriteLine($"指令[{command}]不正确");
            goto start;
    }
}