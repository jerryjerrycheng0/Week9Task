using GameDevWithMarco.DesignPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevWithMarco.Player
{
    public class Player_Bullet : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            //To get where specifically I have collided 
            foreach (ContactPoint2D contact in collision.contacts)
            {
                Vector2 collisionPoint = contact.point;//Add a cool muzzleflash in collision
                float randomRange = Random.Range(05f, 1.5f);

                var flashObj = ObjectPoolingPattern.Instance.GetPoolItem(ObjectPoolingPattern.TypeOfPool.MuzzleFlash);

                flashObj.transform.localScale = new Vector3(randomRange, randomRange, randomRange);

                flashObj.transform.position = collisionPoint;

                var flash = flashObj.GetComponent<Player_MuzzleFlash>();

                StartCoroutine(flash.ReturnToPool());
                StartCoroutine(DeactivateAfter());

                //Destroy(flashObj, 0.5f);
            }

        }

        IEnumerator DeactivateAfter()
        {
            yield return new WaitForSeconds(1.1f);
            
            gameObject.SetActive(false);
        }
    }
}
