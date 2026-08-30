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

            Debug.Log(
                "Enemy3 Awake çalıştı."
            );

            if (arrowPrefab == null)
            {
                Debug.LogError(
                    "Enemy3: Arrow Prefab atanmadı!"
                );
            }
            else
            {
                Debug.Log(
                    "Enemy3: Arrow Prefab hazır."
                );
            }

            if (arrowPoint == null)
            {
                Debug.LogError(
                    "Enemy3: Arrow Point atanmadı!"
                );
            }
            else
            {
                Debug.Log(
                    "Enemy3: Arrow Point hazır."
                );
            }
        }

        protected override void Update()
        {
            base.Update();

            if (player == null)
            {
                Debug.LogWarning(
                    "Enemy3: Player bulunamadı!"
                );

                return;
            }

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

            // Oyuncu algılama alanı dışında.
            if (horizontalDistance > detectionRange)
            {
                if (animator != null)
                {
                    animator.SetFloat(
                        "Speed",
                        0f
                    );
                }

                return;
            }

            FacePlayer();

            // Enemy3 yerinde durur.
            rb.linearVelocity =
                new Vector2(
                    0f,
                    rb.linearVelocity.y
                );

            if (animator != null)
            {
                animator.SetFloat(
                    "Speed",
                    0f
                );
            }

            // Oyuncu düşmanın çok üstünde veya
            // altında ise saldırma.
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
            Debug.Log(
                "Enemy3 ATTACK başladı!"
            );

            if (animator != null)
            {
                animator.SetTrigger(
                    "Attack"
                );

                Debug.Log(
                    "Enemy3 Attack Trigger gönderildi."
                );
            }
            else
            {
                Debug.LogError(
                    "Enemy3 Animator bulunamadı!"
                );
            }

            // OK BURADA OLUŞTURULMUYOR.
            //
            // Animation Event saldırı animasyonundaki
            // doğru karede FireArrow() metodunu çağıracak.
        }

        // =================================================
        // ANIMATION EVENT
        // =================================================

        public void FireArrow()
        {
            Debug.Log(
                "Enemy3 FireArrow() ÇALIŞTI!"
            );

            if (arrowPrefab == null)
            {
                Debug.LogError(
                    "Enemy3: Arrow Prefab BOŞ!"
                );

                return;
            }

            if (arrowPoint == null)
            {
                Debug.LogError(
                    "Enemy3: Arrow Point BOŞ!"
                );

                return;
            }

            if (player == null)
            {
                Debug.LogError(
                    "Enemy3: Player bulunamadı!"
                );

                return;
            }

            // Okun oyuncuya doğru gideceği yön.
            Vector2 direction =
                (
                    player.position -
                    arrowPoint.position
                ).normalized;

            Debug.Log(
                "Enemy3 ok oluşturuyor. " +
                "Yön = " +
                direction
            );

            // Ok prefabından yeni bir ok oluştur.
            GameObject arrow =
                Instantiate(
                    arrowPrefab,
                    arrowPoint.position,
                    Quaternion.identity
                );

            Debug.Log(
                "Arrow oluşturuldu: " +
                arrow.name
            );

            // Oluşturulan objenin ArrowProjectile
            // componentini bul.
            ArrowProjectile projectile =
                arrow.GetComponent<ArrowProjectile>();

            if (projectile == null)
            {
                Debug.LogError(
                    "Oluşturulan Arrow üzerinde " +
                    "ArrowProjectile YOK!"
                );

                Destroy(arrow);

                return;
            }

            // Oku oyuncuya doğru fırlat.
            projectile.Launch(
                direction
            );

            Debug.Log(
                "ArrowProjectile.Launch() çağrıldı!"
            );
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

            Debug.Log(
                "Enemy3 ResetToStart çalıştı."
            );
        }
    }
}