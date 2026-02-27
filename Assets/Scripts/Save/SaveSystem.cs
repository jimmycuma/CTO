using System.IO;
using UnityEngine;
using CursedDepths.Core;

namespace CursedDepths.Save
{
    [System.Serializable]
    public class GameData
    {
        public int gold;
        public int currentLevel;
        public float[] playerPosition;
        public int playerHealth;
        public System.Collections.Generic.List<string> inventoryItems = new System.Collections.Generic.List<string>();
    }

    public static class SaveSystem
    {
        private static string savePath = Application.persistentDataPath + "/curseddepths_save.json";

        public static void SaveGameData()
        {
            GameData data = new GameData();

            if (GameManager.Instance != null)
            {
                data.gold = GameManager.Instance.gold;
                data.currentLevel = GameManager.Instance.currentLevel;

                if (GameManager.Instance.player != null)
                {
                    data.playerPosition = new float[3];
                    data.playerPosition[0] = GameManager.Instance.player.transform.position.x;
                    data.playerPosition[1] = GameManager.Instance.player.transform.position.y;
                    data.playerPosition[2] = GameManager.Instance.player.transform.position.z;
                    data.playerHealth = GameManager.Instance.player.currentHealth;
                }
            }

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(savePath, json);
            
            Debug.Log("Game saved to: " + savePath);
        }

        public static void LoadGameData()
        {
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                GameData data = JsonUtility.FromJson<GameData>(json);

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.gold = data.gold;
                    GameManager.Instance.currentLevel = data.currentLevel;
                }

                Debug.Log("Game loaded from: " + savePath);
            }
            else
            {
                Debug.Log("No save file found.");
            }
        }

        public static void DeleteSave()
        {
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
                Debug.Log("Save file deleted.");
            }
        }

        public static bool SaveExists()
        {
            return File.Exists(savePath);
        }
    }
}
