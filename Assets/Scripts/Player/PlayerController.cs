using SolarOdyssey.Combat;
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

        private Animator animator;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            PlayerMovement = GetComponent<PlayerMovement>();

            // Input System haritasından Move ve Jump aksiyonlarını yakala
            moveAction = playerInput.actions["Move"];
            jumpAction = playerInput.actions["Jump"];

            // Animator Visual objesinin içinde olduğu için
            // Player'ın altındaki Animator'ı buluyoruz.
            animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            // Move aksiyonundaki yön bilgisini oku
            Vector2 moveInput = moveAction.ReadValue<Vector2>();

            // Okunan değer hareket etmesi için PlayerMovement sınıfına aktar.
            PlayerMovement.Move(moveInput);

            // Animator'a yatay hareket bilgisini gönder.
            animator.SetFloat("Speed", Mathf.Abs(moveInput.x));

            // Animator'a yerde olup olmadığımızı gönder.
            animator.SetBool("IsGrounded", PlayerMovement.IsGrounded);

            // Jump aksiyonu tetiklendiyse zıpla
            if (jumpAction.triggered)
            {
                PlayerMovement.Jump();
                Health health = GetComponent<Health>();
                health.TakeDamage(10);
            }
            if (Keyboard.current.fKey.wasPressedThisFrame)
            {
                animator.SetTrigger("Attack");
            }
        }
    }
}