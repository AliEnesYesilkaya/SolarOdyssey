using TMPro;
using SolarOdyssey.Player;
using UnityEngine;

namespace SolarOdyssey.UI
{
    public class KnifeUI : MonoBehaviour
    {
        [Header("Knife UI")]
        [SerializeField] private GameObject knifeCounter;
        [SerializeField] private TMP_Text knifeText;

        private PlayerUpgradeSystem upgradeSystem;

        private void Awake()
        {
            upgradeSystem =
                FindFirstObjectByType<PlayerUpgradeSystem>();
        }

        private void Start()
        {
            UpdateKnifeUI();
        }

        private void Update()
        {
            UpdateKnifeUI();
        }

        private void UpdateKnifeUI()
        {
            if (upgradeSystem == null)
                return;

            int knifeCount =
                upgradeSystem.KnifeCount;

            // Bıçak yoksa UI gizli.
            if (knifeCount <= 0)
            {
                if (knifeCounter != null)
                {
                    knifeCounter.SetActive(false);
                }

                return;
            }

            // Bıçak varsa UI görünür.
            if (knifeCounter != null)
            {
                knifeCounter.SetActive(true);
            }

            if (knifeText != null)
            {
                knifeText.text =
                    knifeCount.ToString("D2");
            }
        }
    }
}