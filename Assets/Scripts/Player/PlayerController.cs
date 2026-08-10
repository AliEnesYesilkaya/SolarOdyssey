using UnityEngine;
using UnityEngine.InputSystem;

namespace SolarOdyssey.Player
{

    public class PlayerController : MonoBehaviour
    {

        private PlayerInput playerInput;
        private PlayerMovement PlayerMovement;
        private InputAction moveAction;
        private InputAction jumpAction;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            PlayerMovement = GetComponent<PlayerMovement>();
            // ınputsystem haritasından move u yakala 
            moveAction = playerInput.actions["Move"];
            jumpAction = playerInput.actions["Jump"];
        }

        void Update()
        {// move aksiyonundaki yön bilgisini oku 
            Vector2 moveInput = moveAction.ReadValue<Vector2>();
            //okunan değer hareket etmesi için playermovement sınıfına aktar.
            PlayerMovement.Move(moveInput);
            if (jumpAction.triggered)
            {
                PlayerMovement.Jump();
            }
        }
    }
}
