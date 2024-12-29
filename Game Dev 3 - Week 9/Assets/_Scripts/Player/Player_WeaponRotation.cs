using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevWithMarco.Player
{
    public class Player_WeaponRotation : MonoBehaviour
    {
        private Vector2 mousePosition; // To store the mouse position
        private float angle; // To store the calculated angle
        [SerializeField] Transform weaponTransform; // Reference to the weapon's Transform

        [SerializeField] private Transform target; // The target to follow (e.g., player's hand)

        private void Update()
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        private void FixedUpdate()
        {
            if (weaponTransform == null) return;

            RotateWeapon();
            UpdateWeaponPosition();
        }

        public void BindWeapon(Transform weapon)
        {
            weaponTransform = weapon;
        }

        private void RotateWeapon()
        {
            // Calculate direction to look towards the mouse
            Vector2 lookDirection = mousePosition - (Vector2)weaponTransform.position;
            angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            weaponTransform.rotation = Quaternion.Euler(0, 0, angle);
        }

        private void UpdateWeaponPosition()
        {
            weaponTransform.position = target.position;
        }
    }
}
