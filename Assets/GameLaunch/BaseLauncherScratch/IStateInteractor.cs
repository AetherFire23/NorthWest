namespace Assets.GameLaunch.BaseLauncherScratch
{
    public interface IStateInteractor<T> where T : class, new()
    {
        public T State { get; set; }
    }
}
