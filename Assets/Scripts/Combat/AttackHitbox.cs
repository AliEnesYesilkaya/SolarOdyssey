using SolarOdyssey.Player;
using UnityEngine;

namespace SolarOdyssey.Combat
{
    public class AttackHitbox : MonoBehaviour
    {
        private Collider2D hitbox;
        private PlayerUpgradeSystem upgradeSystem;

        private void Awake()
        {
            hitbox = GetComponent<Collider2D>();

            hitbox.enabled = false;

            upgradeSystem =
                GetComponentInParent<PlayerUpgradeSystem>();
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
            Health health =
                other.GetComponentInParent<Health>();

            if (health == null)
                return;

            if (upgradeSystem == null)
                return;

            int swordDamage =
                upgradeSystem.GetSwordDamage();

            int bossDamage =
                upgradeSystem.GetBossSwordDamage();

            health.TakePlayerAttackDamage(
                swordDamage,
                bossDamage
            );
        }
    }
}