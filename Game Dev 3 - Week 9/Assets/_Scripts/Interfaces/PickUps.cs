using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevWithMarco.Player;

namespace GameDevWithMarco.Interfaces
{
    public class PickUps : MonoBehaviour, IWeapon
    {
        bool Dropped;
        void OnCollisionEnter2D(Collision2D col)
        {
            if(col.gameObject.CompareTag ("Player"))
            {
                col.gameObject.SetActive(false);
            }
        }
        public void PickUp()
        {

        }
    }
}
