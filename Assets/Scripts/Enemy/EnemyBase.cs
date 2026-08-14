using UnityEngine;

namespace SolarOdyssey.Enemy
{
    public class EnemyBase : MonoBehaviour
    {
        [Header("Enemy Settings")]
        [SerializeField] protected float moveSpeed = 2f;
        [SerializeField] protected float detectionRange = 5f;

        protected Transform player;
        protected Rigidbody2D rb;

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

            stateMachine = new StateMachine();
        }

        protected virtual void Update()
        {
            // AI kararları burada yapılır.
        }

        protected virtual void FixedUpdate()
        {
            // Fizik tabanlı hareket burada çalışır.
            stateMachine?.FixedUpdate();
        }
    }
}