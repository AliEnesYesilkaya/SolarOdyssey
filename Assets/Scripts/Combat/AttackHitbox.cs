using UnityEngine;

namespace SolarOdyssey.Combat
{
    public class AttackHitbox : MonoBehaviour
    {
        private Collider2D hitbox;

        private void Awake()
        {
            hitbox = GetComponent<Collider2D>();
            hitbox.enabled = false;
        }

        public void EnableHitbox()
        {
            hitbox.enabled = true;
        }

        public void DisableHitbox()
        {
            hitbox.enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Health health = other.GetComponentInParent<Health>();

            if (health != null)
            {
                Debug.Log("ATTACK HITBOX ÇARPTI: " + other.name);
                health.TakeDamage(10);
            }
        }
    }
}