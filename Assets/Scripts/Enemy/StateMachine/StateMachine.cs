using Unity.VisualScripting;
using UnityEngine;

namespace SolarOdyssey.Enemy
{
    public class StateMachine
    {
        private IState currentState;
        public IState CurrentState => currentState;
        public void Initialize(IState startingState)//başlangıç durumu ayarlayan metod 
        {
            currentState = startingState;
            currentState.Enter();
        }

        public void ChangeState(IState newState)// durum değişme metodu 
        {
            currentState?.Exit();

            currentState = newState;

            currentState.Enter();
        }

        public void Update() //aktif durumun sürekli çalışması için tick uygula 
        {
            currentState?.Tick();
        }
    }
}