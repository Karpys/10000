using _10000;

DiceGame game = new DiceGame(new Gambler(1000), 100);

game.StartGame();
while (game.InGame)
{
    game.AskForAction();
}
