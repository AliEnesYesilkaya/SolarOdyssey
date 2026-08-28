using TMPro;
using SolarOdyssey.Core;
using UnityEngine;

namespace SolarOdyssey.UI
{
    public class CoinUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text coinText;

        // Altının uçacağı CoinIcon.
        [SerializeField] private RectTransform coinIcon;

        private int totalGold = 0;

        // Diğer sistemlerin mevcut altını okuyabilmesini sağlar.
        public int TotalGold => totalGold;

        // GoldCollectEffect'in CoinIcon'a ulaşmasını sağlar.
        public RectTransform CoinIcon => coinIcon;

        private void Start()
        {
            // Save varsa kayıtlı altını yükle.
            if (SaveSystem.Instance != null)
            {
                totalGold =
                    SaveSystem.Instance.LoadGold();
            }

            UpdateCoinText();
        }

        public void AddGold(int amount)
        {
            totalGold += amount;

            UpdateCoinText();

            // Yeni altın miktarını kaydet.
            if (SaveSystem.Instance != null)
            {
                SaveSystem.Instance.SaveGold(
                    totalGold
                );
            }
        }

        public bool SpendGold(int amount)
        {
            // Yeterli altın yoksa satın alma başarısız.
            if (totalGold < amount)
                return false;

            totalGold -= amount;

            UpdateCoinText();

            // Harcamadan sonraki altın miktarını kaydet.
            if (SaveSystem.Instance != null)
            {
                SaveSystem.Instance.SaveGold(
                    totalGold
                );
            }

            return true;
        }

        private void UpdateCoinText()
        {
            if (coinText == null)
                return;

            coinText.text =
                totalGold.ToString("D4");
        }
    }
}