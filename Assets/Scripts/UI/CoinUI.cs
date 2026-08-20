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

        public void AddGold(int amount) //gelen miktarı mevcut miktara ekle yazıyı eşitle
        {
            totalGold += amount;

            UpdateCoinText();
        }

        private void UpdateCoinText()
        {
            coinText.text = totalGold.ToString("D4");
        }
    }
}