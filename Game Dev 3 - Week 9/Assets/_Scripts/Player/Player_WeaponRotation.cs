using UnityEngine;

namespace GameDevWithMarco.Player
{
    public class Player_WeaponRotation : MonoBehaviour
    {
        private Vector2 mousePosition; 
        private float angle; 
        [SerializeField] Transform weaponTransform; 

        [SerializeField] private Transform target; 

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
