using System.Collections.Generic;
using UnityEngine;

namespace HiveMind.Core.Utilities.Runtime.SerializedDictionary
{
    public abstract class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        #region Fields
        [SerializeField, HideInInspector] private List<TKey> keyData = new();
        [SerializeField, HideInInspector] private List<TValue> valueData = new();
        #endregion

        #region Serializations
        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            Clear();
            for (int i = 0; i < keyData.Count && i < valueData.Count; i++)
                this[keyData[i]] = valueData[i];
        }
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            keyData.Clear();
            valueData.Clear();

            foreach (var item in this)
            {
                this.keyData.Add(item.Key);
                this.valueData.Add(item.Value);
            }
        }
        #endregion
    }
}
