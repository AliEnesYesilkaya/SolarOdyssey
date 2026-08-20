using SolarOdyssey.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace SolarOdyssey.UI
{
    public class PlayerHealthBar : MonoBehaviour
    {
        private Image healthBar;
        private Health playerHealth;

        private void Awake() //player tag bul healthını al healtbara ver
        {
            healthBar = GetComponent<Image>();

            GameObject player =
                GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                playerHealth = player.GetComponent<Health>();
            }
        }

        private void Update()
        {
            if (playerHealth == null)
                return;
            //can çubuk doluluk oranını bölerek bular 
            healthBar.fillAmount =
                (float)playerHealth.CurrentHealth /
                playerHealth.MaxHealth;
        }
    }
}
