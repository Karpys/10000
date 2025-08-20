namespace _10000;

public interface IPlayerProfile
{
    void AskForAction(List<DiceGameAction> actions);
    DiceGame Game { get; set; }
}