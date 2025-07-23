namespace _10000.Utils;

public static class DicePrinter
{
    public static void PrintDices(IEnumerable<int> dices)
    {
        foreach (int dice in dices)
        {
            PrintDice(dice);
        }
    }

    public static void PrintDice(int dice)
    {
        if(dice < 1  || dice > 6)
            return;
        
        Console.WriteLine("-----");
        switch (dice)
        {
            case 1 :
                Console.WriteLine("|   |");
                Console.WriteLine("| o |");
                Console.WriteLine("|   |");
                break;
            case 2 :
                Console.WriteLine("|o  |");
                Console.WriteLine("|   |");
                Console.WriteLine("|  o|");
                break;
            case 3 :
                Console.WriteLine("|o  |");
                Console.WriteLine("| o |");
                Console.WriteLine("|  o|");
                break;
            case 4 :
                Console.WriteLine("|o o|");
                Console.WriteLine("|   |");
                Console.WriteLine("|o o|");
                break;
            case 5 :
                Console.WriteLine("|o o|");
                Console.WriteLine("| o |");
                Console.WriteLine("|o o|");
                break;
            case 6 :
                Console.WriteLine("|o o|");
                Console.WriteLine("|o o|");
                Console.WriteLine("|o o|");
                break;
        }
        Console.WriteLine("-----");
    }

    public static void InlinePrintDices(IEnumerable<int> dices)
    {
        string[] lines = new string[5];

        foreach (int dice in dices)
        {
            lines = InlinePrintDice(dice,lines);
        }

        foreach (string line in lines)
        {
            Console.WriteLine(line);
        }
    }

    public static string[] InlinePrintDice(int dice, string[] lines)
    {
        lines[0] += "-----";
        lines[4] += "-----";

        switch (dice)
        {
            case 1 :
                lines[1] += "|   |";
                lines[2] += "| o |";
                lines[3] += "|   |";
                break;
            case 2 :
                lines[1] += "|o  |";
                lines[2] += "|   |";
                lines[3] += "|  o|";
                break;
            case 3 :
                lines[1] += "|o  |";
                lines[2] += "| o |";
                lines[3] += "|  o|";
                break;
            case 4 :
                lines[1] += "|o o|";
                lines[2] += "|   |";
                lines[3] += "|o o|";
                break;
            case 5 :
                lines[1] += "|o o|";
                lines[2] += "| o |";
                lines[3] += "|o o|";
                break;
            case 6 :
                lines[1] += "|o o|";
                lines[2] += "|o o|";
                lines[3] += "|o o|";
                break;
        }

        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] += " ";
        }

        return lines;
    }
}