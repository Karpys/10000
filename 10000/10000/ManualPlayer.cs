namespace _10000;

public class ManualPlayer : IPlayerProfile
{
    public DiceGame Game
    {
        get => m_DiceGame;
        set => m_DiceGame = value;
    }
    
    private DiceGame m_DiceGame = null;
    
    public void AskForAction(List<DiceGameAction> actions)
    {
        string actionDescription = String.Empty;

        for (int i = 0; i < actions.Count; i++)
        {
            DiceGameAction diceGameAction = actions[i];
            switch (diceGameAction)
            {
                case DiceGameAction.Save:
                    actionDescription += "0 : Save Current Score";
                    break;
                case DiceGameAction.PickDice:
                    actionDescription += "1 : PickDice";
                    break;
                case DiceGameAction.Roll:
                    actionDescription += "2 : Roll";
                    break;
            }
            
            if(i != actions.Count - 1) 
                actionDescription += " | ";
        }
        
        Console.WriteLine(actionDescription);

        int action = -1;
        int.TryParse(Console.ReadLine(),out action);

        DiceGameAction chosen = (DiceGameAction)action;

        if (actions.Contains(chosen))
        {
            switch (chosen)
            {
                case DiceGameAction.Save:
                    m_DiceGame.SavePointAndNextGame();
                    break;
                case DiceGameAction.PickDice:
                    PickDicesAction();
                    break;
                case DiceGameAction.Roll:
                    TryRollCurrent();
                    break;
            }
        }
        else
        {
            Console.WriteLine("Fail parse");
            m_DiceGame.AskForAction();
        }
    }
    
    private void TryRollCurrent()
    {
        m_DiceGame.Roll();
    }

    private void PickDicesAction()
    {
        if (!m_DiceGame.Dices.Contains(1) && !m_DiceGame.Dices.Contains(5))
        {
            Console.WriteLine("No pickable Dice, continue");
            m_DiceGame.AskForAction();
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
            
            if (m_DiceGame.Dices.Contains(dice))
            {
                if (dice == 1)
                {
                    m_DiceGame.PickDice1();
                }
                else
                {
                    m_DiceGame.PickDice5();
                }
            }
        }
        
        m_DiceGame.PrintDices();
    }
}