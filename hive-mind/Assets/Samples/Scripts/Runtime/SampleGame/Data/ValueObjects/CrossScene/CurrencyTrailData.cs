using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
using PrimeTween;
using UnityEngine;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ValueObjects.CrossScene
{
    public readonly struct CurrencyTrailData
    {
        #region ReadonlyFields
        private readonly CurrencyTypes _currencyType;
        private readonly int _amount;
        private readonly float _duration;
        private readonly Ease _ease;
        private readonly Vector3 _startPosition;
        private readonly Vector3 _targetPosition;
        #endregion

        #region Getters
        public CurrencyTypes CurrencyType => _currencyType;
        public int Amount => _amount;
        public float Duration => _duration;
        public Ease Ease => _ease;
        public Vector3 StartPosition => _startPosition;
        public Vector3 TargetPosition => _targetPosition;
        #endregion

        #region Constructor
        public CurrencyTrailData(CurrencyTypes currencyType, int amount, float duration, Ease ease, Vector3 startPosition, Vector3 targetPosition)
        {
            _currencyType = currencyType;
            _amount = amount;
            _duration = duration;
            _ease = ease;
            _startPosition = startPosition;
            _targetPosition = targetPosition;
        }
        #endregion
    }
}