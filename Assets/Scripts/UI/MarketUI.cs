using TMPro;
using SolarOdyssey.Player;
using SolarOdyssey.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace SolarOdyssey.Core
{
    public class MarketUI : MonoBehaviour
    {
        private PlayerUpgradeSystem upgradeSystem;
        private CoinUI coinUI;

        [Header("Market Panel")]
        [SerializeField] private GameObject marketPanel;

        [Header("Market Sections")]
        [SerializeField] private GameObject swordSection;
        [SerializeField] private GameObject knifeSection;
        [SerializeField] private GameObject potionSection;

        [Header("Market Buttons")]
        [SerializeField] private GameObject swordButton;
        [SerializeField] private GameObject knifeButton;
        [SerializeField] private GameObject potionButton;

        [Header("Selected Button Scale")]
        [SerializeField] private float selectedButtonScale = 1.08f;
        [SerializeField] private float normalButtonScale = 1f;

        [Header("Knife UI")]
        [SerializeField] private TMP_Text knifeLevelText;
        [SerializeField] private TMP_Text knifeDamageText;
        [SerializeField] private TMP_Text knifeUpgradeButtonText;

        [Header("Sword UI")]
        [SerializeField] private TMP_Text swordLevelText;
        [SerializeField] private TMP_Text swordDamageText;
        [SerializeField] private TMP_Text swordUpgradeButtonText;

        // ==================================================
        // GELİŞTİRME BARLARI
        // ==================================================

        [Header("Knife Upgrade Bar")]
        [SerializeField] private Image knifeUpgradeBar;

        [Header("Sword Upgrade Bar")]
        [SerializeField] private Image swordUpgradeBar;

        [Header("Upgrade Bar Settings")]
        [SerializeField] private Color filledColor = Color.green;

        // Barın dış kenarından içeriye bırakılan boşluk.
        [SerializeField] private float barHorizontalPadding = 8f;

        // Kareler arasındaki duvarların kapladığı alan.
        [SerializeField] private float barGap = 4f;

        // Karelerin üst/alt kenarlarından içeriye bırakılan boşluk.
        [SerializeField] private float barVerticalPadding = 4f;

        private Image[] knifeFillSquares;
        private Image[] swordFillSquares;

        private Sprite generatedWhiteSprite;

        private void Awake()
        {
            upgradeSystem =
                FindFirstObjectByType<PlayerUpgradeSystem>();

            coinUI =
                FindFirstObjectByType<CoinUI>();

            // Yeşil dolum için kullanılacak basit sprite'ı kod oluşturuyor.
            CreateWhiteSprite();

            // 5 bölmeli barların yeşil alanlarını oluştur.
            CreateUpgradeBar(knifeUpgradeBar, out knifeFillSquares);
            CreateUpgradeBar(swordUpgradeBar, out swordFillSquares);

            UpdateMarketUI();

            // Market ilk açıldığında bütün bölümler kapalı.
            CloseAllSections();

            ResetButtonScales();

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

        // ==================================================
        // MARKET AÇ / KAPAT
        // ==================================================

        public void Show()
        {
            UpdateMarketUI();

            CloseAllSections();
            ResetButtonScales();

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

        // ==================================================
        // MARKET SEKME SİSTEMİ
        // ==================================================

        public void OpenSword()
        {
            CloseAllSections();

            if (swordSection != null)
                swordSection.SetActive(true);

            ResetButtonScales();

            if (swordButton != null)
            {
                swordButton.transform.localScale =
                    Vector3.one * selectedButtonScale;
            }
        }

        public void OpenKnife()
        {
            CloseAllSections();

            if (knifeSection != null)
                knifeSection.SetActive(true);

            ResetButtonScales();

            if (knifeButton != null)
            {
                knifeButton.transform.localScale =
                    Vector3.one * selectedButtonScale;
            }
        }

        public void OpenPotion()
        {
            CloseAllSections();

            if (potionSection != null)
                potionSection.SetActive(true);

            ResetButtonScales();

            if (potionButton != null)
            {
                potionButton.transform.localScale =
                    Vector3.one * selectedButtonScale;
            }
        }

        private void CloseAllSections()
        {
            if (swordSection != null)
                swordSection.SetActive(false);

            if (knifeSection != null)
                knifeSection.SetActive(false);

            if (potionSection != null)
                potionSection.SetActive(false);
        }

        private void ResetButtonScales()
        {
            if (swordButton != null)
                swordButton.transform.localScale =
                    Vector3.one * normalButtonScale;

            if (knifeButton != null)
                knifeButton.transform.localScale =
                    Vector3.one * normalButtonScale;

            if (potionButton != null)
                potionButton.transform.localScale =
                    Vector3.one * normalButtonScale;
        }

        // ==================================================
        // MARKET UI GÜNCELLEME
        // ==================================================

        private void UpdateMarketUI()
        {
            if (upgradeSystem == null)
                return;

            UpdateKnifeUI();
            UpdateSwordUI();
        }

        // ==================================================
        // BIÇAK UI
        // ==================================================

        private void UpdateKnifeUI()
        {
            int level =
                upgradeSystem.KnifeLevel;

            int currentDamage =
                5 + (level * 3);

            if (knifeLevelText != null)
            {
                knifeLevelText.text =
                    "BICAK LEVEL = " + level;
            }

            if (level >= 5)
            {
                if (knifeUpgradeButtonText != null)
                {
                    knifeUpgradeButtonText.text = "MAX";
                }

                if (knifeDamageText != null)
                {
                    knifeDamageText.text =
                        "MAX HASAR = 20";
                }

                UpdateKnifeUpgradeBar(level);
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
                    price.ToString();
            }

            if (knifeDamageText != null)
            {
                knifeDamageText.text =
                    "LVL." +
                    level +
                    " = " +
                    currentDamage +
                    " HASAR → LVL." +
                    nextLevel +
                    " = " +
                    nextDamage +
                    " HASAR";
            }

            UpdateKnifeUpgradeBar(level);
        }

        // ==================================================
        // KILIÇ UI
        // ==================================================

        private void UpdateSwordUI()
        {
            int level =
                upgradeSystem.SwordLevel;

            int currentDamage =
                10 + (level * 4);

            if (swordLevelText != null)
            {
                swordLevelText.text =
                    "KILIC LEVEL = " + level;
            }

            if (level >= 5)
            {
                if (swordUpgradeButtonText != null)
                {
                    swordUpgradeButtonText.text = "MAX";
                }

                if (swordDamageText != null)
                {
                    swordDamageText.text =
                        "MAX HASAR = 30";
                }

                UpdateSwordUpgradeBar(level);
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
                    price.ToString();
            }

            if (swordDamageText != null)
            {
                swordDamageText.text =
                    "LVL." +
                    level +
                    " = " +
                    currentDamage +
                    " HASAR → LVL." +
                    nextLevel +
                    " = " +
                    nextDamage +
                    " HASAR";
            }

            UpdateSwordUpgradeBar(level);
        }

        // ==================================================
        // 5 BÖLMELİ GELİŞTİRME BARINI OLUŞTUR
        // ==================================================

        private void CreateUpgradeBar(
            Image bar,
            out Image[] fillSquares)
        {
            fillSquares = new Image[5];

            if (bar == null)
                return;

            RectTransform barRect =
                bar.GetComponent<RectTransform>();

            float totalWidth =
                barRect.rect.width;

            float totalHeight =
                barRect.rect.height;

            float usableWidth =
                totalWidth -
                (barHorizontalPadding * 2);

            float slotWidth =
                (usableWidth -
                 (barGap * 4)) / 5f;

            for (int i = 0; i < 5; i++)
            {
                GameObject fillObject =
                    new GameObject(
                        "GreenFill_" + (i + 1),
                        typeof(RectTransform),
                        typeof(Image));

                fillObject.transform.SetParent(
                    bar.transform,
                    false);

                Image fillImage =
                    fillObject.GetComponent<Image>();

                fillImage.sprite =
                    generatedWhiteSprite;

                fillImage.color =
                    filledColor;

                RectTransform fillRect =
                    fillObject.GetComponent<RectTransform>();

                float x =
                    barHorizontalPadding +
                    (slotWidth + barGap) * i;

                fillRect.anchorMin =
                    new Vector2(0f, 0.5f);

                fillRect.anchorMax =
                    new Vector2(0f, 0.5f);

                fillRect.pivot =
                    new Vector2(0f, 0.5f);

                fillRect.sizeDelta =
                    new Vector2(
                        slotWidth,
                        totalHeight -
                        (barVerticalPadding * 2));

                fillRect.anchoredPosition =
                    new Vector2(
                        x,
                        0f);

                fillImage.enabled = false;

                fillSquares[i] =
                    fillImage;
            }
        }

        // ==================================================
        // YEŞİL BAR GÜNCELLEME
        // ==================================================

        private void UpdateKnifeUpgradeBar(int level)
        {
            UpdateUpgradeBar(
                knifeFillSquares,
                level);
        }

        private void UpdateSwordUpgradeBar(int level)
        {
            UpdateUpgradeBar(
                swordFillSquares,
                level);
        }

        private void UpdateUpgradeBar(
            Image[] squares,
            int level)
        {
            if (squares == null)
                return;

            for (int i = 0; i < squares.Length; i++)
            {
                if (squares[i] == null)
                    continue;

                squares[i].enabled =
                    i < level;
            }
        }

        // ==================================================
        // KOD İÇİNDE BASİT BEYAZ SPRITE OLUŞTUR
        // ==================================================

        private void CreateWhiteSprite()
        {
            Texture2D texture =
                new Texture2D(1, 1);

            texture.SetPixel(
                0,
                0,
                Color.white);

            texture.Apply();

            generatedWhiteSprite =
                Sprite.Create(
                    texture,
                    new Rect(
                        0,
                        0,
                        1,
                        1),
                    new Vector2(
                        0.5f,
                        0.5f));
        }

        // ==================================================
        // BIÇAK SATIN ALMA
        // ==================================================

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

        // ==================================================
        // BIÇAK GELİŞTİRME
        // ==================================================

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

        // ==================================================
        // KILIÇ GELİŞTİRME
        // ==================================================

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

        // ==================================================
        // İKSİR
        // ==================================================

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