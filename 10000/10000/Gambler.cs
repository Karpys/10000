namespace _10000;

public class Gambler : IPlayerProfile
{
    private DiceGame m_DiceGame = null;
    private int m_GambleUntil = 0;
    public DiceGame Game
    {
        get => m_DiceGame;
        set => m_DiceGame = value;
    }
    
    public Gambler(int gambleUntil)
    {
        m_GambleUntil = gambleUntil;
    }
    public void AskForAction(List<DiceGameAction> actions)
    {
        if (actions.Contains(DiceGameAction.PickDice))
        {
            if (m_DiceGame.Dices.Contains(1))
            {
                TakeAll(1);
            }else if (m_DiceGame.Dices.Contains(5))
            {
                TakeAll(5);
            }
        }
        
        if(actions.Contains(DiceGameAction.Save))
            CheckSave();
        
        if(actions.Contains(DiceGameAction.Roll))
            m_DiceGame.Roll();
    }

    private void CheckSave()
    {
        if (m_DiceGame.CurrentScore >= m_GambleUntil)
        {
            m_DiceGame.SavePointAndNextGame();
        }
    }

    private void TakeAll(int dice)
    {
        int oneCount = m_DiceGame.Dices.Count(i => i == dice);

        for (int i = 0; i < oneCount; i++)
        {
            m_DiceGame.PickDice1();
        }
    }
}