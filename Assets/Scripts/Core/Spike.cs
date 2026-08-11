using SolarOdyssey.Combat;
using UnityEngine;

namespace SolarOdyssey.Core
{
    public class Spike : MonoBehaviour
    {
        [SerializeField] private int damage = 20;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Health health = other.GetComponent<Health>();

                if (health != null)
                {
                    health.TakeDamage(damage);
                }
            }
        }
    }
}