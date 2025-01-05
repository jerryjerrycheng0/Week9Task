using System.Collections;
using UnityEngine;
using GameDevWithMarco.DesignPattern;
using GameDevWithMarco.CameraStuff;

namespace GameDevWithMarco.Player
{
    public class Player_ShootingSniper : MonoBehaviour
    {
        [Header("Shooting Variables")]
        [SerializeField] private Transform tipOfTheBarrel;
        [SerializeField] private float bulletSpeed; 
        [SerializeField] private GameEvent bulletShot; 
        [SerializeField] private ParticleSystem sparks; 
        [SerializeField] private GameObject muzzleFlash;

        [Header("Reloading Variables")]
        [SerializeField] private float reloadTime = 2f; 
        [SerializeField] private bool isReloading = false; 
        [SerializeField] private int maxAmmo = 5;
        [SerializeField] private int currentAmmo; 

        [Header("Sounds")]
        [SerializeField] AudioSource shootingSound;
        [SerializeField] AudioSource reloadSound;

        private Transform weaponTransform;
        private Player_Recoil playerRecoil; 

        private void Start()
        {
            playerRecoil = FindObjectOfType<Player_Recoil>();
            currentAmmo = maxAmmo; // Start with max ammo
        }

        private void Update()
        {
            if (isReloading)
            {
                return; // If reloading, don't allow shooting
            }

            if (Input.GetButtonDown("Fire1") && tipOfTheBarrel != null && currentAmmo > 0)
            {
                Fire();
            }

            // Handle reloading
            if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
            {
                StartCoroutine(Reload());
            }
        }

        // Bind weapon when it is switched or equipped
        public void BindWeapon(Transform weapon)
        {
            weaponTransform = weapon;
            tipOfTheBarrel = weaponTransform.Find("TipOfTheBarrel");
        }

        // Fire method - shooting the sniper rifle
        private void Fire()
        {
            CameraRippleEffect.Instance.Ripple(tipOfTheBarrel.transform.position);
            // Spawn a bullet from the pool
            GameObject spawnedBullet = ObjectPoolingPattern.Instance.GetPoolItem(ObjectPoolingPattern.TypeOfPool.BulletPool);

            if (spawnedBullet != null && tipOfTheBarrel != null)
            {
                spawnedBullet.transform.position = tipOfTheBarrel.position;
                spawnedBullet.transform.rotation = tipOfTheBarrel.rotation;

                Rigidbody2D bulletsRb = spawnedBullet.GetComponent<Rigidbody2D>();
                bulletsRb.AddForce(tipOfTheBarrel.right * bulletSpeed * 100); // Apply force for the bullet

                // Apply recoil after firing
                playerRecoil?.ApplyRecoil(tipOfTheBarrel.right);

                // Trigger muzzle flash effect
                if (muzzleFlash != null)
                {
                    GameObject flash = Instantiate(muzzleFlash, tipOfTheBarrel.position, tipOfTheBarrel.rotation);
                    Destroy(flash, 0.1f); // Destroy the muzzle flash after a short duration
                }

                // Trigger shooting effects
                TriggerEffects();
            }
        }

        private void TriggerEffects()
        {
            bulletShot.Raise();
            sparks.Play();
            shootingSound.Play();

            // Decrease ammo count
            currentAmmo--;
        }

        // Reloading logic with a cooldown
        private IEnumerator Reload()
        {
            isReloading = true;
            reloadSound.Play();

            // Trigger reload animation or effects if needed

            yield return new WaitForSeconds(reloadTime); // Wait for the reload time to finish

            currentAmmo = maxAmmo; // Set ammo to max after reloading
            isReloading = false; // Reload complete
        }
    }
}
