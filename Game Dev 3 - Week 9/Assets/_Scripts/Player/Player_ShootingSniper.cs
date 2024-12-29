using UnityEngine;
using GameDevWithMarco.DesignPattern;
using System.Collections;

namespace GameDevWithMarco.Player
{
    public class Player_ShootingSniper : MonoBehaviour
    {
        [Header("Shooting Variables")]
        [SerializeField] private Transform tipOfTheBarrel; // Assigned dynamically
        [SerializeField] private float bulletSpeed; // Bullet speed
        [SerializeField] private GameEvent bulletShot; // Event to raise when bullet is shot
        [SerializeField] private ParticleSystem sparks; // Particle system for sparks
        [SerializeField] private GameObject muzzleFlash; // Muzzle flash effect

        [Header("Reloading Variables")]
        [SerializeField] private float reloadTime = 2f; // Time it takes to reload the sniper rifle
        [SerializeField] private bool isReloading = false; // Track reload state
        [SerializeField] private int maxAmmo = 5; // Max ammo count
        [SerializeField] private int currentAmmo; // Current ammo count

        private Transform weaponTransform; // Current weapon's transform
        private Player_Recoil playerRecoil; // Reference to the recoil script

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
                bulletShot.Raise();
                sparks.Play();

                // Decrease ammo count
                currentAmmo--;
            }
        }

        // Reloading logic with a cooldown
        private IEnumerator Reload()
        {
            isReloading = true;

            // Trigger reload animation or effects if needed

            yield return new WaitForSeconds(reloadTime); // Wait for the reload time to finish

            currentAmmo = maxAmmo; // Set ammo to max after reloading
            isReloading = false; // Reload complete
        }
    }
}
