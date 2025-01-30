using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ValueObjects.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
using Lofelt.NiceVibrations;
using UnityEngine;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene
{
    public readonly struct ChangeLoadingScreenActivationSignal
    {
        #region ReadonlyFields
        private readonly bool _isActive;
        private readonly AsyncOperation _asyncOperation;
        #endregion

        #region Getters
        public bool IsActive => _isActive;
        public AsyncOperation AsyncOperation => _asyncOperation;
        #endregion

        #region Constructor
        public ChangeLoadingScreenActivationSignal(bool isActive, AsyncOperation asyncOperation)
        {
            _isActive = isActive;
            _asyncOperation = asyncOperation;
        }
        #endregion
    }
    public readonly struct LoadSceneSignal
    {
        #region ReadonlyFields
        private readonly SceneID _sceneId;
        #endregion

        #region Getters
        public SceneID SceneId => _sceneId;
        #endregion

        #region Constructor
        public LoadSceneSignal(SceneID sceneId) => _sceneId = sceneId;
        #endregion
    } // Has Command
    public readonly struct PlayAudioSignal
    {
        #region ReadonlyFields
        private readonly AudioTypes _audioType;
        private readonly MusicTypes _musicType;
        private readonly SoundTypes _soundType;
        #endregion

        #region Getters
        public AudioTypes AudioType => _audioType;
        public MusicTypes MusicType => _musicType;
        public SoundTypes SoundType => _soundType;
        #endregion

        #region Constructor
        public PlayAudioSignal(AudioTypes audioType, MusicTypes musicType, SoundTypes soundType)
        {
            _audioType = audioType;
            _musicType = musicType;
            _soundType = soundType;
        }
        #endregion
    }
    public readonly struct PlayHapticSignal
    {
        #region ReadonlyFields
        private readonly HapticPatterns.PresetType _hapticType;
        #endregion

        #region Getters
        public HapticPatterns.PresetType HapticType => _hapticType;
        #endregion

        #region Constructor
        public PlayHapticSignal(HapticPatterns.PresetType hapticType) => _hapticType = hapticType;
        #endregion
    } // Has Command
    public readonly struct ChangeCurrencySignal
    {
        #region ReadonlyFields
        private readonly CurrencyTypes _currencyType;
        private readonly int _amount;
        private readonly bool _isSet;
        #endregion

        #region Getters
        public CurrencyTypes CurrencyType => _currencyType;
        public int Amount => _amount;
        public bool IsSet => _isSet;
        #endregion

        #region Constructor
        public ChangeCurrencySignal(CurrencyTypes currencyType, int amount, bool isSet)
        {
            _currencyType = currencyType;
            _amount = amount;
            _isSet = isSet;
        }
        #endregion
    } // Has Command
    public readonly struct SpawnCurrencyTrailSignal
    {
        #region ReadonlyFields
        private readonly CurrencyTrailData _currencyTrailData;
        #endregion

        #region Getters
        public CurrencyTrailData CurrencyTrailData => _currencyTrailData;
        #endregion

        #region Constructor
        public SpawnCurrencyTrailSignal(CurrencyTrailData currencyTrailData) => _currencyTrailData = currencyTrailData;
        #endregion
    }
    public readonly struct RefreshCurrencyVisualSignal
    {
        #region Fields
        private readonly CurrencyTypes _currencyType;
        #endregion

        #region Getters
        public CurrencyTypes CurrencyType => _currencyType;
        #endregion

        #region Constructor
        public RefreshCurrencyVisualSignal(CurrencyTypes currencyType) => _currencyType = currencyType;
        #endregion
    }
    public readonly struct SettingsButtonPressedSignal
    {
        #region ReadonlyFields
        private readonly SettingsTypes _settingsType;
        #endregion

        #region Getters
        public SettingsTypes SettingsType => _settingsType;
        #endregion

        #region Constructor
        public SettingsButtonPressedSignal(SettingsTypes settingsType) => _settingsType = settingsType;
        #endregion
    } // Has Command
    public readonly struct SettingsButtonRefreshSignal
    {
        #region ReadonlyFields
        private readonly SettingsTypes _settingsType;
        #endregion

        #region Getters
        public SettingsTypes SettingsType => _settingsType;
        #endregion

        #region Constructor
        public SettingsButtonRefreshSignal(SettingsTypes settingsType) => _settingsType = settingsType;
        #endregion
    }
    public readonly struct ChangeUIPanelSignal
    {
        #region ReadonlyFields
        private readonly UIPanelTypes _uiPanelType;
        #endregion

        #region Getters
        public UIPanelTypes UIPanelType => _uiPanelType;
        #endregion

        #region Constructor
        public ChangeUIPanelSignal(UIPanelTypes uiPanelType) => _uiPanelType = uiPanelType;
        #endregion
    }
}
