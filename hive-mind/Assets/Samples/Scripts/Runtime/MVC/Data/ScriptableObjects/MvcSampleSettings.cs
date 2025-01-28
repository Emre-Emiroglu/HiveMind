using UnityEngine;

namespace CodeCatGames.HiveMind.Samples.Runtime.MVC.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "MvcSampleSettings", menuName = "CodeCatGames/HiveMind/Samples/MVC/MvcSampleSettings", order = 0)]
    public sealed class MvcSampleSettings : ScriptableObject
    {
        #region Fields
        [Header("Mvc Sample Settings Fields")]
        [SerializeField] private Color[] colors;
        [SerializeField] private string[] colorNames;
        #endregion

        #region Getters
        public Color[] Colors => colors;
        public string[] ColorNames => colorNames;
        #endregion
    }
}