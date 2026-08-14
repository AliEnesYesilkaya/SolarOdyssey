using UnityEngine;

namespace SolarOdyssey.Enemy
{
    public class StateMachine
    {
        private IState currentState;

        public IState CurrentState => currentState;

        public void Initialize(IState startingState)
        {
            currentState = startingState;
            currentState.Enter();
        }

        public void ChangeState(IState newState)
        {
            currentState?.Exit();

            currentState = newState;

            currentState.Enter();
        }

        public void FixedUpdate()
        {
            currentState?.Tick();
        }
    }
}