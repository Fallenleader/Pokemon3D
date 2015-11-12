namespace Pokemon3D.Common
{
    public abstract class GameContextObject
    {
        public GameContext GameContext { get; private set; }

        public GameContextObject(GameContext gameContext)
        {
            GameContext = gameContext;
        }
    }
}
