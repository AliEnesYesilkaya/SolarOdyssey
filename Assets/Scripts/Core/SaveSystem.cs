using UnityEngine;

namespace SolarOdyssey.Core
{
    public class SaveSystem : MonoBehaviour
    {
        public static SaveSystem Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null &&
                Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            // Sahne değişse bile SaveSystem yaşamaya devam eder.
            DontDestroyOnLoad(gameObject);
        }

        // ------------------------------------------------
        // GOLD
        // ------------------------------------------------

        public void SaveGold(int gold)
        {
            PlayerPrefs.SetInt("Gold", gold);
            PlayerPrefs.Save();
        }

        public int LoadGold()
        {
            return PlayerPrefs.GetInt("Gold", 0);
        }

        // ------------------------------------------------
        // KILIÇ
        // ------------------------------------------------

        public void SaveSwordLevel(int level)
        {
            PlayerPrefs.SetInt(
                "SwordLevel",
                level
            );

            PlayerPrefs.Save();
        }

        public int LoadSwordLevel()
        {
            return PlayerPrefs.GetInt(
                "SwordLevel",
                0
            );
        }

        // ------------------------------------------------
        // BIÇAK LEVEL
        // ------------------------------------------------

        public void SaveKnifeLevel(int level)
        {
            PlayerPrefs.SetInt(
                "KnifeLevel",
                level
            );

            PlayerPrefs.Save();
        }

        public int LoadKnifeLevel()
        {
            return PlayerPrefs.GetInt(
                "KnifeLevel",
                0
            );
        }

        // ------------------------------------------------
        // BIÇAK SAYISI
        // ------------------------------------------------

        public void SaveKnifeCount(int count)
        {
            PlayerPrefs.SetInt(
                "KnifeCount",
                count
            );

            PlayerPrefs.Save();
        }

        public int LoadKnifeCount()
        {
            return PlayerPrefs.GetInt(
                "KnifeCount",
                0
            );
        }

        // ------------------------------------------------
        // İKSİR
        // ------------------------------------------------

        public void SavePotionCount(int count)
        {
            PlayerPrefs.SetInt(
                "PotionCount",
                count
            );

            PlayerPrefs.Save();
        }

        public int LoadPotionCount()
        {
            return PlayerPrefs.GetInt(
                "PotionCount",
                0
            );
        }

        // ------------------------------------------------
        // CAN
        // ------------------------------------------------

        public void SaveLives(int lives)
        {
            PlayerPrefs.SetInt(
                "CurrentLives",
                lives
            );

            PlayerPrefs.Save();
        }

        public int LoadLives(int maxLives)
        {
            return Mathf.Clamp(
                PlayerPrefs.GetInt(
                    "CurrentLives",
                    maxLives
                ),
                0,
                maxLives
            );
        }

        // ------------------------------------------------
        // CHECKPOINT
        // ------------------------------------------------

        public void SaveCheckpoint(Vector3 position)
        {
            PlayerPrefs.SetFloat(
                "CheckpointX",
                position.x
            );

            PlayerPrefs.SetFloat(
                "CheckpointY",
                position.y
            );

            PlayerPrefs.SetFloat(
                "CheckpointZ",
                position.z
            );

            PlayerPrefs.SetInt(
                "HasCheckpoint",
                1
            );

            PlayerPrefs.Save();
        }

        public bool HasSavedCheckpoint()
        {
            return PlayerPrefs.GetInt(
                "HasCheckpoint",
                0
            ) == 1;
        }

        public Vector3 LoadCheckpoint()
        {
            return new Vector3(
                PlayerPrefs.GetFloat("CheckpointX"),
                PlayerPrefs.GetFloat("CheckpointY"),
                PlayerPrefs.GetFloat("CheckpointZ")
            );
        }

        // ------------------------------------------------
        // TÜM SAVE'İ SİL
        // ------------------------------------------------

        public void DeleteSave()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();

            Debug.Log(
                "Save dosyası silindi."
            );
        }
    }
}