namespace _10000.Utils;

public static class DicePrinter
{
    public static void PrintDices(int[] dices)
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

    public static void InlinePrintDices(int[] dices)
    {
        string[] lines = new string[5];
    }
}