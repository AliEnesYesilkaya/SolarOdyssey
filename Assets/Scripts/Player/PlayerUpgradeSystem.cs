using UnityEngine;

namespace SolarOdyssey.Player
{
    public class PlayerUpgradeSystem : MonoBehaviour
    {
        [Header("Sword")]
        [SerializeField] private int swordLevel = 0;

        [Header("Knife")]
        [SerializeField] private int knifeLevel = 0;
        [SerializeField] private int knifeCount = 0;

        [Header("Potion")]
        [SerializeField] private int potionCount = 0;
        [SerializeField] private float potionRemainingTime = 0f;

        public int SwordLevel => swordLevel;
        public int KnifeLevel => knifeLevel;
        public int KnifeCount => knifeCount;
        public int PotionCount => potionCount;

        public bool IsPotionActive =>
            potionRemainingTime > 0f;

        public float PotionRemainingTime =>
            potionRemainingTime;

        private void Update()
        {
            if (potionRemainingTime <= 0f)
                return;

            potionRemainingTime -= Time.deltaTime;

            if (potionRemainingTime < 0f)
            {
                potionRemainingTime = 0f;
            }
        }

        // ------------------------------------------------
        // KILIÇ HASARI
        // ------------------------------------------------

        public int GetSwordDamage()
        {
            return 10 + (swordLevel * 4);
        }

        public int GetBossSwordDamage()
        {
            return 5 + (swordLevel * 2);
        }

        // ------------------------------------------------
        // BIÇAK HASARI
        // ------------------------------------------------

        public int GetKnifeDamage()
        {
            return 5 + (knifeLevel * 3);
        }

        // ------------------------------------------------
        // KILIÇ GELİŞTİRME
        // ------------------------------------------------

        public bool UpgradeSword()
        {
            if (swordLevel >= 5)
                return false;

            swordLevel++;

            return true;
        }

        // ------------------------------------------------
        // BIÇAK GELİŞTİRME
        // ------------------------------------------------

        public bool UpgradeKnife()
        {
            if (knifeLevel >= 5)
                return false;

            knifeLevel++;

            return true;
        }

        // ------------------------------------------------
        // BIÇAK SATIN ALMA
        // ------------------------------------------------

        public void AddKnives(int amount)
        {
            if (amount <= 0)
                return;

            knifeCount += amount;
        }

        public bool UseKnife()
        {
            if (knifeCount <= 0)
                return false;

            knifeCount--;

            return true;
        }

        // ------------------------------------------------
        // İKSİR SATIN ALMA
        // ------------------------------------------------

        public void AddPotion(float duration)
        {
            if (duration <= 0f)
                return;

            potionCount++;
        }

        // ------------------------------------------------
        // İKSİR KULLANMA
        // ------------------------------------------------

        public bool UsePotion(float duration)
        {
            if (potionCount <= 0)
                return false;

            if (IsPotionActive)
                return false;

            potionCount--;

            potionRemainingTime = duration;

            return true;
        }
    }
}