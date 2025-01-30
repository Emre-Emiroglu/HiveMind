using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.CrossScene
{
    public sealed class AudioMediator : Mediator<AudioView>
    {
        #region ReadonlyFields
        private readonly SignalBus _signalBus;
        private readonly AudioModel _audioModel;
        #endregion

        #region Constructor
        public AudioMediator(AudioView view, SignalBus signalBus, AudioModel audioModel) : base(view)
        {
            _signalBus = signalBus;
            _audioModel = audioModel;
        }
        #endregion

        #region PostConstruct
        public override void PostConstruct() { }
        #endregion

        #region Core
        public override void Initialize() => _signalBus.Subscribe<PlayAudioSignal>(OnPlayAudio);
        public override void Dispose() => _signalBus.Unsubscribe<PlayAudioSignal>(OnPlayAudio);
        #endregion

        #region SignalReceivers
        private void OnPlayAudio(PlayAudioSignal signal) =>
            PlayAudioProcess(signal.AudioType, signal.MusicType, signal.SoundType);
        #endregion

        #region Executes
        private void PlayAudioProcess(AudioTypes audioType, MusicTypes musicType, SoundTypes soundType)
        {
            switch (audioType)
            {
                case AudioTypes.Music:
                    GetView.AudioSources[audioType].clip = _audioModel.GetSettings.Musics[musicType];
                    GetView.AudioSources[audioType].loop = true;
                    GetView.AudioSources[audioType].Play();
                    break;
                case AudioTypes.Sound:
                    GetView.AudioSources[audioType].PlayOneShot(_audioModel.GetSettings.Sounds[soundType]);
                    break;
            }
        }
        #endregion
    }
}