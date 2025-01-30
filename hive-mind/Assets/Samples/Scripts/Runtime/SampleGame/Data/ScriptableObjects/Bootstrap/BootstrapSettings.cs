using UnityEngine;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.Bootstrap
{
    [CreateAssetMenu(fileName = "BootstrapSettings", menuName = "CodeCatGames/HiveMind/Samples/SampleGame/Bootstrap/BootstrapSettings")]
    public sealed class BootstrapSettings : ScriptableObject
    {
        #region Fields
        [Header("Bootstrap Settings Fields")]
        [SerializeField] private Sprite logoSprite;
        [SerializeField] private float sceneActivationDuration;
        #endregion

        #region Getters
        public Sprite LogoSprite => logoSprite;
        public float SceneActivationDuration => sceneActivationDuration;
        #endregion
    }
}
