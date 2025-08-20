namespace _10000;

using Utils;

public enum DiceGameAction
{
    Save = 0,
    PickDice = 1,
    Roll = 2
}

public class DiceGame
{
    private IPlayerProfile m_Player = null;
    private Random m_Random = new Random();
    private int m_Point;
    private int m_CurrentPoint;
    private bool m_HasToForceRoll = false;
    private List<int> m_Dices;
    private bool m_CanRoll = true;
    private int m_GameCount = 0;

    public List<int> Dices => m_Dices;

    public DiceGame(IPlayerProfile player)
    {
        player.Game = this;
        m_Player = player;
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
        NextGame();
    }
    
    private void NextRound()
    {
        m_HasToForceRoll = false;
        m_CanRoll = true;
        Console.WriteLine("Next Round");
        Reroll();    
    }

    public void Roll()
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
        
        if(ApplySpecialRule())
            NextRound();
        
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

    private bool ApplySpecialRule()
    {
        if (m_Dices.Count < 6)
            return false;

        //Triple double
        if (m_Dices.GroupBy(i => i).Where(g => g.Count() >= 2).ToList().Count == 3)
        {
            UpdateScore(1000);
            return true;
        }

        //Suite
        if (m_Dices.GroupBy(i => i).ToList().Count == 6)
        {
            UpdateScore(1000);
            return true;
        }

        return false;
    }

    private bool CheckForCantPlay()
    {
        if (m_Dices.Contains(1) || m_Dices.Contains(5))
            return false;
        
        Console.WriteLine("No action possible");
        return true;
    }

    public void PrintDices()
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
    }
    
    private void RemoveDice(int diceToRemove)
    {
        m_Dices.Remove(diceToRemove);
    }

    public void AskForAction()
    {
        List<DiceGameAction> actions = FetchPossibleActions();
        m_Player.AskForAction(actions);
    }

    private List<DiceGameAction> FetchPossibleActions()
    {
        List<DiceGameAction> gameActions = new List<DiceGameAction>();

        if (!m_HasToForceRoll)
            gameActions.Add(DiceGameAction.Save);
        
        if(HasPickableDices())
            gameActions.Add(DiceGameAction.PickDice);
        
        if(m_CanRoll)
            gameActions.Add(DiceGameAction.Roll);
        
        return gameActions;
    }

    private bool HasPickableDices()
    {
        return m_Dices.Contains(1) || m_Dices.Contains(5);
    }

    public void SavePointAndNextGame()
    {
        m_Point += m_CurrentPoint;
        Console.WriteLine("Point this round : "  + m_Point);
        NextGame();
    }

    public void PickDice1()
    {
        UpdateScore(100);
        m_CanRoll = true;
        RemoveDice(1);
    }
    
    public void PickDice5()
    {
        UpdateScore(50);
        m_CanRoll = true;
        RemoveDice(5);
    }
}