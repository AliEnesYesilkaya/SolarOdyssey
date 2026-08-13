using UnityEngine;

namespace SolarOdyssey.Enemy
{
    public class PatrolState : IState 
        //bir alanı korumak için atılan devrriye yönetimi 
    {
        public void Enter()
        {
            Debug.Log("Patrol State başladı");
        }

        public void Tick()
        {
        }

        public void Exit()
        {
            Debug.Log("Patrol State bitti");
        }
    }
}