using SolarOdyssey.Audio;
using SolarOdyssey.Combat;
using UnityEngine;

namespace SolarOdyssey.Core
{
    public class Spike : MonoBehaviour
    {
        [SerializeField] private int damage = 20;

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("SPIKE TRIGGER ÇALIÞTI");

            if (other.CompareTag("Player"))
            {
                Debug.Log("PLAYER TAG'Ý DOÐRU");

                // Suya düþme sesi
                if (EnvironmentAudio.Instance != null)
                {
                    EnvironmentAudio.Instance.PlayWaterSplash();
                }

                Health health = other.GetComponent<Health>();

                if (health != null)
                {
                    Debug.Log("HEALTH BULUNDU");
                    health.TakeDamage(damage);
                }
                else
                {
                    Debug.Log("HEALTH BULUNAMADI");
                }
            }
            else
            {
                Debug.Log(
                    "ÇARPAN OBJE PLAYER DEÐÝL: " +
                    other.name
                );
            }
        }
    }
}