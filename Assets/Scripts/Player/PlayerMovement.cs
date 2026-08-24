using UnityEngine;

namespace SolarOdyssey.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private Rigidbody2D rb;
        private PlayerAudio playerAudio;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;

        [Header("Jump Settings")]
        [SerializeField] private float jumpForce = 7f;

        private bool isGrounded;
        private bool isTouchingWall;

        public bool IsGrounded => isGrounded;
        public bool IsTouchingWall => isTouchingWall;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            playerAudio = GetComponent<PlayerAudio>();
        }

        public void Move(Vector2 moveInput)
        {
            rb.linearVelocity = new Vector2(
                moveInput.x * moveSpeed,
                rb.linearVelocity.y
            );

            Flip(moveInput.x);
        }

        public void Jump()
        {
            if (!isGrounded)
                return;

            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce
            );

            isGrounded = false;
        }

        private void Flip(float horizontalInput)
        {
            if (horizontalInput > 0)
            {
                transform.localScale =
                    new Vector3(2f, 2f, 2f);
            }
            else if (horizontalInput < 0)
            {
                transform.localScale =
                    new Vector3(-2f, 2f, 2f);
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("Ground"))
                return;

            bool wasGrounded = isGrounded;

            isGrounded = false;
            isTouchingWall = false;

            foreach (ContactPoint2D contact in collision.contacts)
            {
                // Gerçek zemin teması
                if (contact.normal.y > 0.5f)
                {
                    isGrounded = true;
                }

                // Duvar teması
                if (Mathf.Abs(contact.normal.x) > 0.5f)
                {
                    isTouchingWall = true;
                }
            }

            // Havadan yere yeni indiysek
            if (!wasGrounded &&
                isGrounded &&
                playerAudio != null)
            {
                playerAudio.PlayLand();
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("Ground"))
                return;

            isGrounded = false;
            isTouchingWall = false;
        }
    }
}