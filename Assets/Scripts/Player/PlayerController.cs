using System.Collections;
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
        private AttackHitbox attackHitbox;

        private PlayerAudio playerAudio;


        private bool isAttacking;

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

            // Player'ın altındaki AttackHitbox'ı bul
            attackHitbox = GetComponentInChildren<AttackHitbox>();
            playerAudio = GetComponent<PlayerAudio>();
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

                if (PlayerMovement.IsGrounded == false)
                {
                    playerAudio.PlayJump();
                }
            }
            // F tuşuna basınca saldır
            if (Keyboard.current.fKey.wasPressedThisFrame && !isAttacking)
            {
                StartCoroutine(Attack());
            }
        }

        private IEnumerator Attack()
        {
            isAttacking = true;

            // Attack animasyonunu başlat
            animator.SetTrigger("Attack");
            playerAudio.PlayAttack();

            // Saldırının vuruş anına kadar bekle
            yield return new WaitForSeconds(0.2f);

            // Hasar alanını aç
            attackHitbox.EnableHitbox();

            // Hitbox'ın aktif kalacağı süre
            yield return new WaitForSeconds(0.2f);

            // Hasar alanını kapat
            attackHitbox.DisableHitbox();

            isAttacking = false;
        }
    }
}