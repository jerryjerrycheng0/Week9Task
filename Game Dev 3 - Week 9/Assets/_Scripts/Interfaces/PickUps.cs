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
                playerMovement.isShotgun = true;
                playerMovement.sniperPrefab.SetActive(false);
                playerMovement.shotgunPrefab.SetActive(true);
            }
            else
            {
                playerMovement.isSniper = true;
                playerMovement.sniperPrefab.SetActive(true);
                playerMovement.shotgunPrefab.SetActive(false);
            }
        }
    }
}
