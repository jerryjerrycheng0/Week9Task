using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevWithMarco.Player
{
    public class Player_Bullet : MonoBehaviour
    {
        [SerializeField] GameObject flash;
        private void OnCollisionEnter2D(Collision2D collision)
        {
            //To get where specifically I have collided 
            foreach (ContactPoint2D contact in collision.contacts)
            {
                Vector2 collisionPoint = contact.point;//Add a cool muzzleflash in collision
                float randomRange = Random.Range(05f, 1.5f);
                var flashObj = Instantiate(flash, collisionPoint, Quaternion.identity);
                flashObj.transform.localScale = new Vector3(randomRange, randomRange, randomRange);
                Destroy(flashObj, 0.5f);
            }
            //Returns the game object to the available pool
            gameObject.SetActive(false);
        }
    }
}
