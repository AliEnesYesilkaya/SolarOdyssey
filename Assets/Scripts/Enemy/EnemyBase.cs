using SolarOdyssey.Combat;
using UnityEngine;

namespace SolarOdyssey.Enemy
{
    public class EnemyBase : MonoBehaviour
    {
        [Header("Enemy Settings")]
        [SerializeField] protected float moveSpeed = 2f;
        [SerializeField] protected float detectionRange = 5f;
        [SerializeField] protected float attackRange = 1.2f;
        [SerializeField] protected int attackDamage = 20;

        protected Transform player;
        protected Rigidbody2D rb;
        protected Animator animator;

        protected StateMachine stateMachine;

        protected virtual void Awake()
        {
            GameObject playerObject =
                GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }

            rb = GetComponent<Rigidbody2D>();

            animator = GetComponentInChildren<Animator>();

            stateMachine = new StateMachine();
        }

        protected virtual void Update()
        {
            FacePlayer();
        }

        protected virtual void FixedUpdate()
        {
            stateMachine?.FixedUpdate();
        }

        protected void FacePlayer()
        {
            if (player == null)
                return;

            Transform visual = transform.Find("Visual");

            if (visual == null)
                return;

            float direction = Mathf.Sign(
                player.position.x - transform.position.x
            );

            Vector3 scale = visual.localScale;

            scale.x = Mathf.Abs(scale.x) * direction;

            visual.localScale = scale;
        }

        // Animasyon çağrılacak 
        public void AttackHit()
        {
            if (player == null)
                return;

            float distance = Vector2.Distance(
                transform.position,
                player.position
            );

            // Oyuncu gerçekten saldırı menzilindeyse hasar ver.
            if (distance > attackRange)
                return;

            Health health = player.GetComponent<Health>();

            if (health != null)
            {
                health.TakeDamage(attackDamage);

                Debug.Log(
                    gameObject.name +
                    " oyuncuya " +
                    attackDamage +
                    " hasar verdi."
                );
            }
        }
    }
}