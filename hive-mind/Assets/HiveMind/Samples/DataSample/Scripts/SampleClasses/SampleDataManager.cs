using HiveMind.Core.Data.Services;
using UnityEngine;
using UnityEngine.UI;

namespace HiveMind.Samples.DataSample.SampleClasses
{
    public class SampleDataManager : MonoBehaviour
    {
        #region Fields
        [Header("Sample Data Manager Fields")]
        [SerializeField] private InputField pathField;
        [SerializeField] private InputField dataField;
        [SerializeField] private Text savedDatasText;
        [SerializeField] private Text loadedDatasText;
        private string path;
        private string data;
        private bool encrypted;
        #endregion

        #region Receivers
        public void OnPathValueChanged(string path) => this.path = pathField.text;
        public void OnDataValueChanged(string data) => this.data = dataField.text;
        public void OnEncryptedToggleValueChanged(bool encrypted) => this.encrypted = encrypted;
        public void OnSavedDataButtonPressed()
        {
            bool save = DataService.SaveData(data, path, encrypted);

            savedDatasText.text = save ? data : "Data cannot save";
        }
        public void OnLoadDataButtonPressed() => loadedDatasText.text = DataService.LoadData<string>(path, encrypted, "");
        #endregion
    }
}
