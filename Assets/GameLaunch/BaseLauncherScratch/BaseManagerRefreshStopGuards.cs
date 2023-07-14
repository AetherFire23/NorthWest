namespace Assets.GameLaunch.BaseLauncherScratch
{
    public class BaseManagerRefreshStopGuards
    {
        public bool IsInitializing { get; set; } = false;
        public bool IsRefreshing { get; set; } = false;

        private float _timeBetweenRefreshes { get; set; } = 3f;
        private float _elapsed { get; set; } = 0f;
        private int _timesTicked { get; set; } = 0;
        public bool MustPreventInitialization()
        {
            bool mustPrevent = IsInitializing;
            return mustPrevent;
        }
    }
}
