using UnityEngine;

namespace SolarOdyssey.Enemy
{
    public class Enemy3AttackAnimationEvents : MonoBehaviour
    {
        private Enemy3 enemy;

        private void Awake()
        {
            enemy = GetComponentInParent<Enemy3>();
        }

        public void FireArrow()
        {
            if (enemy != null)
            {
                enemy.FireArrow();
            }
        }
    }
}