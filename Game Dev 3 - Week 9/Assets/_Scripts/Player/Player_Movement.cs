using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevWithMarco.Interfaces;

namespace GameDevWithMarco.Player
{
    public class Player_Movement : MonoBehaviour
    {
        //--- References and movement variables ---
        Rigidbody2D rb;

        [Header("Movement Control Variables")]
        [Range(1f, 5f)] public float speed;
        [Range(1f, 20f)] public float jumpStrenght;
        Vector2 movementValue;
        public bool facingRight = true;

        [Header("Movement Juice Variables")]
        public float maxSpeed = 7f;
        public float newLinearDrag = 4f;

        [Header("Ground Check Variables")]
        public bool amIGrounded = true;
        public LayerMask whatIsGround;
        public Transform raycastLeftPos, raycastRightPos;
        public float rayLenght;

        [Header("Jump Juice Variables")]
        public float gravityValue = 1f;
        public float fallMultiplier = 5f;

        [Header("Guns")]
        public GameObject shotgunPrefab;
        public GameObject sniperPrefab;
        public bool isShotgun = false;
        public bool isSniper = false;
        [SerializeField] AudioSource gunSound;

        private bool canSwitchWeapons = false;  // To check if both weapons are picked up

        //------- Built-in Methods -------
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            shotgunPrefab.SetActive(false);
            sniperPrefab.SetActive(false);
        }

        void Update()
        {
            // Switch between weapons if both are picked up
            SwitchGuns();
            GetMovementValue();

            if (Input.GetButtonDown("Jump") && amIGrounded)
            {
                Jump();
            }

            GroundCheck();
        }

        private void FixedUpdate()
        {
            HorizontalMovement();
            ModifyPhysics();
        }

        // Custom Methods for movement
        private void GetMovementValue()
        {
            movementValue = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        private void HorizontalMovement()
        {
            rb.AddForce(Vector2.right * movementValue.x * speed * 10);
            CheckIfIShouldFlip();
            LimitPlayerVelocity();
        }

        private void LimitPlayerVelocity()
        {
            if (Mathf.Abs(rb.velocity.x) > maxSpeed)
            {
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
            }
        }

        private void CheckIfIShouldFlip()
        {
            if ((movementValue.x > 0 && !facingRight) || (movementValue.x < 0 && facingRight))
            {
                Flip();
            }
        }

        void Flip()
        {
            facingRight = !facingRight;
            transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
        }

        void ModifyPhysics()
        {
            bool changingDirection = (movementValue.x > 0 && rb.velocity.x < 0) || (movementValue.x < 0 && rb.velocity.x > 0);
            if (amIGrounded)
            {
                if (Mathf.Abs(movementValue.x) < 0.4f || changingDirection)
                {
                    rb.drag = newLinearDrag;
                }
                else
                {
                    rb.drag = 0;
                }

                rb.gravityScale = 0;
            }
            else
            {
                rb.gravityScale = gravityValue;
                rb.drag = newLinearDrag * 0.15f;

                if (rb.velocity.y < 0)
                {
                    rb.gravityScale = gravityValue * fallMultiplier;
                }
                else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
                {
                    rb.gravityScale = gravityValue * (fallMultiplier / 2);
                }
            }
        }

        private void Jump()
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpStrenght, ForceMode2D.Impulse);
        }

        private void GroundCheck()
        {
            bool isTouchingOnTheLeft = Physics2D.Raycast(raycastLeftPos.position, Vector2.down, rayLenght, whatIsGround);
            bool isTouchingOnTheRight = Physics2D.Raycast(raycastRightPos.position, Vector2.down, rayLenght, whatIsGround);
            RaycastVisualDebug(isTouchingOnTheLeft, isTouchingOnTheRight);

            if (isTouchingOnTheLeft || isTouchingOnTheRight)
            {
                amIGrounded = true;
            }
            else
            {
                amIGrounded = false;
            }
        }

        private void RaycastVisualDebug(bool isTouchingOnTheLeft, bool isTouchingOnTheRight)
        {
            if (isTouchingOnTheLeft)
            {
                Debug.DrawRay(raycastLeftPos.position, Vector2.down * rayLenght, Color.green);
            }
            else
            {
                Debug.DrawRay(raycastLeftPos.position, Vector2.down * rayLenght, Color.red);
            }
            if (isTouchingOnTheRight)
            {
                Debug.DrawRay(raycastRightPos.position, Vector2.down * rayLenght, Color.green);
            }
            else
            {
                Debug.DrawRay(raycastRightPos.position, Vector2.down * rayLenght, Color.red);
            }
        }

        private void SwitchGuns()
        {
            // Only switch guns if both are picked up
            if (canSwitchWeapons && Input.GetKeyDown(KeyCode.LeftShift))
            {
                gunSound.Play();
                if (isShotgun && isSniper)
                {
                    // Toggle between weapons
                    if (shotgunPrefab.activeSelf)
                    {
                        shotgunPrefab.SetActive(false);
                        sniperPrefab.SetActive(true);
                    }
                    else
                    {
                        shotgunPrefab.SetActive(true);
                        sniperPrefab.SetActive(false);
                    }
                }
            }
        }

        // Call this method to enable weapon switching after both are picked up
        public void EnableWeaponSwitching()
        {
            canSwitchWeapons = true;
        }

        // Method to enable only one weapon
        public void EquipWeapon(GameObject weaponPrefab)
        {
            gunSound.Play();
            gunSound.pitch = 0.8f;
            if (weaponPrefab == shotgunPrefab)
            {
                isShotgun = true;
                shotgunPrefab.SetActive(true);
                sniperPrefab.SetActive(false);
            }
            else if (weaponPrefab == sniperPrefab)
            {
                isSniper = true;
                sniperPrefab.SetActive(true);
                shotgunPrefab.SetActive(false);
            }
        }
    }
}
