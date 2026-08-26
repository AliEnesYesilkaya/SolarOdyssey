using TMPro;
using UnityEngine;

namespace SolarOdyssey.UI
{
    public class CoinUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text coinText;

        private int totalGold = 0;

        private void Start()
        {
            UpdateCoinText();
        }

        public void AddGold(int amount)
        {
            totalGold += amount;

            UpdateCoinText();
        }

        // Kalp satın alma gibi sistemlerin
        // altın harcayabilmesini sağlar.
        public bool SpendGold(int amount)
        {
            // Yeterli altın yoksa satın alma başarısız.
            if (totalGold < amount)
                return false;

            totalGold -= amount;

            UpdateCoinText();

            return true;
        }

        private void UpdateCoinText()
        {
            coinText.text =
                totalGold.ToString("D4");
        }
    }
}