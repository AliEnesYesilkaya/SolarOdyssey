using UnityEngine;

namespace SolarOdyssey.Enemy
{
    public class EnemyAttackAnimationEvents : MonoBehaviour
    {
        private EnemyBase enemy;

        private void Awake()
        {
            enemy = GetComponentInParent<EnemyBase>();
        }

        public void AttackHit()
        {
            if (enemy != null)
            {
                enemy.AttackHit();
            }
        }
    }
}