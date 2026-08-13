using UnityEngine;

namespace SolarOdyssey.Enemy
{
    public class EnemyBase : MonoBehaviour
    {
        [Header("Enemy Settings")]
        [SerializeField] protected float moveSpeed = 2f;
        [SerializeField] protected float detectionRange = 5f;

        protected Transform player;

        protected StateMachine stateMachine;

        protected virtual void Awake()//oyuncu tagına ulaşırsa onun konumunu alır ve state machinei uygular 
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }

            stateMachine = new StateMachine();
        }

        protected virtual void Update()
        {
            stateMachine?.Update();
        }
    }
}