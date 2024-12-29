using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevWithMarco.Player;

namespace GameDevWithMarco.Interfaces
{
    public class PickUps : MonoBehaviour, IWeapon
    {
        public bool Dropped = true;
        public bool Gun = true; // Indicates if this is a shotgun (true) or sniper (false)
        [SerializeField] Player_Movement playerMovement;

        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                // Pick up the weapon
                PickUp();
                gameObject.SetActive(false); // Hide the pickup object
                Dropped = false;
            }
        }

        public void PickUp()
        {
            // Activate the correct weapon in the Player_Movement script
            if (Gun)
            {
                playerMovement.EquipWeapon(playerMovement.shotgunPrefab);
            }
            else
            {
                playerMovement.EquipWeapon(playerMovement.sniperPrefab);
            }

            // Enable switching weapons once both are picked up
            if (playerMovement.isShotgun && playerMovement.isSniper)
            {
                playerMovement.EnableWeaponSwitching();
            }
        }
    }
}
