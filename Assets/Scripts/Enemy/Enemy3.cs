using UnityEngine;
using SolarOdyssey.Projectiles;

namespace SolarOdyssey.Enemy
{
    public class Enemy3 : EnemyBase
    {
        [Header("Ranged Enemy Settings")]
        [SerializeField] private float attackCooldown = 1.5f;
        [SerializeField] private float verticalTolerance = 1f;

        [Header("Arrow Settings")]
        [SerializeField] private GameObject arrowPrefab;
        [SerializeField] private Transform arrowPoint;

        private float attackTimer;

        protected override void Awake()
        {
            base.Awake();

            attackTimer = 0f;
        }

        protected override void Update()
        {
            base.Update();

            if (player == null)
                return;

            float horizontalDistance =
                Mathf.Abs(
                    player.position.x -
                    transform.position.x
                );

            float verticalDistance =
                Mathf.Abs(
                    player.position.y -
                    transform.position.y
                );

            if (horizontalDistance > detectionRange)
            {
                animator.SetFloat(
                    "Speed",
                    0f
                );

                return;
            }

            FacePlayer();

            rb.linearVelocity =
                new Vector2(
                    0f,
                    rb.linearVelocity.y
                );

            animator.SetFloat(
                "Speed",
                0f
            );

            if (verticalDistance > verticalTolerance)
                return;

            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0f)
            {
                Attack();

                attackTimer =
                    attackCooldown;
            }
        }

        private void Attack()
        {
            if (animator != null)
            {
                animator.SetTrigger(
                    "Attack"
                );
            }

            Debug.Log(
                "Enemy3 OK ATIYOR!"
            );
        }

        public void FireArrow()
        {
            if (arrowPrefab == null)
            {
                Debug.LogWarning(
                    "Enemy3: Arrow Prefab atanmadı!"
                );

                return;
            }

            if (arrowPoint == null)
            {
                Debug.LogWarning(
                    "Enemy3: Arrow Point atanmadı!"
                );

                return;
            }

            if (player == null)
                return;

            GameObject arrow =
                Instantiate(
                    arrowPrefab,
                    arrowPoint.position,
                    Quaternion.identity
                );

            Vector2 direction =
                (
                    player.position -
                    arrowPoint.position
                ).normalized;

            ArrowProjectile projectile =
                arrow.GetComponent<ArrowProjectile>();

            if (projectile != null)
            {
                projectile.Launch(
                    direction
                );
            }
        }

        public override void ResetToStart()
        {
            base.ResetToStart();

            attackTimer = 0f;

            if (animator != null)
            {
                animator.SetFloat(
                    "Speed",
                    0f
                );
            }
        }
    }
}