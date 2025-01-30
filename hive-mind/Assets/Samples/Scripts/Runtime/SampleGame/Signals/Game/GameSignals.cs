namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Game
{
    public readonly struct InitializeGameSignal { } // Has Command
    public readonly struct PlayGameSignal { } // Has Command
    public readonly struct GameOverSignal
    {
        #region ReadonlyFields
        private readonly bool _isSuccess;
        #endregion

        #region Getters
        public bool IsSuccess => _isSuccess;
        #endregion

        #region Constructor
        public GameOverSignal(bool isSuccess) => _isSuccess = isSuccess;
        #endregion
    } // Has Command
    public readonly struct GameExitSignal { } // Has Command
    public readonly struct SetupGameOverPanelSignal
    {
        #region ReadonlyFields
        private readonly bool _isSuccess;
        #endregion

        #region Getters
        public bool IsSuccess => _isSuccess;
        #endregion

        #region Constructor
        public SetupGameOverPanelSignal(bool isSuccess) => _isSuccess = isSuccess;
        #endregion
    }
}
