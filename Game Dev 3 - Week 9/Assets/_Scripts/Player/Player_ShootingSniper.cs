using UnityEngine;
using GameDevWithMarco.CameraStuff;
using GameDevWithMarco.DesignPattern;
using GameDevWithMarco.Interfaces;

namespace GameDevWithMarco.Player
{
    public class Player_ShootingSniper : MonoBehaviour
    {
        [SerializeField] Transform tipOfTheBarrel;
        [SerializeField] Transform ejectionPort;
        [SerializeField] float bulletSpeed;
        [SerializeField] float pushBackForce;
        [SerializeField] float recoilForce; // Additional recoil force
        [SerializeField] float reloadTime;
        [SerializeField] GameEvent bulletShot;
        [SerializeField] GameObject muzzleFlash;
        [SerializeField] ParticleSystem sparks;
        [SerializeField] Rigidbody2D rb; // Reference to the player's Rigidbody2D
        private bool canShoot = true;

        private void Start()
        {

        }

        void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Fire();
            }
        }

        void Fire()
        {
            if (!canShoot) return;

            canShoot = false;

            // Spawns the bullet
            GameObject spawnedBullet = ObjectPoolingPattern.Instance.GetPoolItem(ObjectPoolingPattern.TypeOfPool.BulletPool);

            // Make the bullet be in the right position
            if (spawnedBullet != null)
            {
                spawnedBullet.transform.position = tipOfTheBarrel.transform.position;
                spawnedBullet.transform.rotation = tipOfTheBarrel.transform.rotation;
            }

            // Fires the bullet
            Rigidbody2D bulletsRb = spawnedBullet.GetComponent<Rigidbody2D>();
            Vector2 firingDirection = tipOfTheBarrel.right; // 'right' points in the local x-axis direction of the barrel
            bulletsRb.AddForce(firingDirection * bulletSpeed * 100);

            // Apply recoil
            ApplyRecoil(firingDirection);

            // Trigger effects
            bulletShot.Raise();
            MuzzleFlashLogic();
            sparks.Play();
            CameraRippleEffect.Instance.Ripple(tipOfTheBarrel.transform.position);

            // Reload logic
            Invoke(nameof(Reload), reloadTime);
        }

        private void ApplyRecoil(Vector2 firingDirection)
        {
            // Push back the player in the opposite direction
            Vector2 recoilDirection = -firingDirection.normalized;
            rb.AddForce(recoilDirection * recoilForce * 100);

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


        private void Reload()
        {
            canShoot = true;
        }
    }
}
