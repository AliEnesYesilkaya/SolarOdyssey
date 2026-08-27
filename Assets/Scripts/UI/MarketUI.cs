using TMPro;
using SolarOdyssey.Player;
using SolarOdyssey.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SolarOdyssey.Core
{
    public class MarketUI : MonoBehaviour
    {
        private PlayerUpgradeSystem upgradeSystem;
        private CoinUI coinUI;

        [Header("Market Panel")]
        [SerializeField] private GameObject marketPanel;

        [Header("Knife UI")]
        [SerializeField] private TMP_Text knifeLevelText;
        [SerializeField] private TMP_Text knifeDamageText;
        [SerializeField] private TMP_Text knifeUpgradeButtonText;

        [Header("Sword UI")]
        [SerializeField] private TMP_Text swordLevelText;
        [SerializeField] private TMP_Text swordDamageText;
        [SerializeField] private TMP_Text swordUpgradeButtonText;

        private void Awake()
        {
            upgradeSystem =
                FindFirstObjectByType<PlayerUpgradeSystem>();

            coinUI =
                FindFirstObjectByType<CoinUI>();

            UpdateMarketUI();

            // MarketUI objesi aktif kalacak.
            // Sadece görüntülenen panel kapalı olacak.
            if (marketPanel != null)
            {
                marketPanel.SetActive(false);
            }
        }

        private void Update()
        {
            if (Keyboard.current == null)
                return;

            // M = market aç / kapat
            if (Keyboard.current.mKey.wasPressedThisFrame)
            {
                if (marketPanel != null &&
                    marketPanel.activeSelf)
                {
                    Hide();
                }
                else
                {
                    Show();
                }
            }
        }

        public void Show()
        {
            UpdateMarketUI();

            if (marketPanel != null)
            {
                marketPanel.SetActive(true);
            }

            Time.timeScale = 0f;
        }

        public void Hide()
        {
            if (marketPanel != null)
            {
                marketPanel.SetActive(false);
            }

            Time.timeScale = 1f;
        }

        // ------------------------------------------------
        // MARKET UI GÜNCELLEME
        // ------------------------------------------------

        private void UpdateMarketUI()
        {
            if (upgradeSystem == null)
                return;

            UpdateKnifeUI();
            UpdateSwordUI();
        }

        // ------------------------------------------------
        // BIÇAK UI
        // ------------------------------------------------

        private void UpdateKnifeUI()
        {
            int level =
                upgradeSystem.KnifeLevel;

            int currentDamage =
                5 + (level * 3);

            if (knifeLevelText != null)
            {
                knifeLevelText.text =
                    "Bıçak level = " +
                    level;
            }

            if (level >= 5)
            {
                if (knifeUpgradeButtonText != null)
                {
                    knifeUpgradeButtonText.text =
                        "MAX ";
                }

                if (knifeDamageText != null)
                {
                    knifeDamageText.text =
                        "MAX HASAR = 20";
                }

                return;
            }

            int nextLevel =
                level + 1;

            int nextDamage =
                5 + (nextLevel * 3);

            int price =
                nextLevel * 100;

            if (knifeUpgradeButtonText != null)
            {
                knifeUpgradeButtonText.text =
                    "Lvl." +
                    nextLevel +
                    " = " +
                    price;
            }

            if (knifeDamageText != null)
            {
                knifeDamageText.text =
                    "Lvl." +
                    level +
                    " = " +
                    currentDamage +
                    " hasar → Lvl." +
                    nextLevel +
                    " = " +
                    nextDamage +
                    " hasar";
            }
        }

        // ------------------------------------------------
        // KILIÇ UI
        // ------------------------------------------------

        private void UpdateSwordUI()
        {
            int level =
                upgradeSystem.SwordLevel;

            int currentDamage =
                10 + (level * 4);

            if (swordLevelText != null)
            {
                swordLevelText.text =
                    "Kılıç level = " +
                    level;
            }

            if (level >= 5)
            {
                if (swordUpgradeButtonText != null)
                {
                    swordUpgradeButtonText.text =
                        "MAX ";
                }

                if (swordDamageText != null)
                {
                    swordDamageText.text =
                        "MAX HASAR = 30";
                }

                return;
            }

            int nextLevel =
                level + 1;

            int nextDamage =
                10 + (nextLevel * 4);

            int price =
                nextLevel * 150;

            if (swordUpgradeButtonText != null)
            {
                swordUpgradeButtonText.text =
                    "Lvl." +
                    nextLevel +
                    " = " +
                    price;
            }

            if (swordDamageText != null)
            {
                swordDamageText.text =
                    "Lvl." +
                    level +
                    " = " +
                    currentDamage +
                    " hasar → Lvl." +
                    nextLevel +
                    " = " +
                    nextDamage +
                    " hasar";
            }
        }

        // ------------------------------------------------
        // BIÇAK SATIN ALMA
        // ------------------------------------------------

        public void BuyFiveKnives()
        {
            if (upgradeSystem == null ||
                coinUI == null)
                return;

            if (!coinUI.SpendGold(400))
                return;

            upgradeSystem.AddKnives(5);
        }

        public void BuyTenKnives()
        {
            if (upgradeSystem == null ||
                coinUI == null)
                return;

            if (!coinUI.SpendGold(600))
                return;

            upgradeSystem.AddKnives(10);
        }

        // ------------------------------------------------
        // BIÇAK GELİŞTİRME
        // ------------------------------------------------

        public void UpgradeKnife()
        {
            if (upgradeSystem == null ||
                coinUI == null)
                return;

            int level =
                upgradeSystem.KnifeLevel;

            if (level >= 5)
                return;

            int price =
                (level + 1) * 100;

            if (!coinUI.SpendGold(price))
                return;

            if (upgradeSystem.UpgradeKnife())
            {
                UpdateMarketUI();
            }
        }

        // ------------------------------------------------
        // KILIÇ GELİŞTİRME
        // ------------------------------------------------

        public void UpgradeSword()
        {
            if (upgradeSystem == null ||
                coinUI == null)
                return;

            int level =
                upgradeSystem.SwordLevel;

            if (level >= 5)
                return;

            int price =
                (level + 1) * 150;

            if (!coinUI.SpendGold(price))
                return;

            if (upgradeSystem.UpgradeSword())
            {
                UpdateMarketUI();
            }
        }

        // ------------------------------------------------
        // İKSİR
        // ------------------------------------------------

        public void BuyOneMinutePotion()
        {
            if (upgradeSystem == null ||
                coinUI == null)
                return;

            if (!coinUI.SpendGold(500))
                return;

            upgradeSystem.AddPotion(60f);
        }

        public void BuyThreeMinutePotion()
        {
            if (upgradeSystem == null ||
                coinUI == null)
                return;

            if (!coinUI.SpendGold(1000))
                return;

            upgradeSystem.AddPotion(180f);
        }
    }
}