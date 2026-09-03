using UnityEngine;

namespace SolarOdyssey.Enemy
{
    public class FireKnightAnimationEvents : MonoBehaviour
    {
        private FireKnightBoss boss;

        private void Awake()
        {
            boss =
                GetComponentInParent<FireKnightBoss>();
        }

        public void Attack1Hit()
        {
            if (boss != null)
            {
                boss.Attack1Hit();
            }
        }

        public void Attack2Hit()
        {
            if (boss != null)
            {
                boss.Attack2Hit();
            }
        }

        public void Attack3Hit()
        {
            if (boss != null)
            {
                boss.Attack3Hit();
            }
        }

        public void RangedAttackHit()
        {
            if (boss != null)
            {
                boss.RangedAttackHit();
            }
        }

        public void AttackFinished()
        {
            if (boss != null)
            {
                boss.AttackFinished();
            }
        }

        public void RangedAttackFinished()
        {
            if (boss != null)
            {
                boss.RangedAttackFinished();
            }
        }
    }
}
