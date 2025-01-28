using UnityEngine;

namespace CodeCatGames.HiveMind.Samples.Runtime.MVC.Signals
{
    public readonly struct InitializeSignal { } //Has Command
    public readonly struct ChangeColorSignal
    {
        #region Fields
        private readonly Color _color;
        #endregion

        #region Getters
        public Color Color => _color;
        #endregion

        #region Constructor
        public ChangeColorSignal(Color color) => _color = color;
        #endregion
    }
}
