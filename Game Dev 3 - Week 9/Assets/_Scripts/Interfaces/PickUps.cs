using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevWithMarco.Player;

namespace GameDevWithMarco.Interfaces
{
    public class PickUps : MonoBehaviour, IWeapon
    {
        public bool Dropped = true;
        public bool Gun = true;
        [SerializeField] Player_Movement playerMovement;
        void OnCollisionEnter2D(Collision2D col)
        {
            if(col.gameObject.CompareTag ("Player"))
            {
                gameObject.SetActive(false);
                Dropped = false;
            }
            PickUp();
           
        }
        public void PickUp()
        {
            if (Gun == true)
            {
                playerMovement.isShotgun = true;
                playerMovement.isSniper = false;
            }
            else
            {
                playerMovement.isShotgun = false;
                playerMovement.isSniper = true;
            }
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                gameObject.SetActive(true);
                Dropped = true;
            }
        }
    }
}
