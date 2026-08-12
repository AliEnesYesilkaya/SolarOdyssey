using SolarOdyssey.Combat;
using UnityEngine;
using UnityEngine.UIElements;

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
                Debug.Log("ÇARPAN OBJE PLAYER DEÐÝL: " + other.name);
            }
        }


    }
}