using UnityEngine;
using GameDevWithMarco.CameraStuff;
using GameDevWithMarco.DesignPattern;

namespace GameDevWithMarco.Player
{
    public class Player_Shooting : MonoBehaviour
    {
        [SerializeField] Transform tipOfTheBarrel;
        [SerializeField] Transform ejectionPort;
        [SerializeField] float bulletSpeed;
        Player_Movement playerMovementRef;
        Rigidbody2D rb;
        [SerializeField] float pushBackForce;
        [SerializeField] GameEvent bulletShot;
        [SerializeField] GameObject muzzleFlash;
        [SerializeField] ParticleSystem sparks;

        private void Start()
        {
            playerMovementRef = GetComponent<Player_Movement>();
            rb = GetComponent<Rigidbody2D>();
        }
        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Fire();
            }
        }

        void Fire()
        {
            //Spawns the bullet
            GameObject spawnedBullet = ObjectPoolingPattern.Instance.GetPoolItem(ObjectPoolingPattern.TypeOfPool.BulletPool);

            //Make the bullet be in the right position
            if (spawnedBullet != null) spawnedBullet.transform.position = tipOfTheBarrel.transform.position;

            //Random bullet scale
            RandomiseBulletSize(spawnedBullet);

            //Fires the bullet
            Rigidbody2D bulletsRb = spawnedBullet.GetComponent<Rigidbody2D>();
            FireBulletInRightDirection(bulletsRb);

            //Does a pushback
            PushBack();
            //Raises the event
            bulletShot.Raise();

            //Fires the ripple effect
            CameraRippleEffect.Instance.Ripple(tipOfTheBarrel.transform.position);

            //Muzzle flash code
            MuzzleFlashLogic();

            //Plays the sparks particles
            sparks.Play();
        }

        private void MuzzleFlashLogic()
        {
            var muzzleFlashObject = ObjectPoolingPattern.Instance.GetPoolItem(ObjectPoolingPattern.TypeOfPool.MuzzleFlash);
            float randomValue = Random.Range(0.8f, 1.25f);

            muzzleFlashObject.transform.localScale = new Vector3(randomValue, randomValue, randomValue);

            var muzzleFlashScript = muzzleFlashObject.GetComponent<Player_MuzzleFlash>();

            StartCoroutine(muzzleFlashScript.ReturnToPool());

            muzzleFlashObject.transform.position = tipOfTheBarrel.position;
        }

        private void FireBulletInRightDirection(Rigidbody2D bulletsRb)
        {
            if (playerMovementRef.facingRight == true)
            {
                bulletsRb.AddForce(Vector2.right * bulletSpeed * 100);
            }
            else
            {
                bulletsRb.AddForce(Vector2.left * bulletSpeed * 100);
            }
        }

        private void RandomiseBulletSize(GameObject spawnedBullet)
        {
            float randomScaleValue = Random.Range(0.7f, 1.3f);
            spawnedBullet.transform.localScale = new Vector3(randomScaleValue, randomScaleValue, randomScaleValue);
        }

        public void PushBack()
        {
            if (playerMovementRef.facingRight == true)
            {
                rb.AddForce(Vector2.left * pushBackForce * 100);
            }
            else
            {
                rb.AddForce(Vector2.right * pushBackForce * 100);
            }
        }
    }
}
