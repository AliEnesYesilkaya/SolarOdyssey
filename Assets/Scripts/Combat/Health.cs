using UnityEngine;
using SolarOdyssey.Combat;

namespace SolarOdyssey.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 100;
        
        private int currentHealth;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;

            Debug.Log(gameObject.name + " Health: " + currentHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log(gameObject.name + " Dead!");
        }
    }
}