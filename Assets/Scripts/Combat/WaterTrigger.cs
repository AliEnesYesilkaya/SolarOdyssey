using SolarOdyssey.UI;
using UnityEngine;

namespace SolarOdyssey
{
    public class WaterTrigger : MonoBehaviour
    {
        private WaterEffect waterEffect;

        private void Awake()
        {
            waterEffect =
                FindFirstObjectByType<WaterEffect>(
                    FindObjectsInactive.Include
                );
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            if (waterEffect == null)
                return;

            waterEffect.EnterWater();

            Debug.Log("Oyuncu suya girdi.");
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            if (waterEffect == null)
                return;

            waterEffect.ExitWater();

            Debug.Log("Oyuncu sudan çıktı.");
        }
    }
}