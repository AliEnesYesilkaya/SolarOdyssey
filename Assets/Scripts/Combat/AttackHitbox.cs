using UnityEngine;

namespace SolarOdyssey.Combat
{
    public class AttackHitbox : MonoBehaviour
    {
        private Collider2D hitbox;

        private void Awake()
        {
            hitbox = GetComponent<Collider2D>();
            hitbox.enabled = false;// saldırı yapılmadığı sürece hitbox kapalı 
        } 

        public void EnableHitbox()
        {
            hitbox.enabled = true;
        }

        public void DisableHitbox()
        {
            hitbox.enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D other) // temas edilen nesnenin sağlık sistemi varsa hasar ver.
        {
            Health health = other.GetComponent<Health>();

            if (health != null)
            {
                health.TakeDamage(10);
            }
        }
    }
}