using UnityEngine;

namespace SolarOdyssey.Enemy
{
    public class ChaseState : IState
    {
        private Transform enemy;
        private Transform player;
        private float moveSpeed;

        public ChaseState(Transform enemy, Transform player, float moveSpeed)
        {
            this.enemy = enemy;
            this.player = player;
            this.moveSpeed = moveSpeed;
        }

        public void Enter()
        {
            Debug.Log("Chase State başladı");
        }

        public void Tick() //düşman varsa yön vektörünü hesapla ve takip et 
        {
            if (player == null)
                return;

            Vector3 direction = (player.position - enemy.position).normalized;

            enemy.position += direction * moveSpeed * Time.deltaTime;
        }

        public void Exit()
        {
            Debug.Log("Chase State bitti");
        }
    }
}