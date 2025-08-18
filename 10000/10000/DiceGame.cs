namespace _10000;

using Utils;

public class DiceGame
{
    private Random m_Random = new Random();
    private int m_Point;
    private int m_CurrentPoint;
    private bool m_HasToForceRoll = false;
    private List<int> m_Dices;
    private bool m_CanRoll = true;
    private int m_GameCount = 0;

    public List<int> Dices => m_Dices;

    public DiceGame()
    {
        m_Dices = Enumerable.Repeat(0, 6).ToList();
    }

    public void StartGame()
    {
        Console.WriteLine("Game : " + m_GameCount);
        Roll();
    }

    private void Reroll()
    {
        m_Dices = Enumerable.Repeat(0, 6).ToList();
        Roll();
    }

    private void NextGame()
    {
        m_GameCount++;
        Console.WriteLine("Game : " + m_GameCount);
        Console.WriteLine("Point : " + m_Point);
        Reset();
    }

    private void Reset()
    {
        m_HasToForceRoll = false;
        m_CanRoll = true;
        m_CurrentPoint = 0;
        Reroll();
    }

    private void FailRound()
    {
        Console.WriteLine("Fail round");
        m_CurrentPoint = 0;
        NextRound();
    }
    
    private void NextRound()
    {
        m_HasToForceRoll = false;
        m_CanRoll = true;
        Console.WriteLine("Next Round");
        Reroll();    
    }
    
    private void Roll()
    {
        if (!m_CanRoll)
        {
            Console.WriteLine("Cant roll, pick a dice or save current score");
            AskForAction();
            return;
        }

        m_CanRoll = false;

        for (int i = 0; i < m_Dices.Count; i++)
        {
            int result = m_Random.Next(1, 7);
            m_Dices[i] = result;
        }

        PrintDices();
        
        bool hasThree = ApplyRuleOfThree();

        if (!hasThree)
        {
            if (CheckForCantPlay())
            {
                Console.WriteLine("No pickable dices");
                FailRound();
                return;
            }
        }
        else
        {
            if (m_Dices.Count == 0)
            {
                Console.WriteLine("No more dices, force to play next round");
                Reroll();
                return;
            }
        }
        
        AskForAction();
    }

    private bool CheckForCantPlay()
    {
        if (m_Dices.Contains(1) || m_Dices.Contains(5))
            return false;
        
        Console.WriteLine("No action possible");
        return true;
    }

    private void PrintDices()
    {
        DicePrinter.InlinePrintDices(m_Dices);
    }

    private bool ApplyRuleOfThree()
    {
        List<int> triples = m_Dices.GroupBy(x => x).Where(g => g.Count() >= 3).Select(g => g.Key).ToList();

        foreach (int triple in triples)
        {
            RemoveDices(new List<int>(){triple,triple,triple});
            int mult = triple == 1 ? 1000 : 100;
            UpdateScore(triple * mult);
            Console.WriteLine("Triple : " + triple);
            PrintDices();
            m_CanRoll = true;
            m_HasToForceRoll = true;
            return true;
        }
        
        return triples.Count > 0;
    }

    private void UpdateScore(int score)
    {
        m_CurrentPoint += score;
        Console.WriteLine("New score : " + m_CurrentPoint);
    }

    private void RemoveDices(List<int> dicesToRemove)
    {
        foreach (int i in dicesToRemove)
        {
            m_Dices.Remove(i);
        }
        
        if(m_Dices.Count == 0)
            NextRound();
    }
    
    private void RemoveDice(int diceToRemove)
    {
        m_Dices.Remove(diceToRemove);
        
        if(m_Dices.Count == 0)
            NextRound();
    }

    private void AskForAction()
    {
        //IProfile : AskForAction
        //ManualPlayer
        Console.WriteLine("0 : Leave with Current Score | 1 : Pick Dice | 2 : Roll");
        int action = -1;
        int.TryParse(Console.ReadLine(),out action);

        switch (action)
        {
            case 0:
                SavePointAndNextGame();
                break;
            case 1:
                PickDicesAction();
                break;
            case 2:
                TryRollCurrent();
                break;
            case -1:
                Console.WriteLine("Fail parse");
                AskForAction();
                break;
        }
    }

    private void SavePointAndNextGame()
    {
        m_Point = m_CurrentPoint;
        Console.WriteLine("Point this round : "  + m_Point);
        NextGame();
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
        List<int> diceToPick = action.Split()
            .Select(s => int.TryParse(s, out int value) ? (int?)value : null)
            .Where(v => v.HasValue)
            .Select(v => v.Value)
            .ToList();


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
            }
        }
        
        PrintDices();
        AskForAction();
    }
}