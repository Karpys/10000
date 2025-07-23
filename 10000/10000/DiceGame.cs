namespace _10000;

using Utils;

public class DiceGame
{
    private Random m_Random = new Random();
    private int m_Point;
    private List<int> m_Dices;
    private bool m_CanRoll = true;

    public List<int> Dices => m_Dices;

    public DiceGame()
    {
        m_Dices = Enumerable.Repeat(0, 6).ToList();
    }

    public void Reroll()
    {
        m_Dices = Enumerable.Repeat(0, 6).ToList();
        Roll();
    }
    public void Roll()
    {
        if(!m_CanRoll)
            return;

        m_CanRoll = false;

        for (int i = 0; i < m_Dices.Count; i++)
        {
            int result = m_Random.Next(1, 7);
            m_Dices[i] = result;
        }

        PrintDices();
        
        ApplyRuleOfThree();
        AskForAction();
    }

    private void PrintDices()
    {
        DicePrinter.InlinePrintDices(m_Dices);
    }

    private void ApplyRuleOfThree()
    {
        int triple = m_Dices.GroupBy(x => x).Where(g => g.Count() >= 3).Select(g => g.Key).FirstOrDefault();

        if (triple != 0)
        {
            RemoveDices(new List<int>(){triple,triple,triple});
            int mult = triple == 1 ? 1000 : 100;
            UpdateScore(triple * mult);
            PrintDices();
            m_CanRoll = true;
            //Force Roll
        }
    }

    private void UpdateScore(int score)
    {
        m_Point += score;
        Console.WriteLine("New score : " + m_Point);
    }

    private void RemoveDices(List<int> dicesToRemove)
    {
        foreach (int i in dicesToRemove)
        {
            m_Dices.Remove(i);
        }
        
        if(m_Dices.Count == 0)
            Reroll();
    }
    
    private void RemoveDice(int diceToRemove)
    {
        m_Dices.Remove(diceToRemove);
        
        if(m_Dices.Count == 0)
            Reroll();
    }

    private void AskForAction()
    {
        Console.WriteLine("0 : Leave with Current Score | 1 : Pick Dice | 2 : Roll");
        int action = int.Parse(Console.ReadLine());

        switch (action)
        {
            case 0:
                //Save "m_Point"
                break;
            case 1:
                PickDicesAction();
                break;
            case 2:
                TryRollCurrent();
                break;
        }
    }

    private void TryRollCurrent()
    {
        Roll();
    }

    private void PickDicesAction()
    {
        if (!m_Dices.Contains(1) && !m_Dices.Contains(5))
        {
            Console.WriteLine("No pickable Dice, continue");
            AskForAction();
            return;
        }
        
        Console.WriteLine("Pick Dice");
        string action = Console.ReadLine();
        List<int> diceToPick = action.Split().Select(s => int.Parse(s)).ToList();

        foreach (int dice in diceToPick)
        {
            if (dice != 1 && dice != 5)
            {
                Console.WriteLine("Dice cannot be removed");
                continue;
            }
            
            if (m_Dices.Contains(dice))
            {

                if (dice == 1)
                {
                    UpdateScore(100);
                }
                else
                {
                    UpdateScore(50);
                }
                
                m_CanRoll = true;
                RemoveDice(dice);
                m_Dices.Remove(dice);
            }
        }
        
        PrintDices();
        AskForAction();
    }
}