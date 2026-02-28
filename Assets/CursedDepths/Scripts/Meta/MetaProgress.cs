using System.Collections.Generic;
using UnityEngine;

namespace CursedDepths.Meta
{
    [System.Serializable]
    public class MetaProgressData
    {
        public int currency;
        public List<string> unlockedUpgrades = new List<string>();
    }

    public class MetaProgress
    {
        private const string SaveKey = "CursedDepths_Meta";
        private MetaProgressData data = new MetaProgressData();

        public int Currency => data.currency;

        public void AddCurrency(int amount)
        {
            data.currency += amount;
            Core.EventBus.RaiseCurrencyChanged(data.currency);
        }

        public void RegisterUpgrade(string id)
        {
            if (!data.unlockedUpgrades.Contains(id))
            {
                data.unlockedUpgrades.Add(id);
            }
        }

        public bool HasUpgrade(string id)
        {
            return data.unlockedUpgrades.Contains(id);
        }

        public void Save()
        {
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(SaveKey, json);
            PlayerPrefs.Save();
        }

        public void Load()
        {
            if (!PlayerPrefs.HasKey(SaveKey))
            {
                return;
            }

            string json = PlayerPrefs.GetString(SaveKey);
            data = JsonUtility.FromJson<MetaProgressData>(json) ?? new MetaProgressData();
            Core.EventBus.RaiseCurrencyChanged(data.currency);
        }
    }
}
