using UnityEngine;
using GameDevWithMarco.DesignPattern;
using GameDevWithMarco.CameraStuff;

namespace GameDevWithMarco.Player
{
    public class Player_Shooting : MonoBehaviour
    {
        [Header("Shooting Variables")]
        [SerializeField] Transform tipOfTheBarrel; // Assigned dynamically
        [SerializeField] private float bulletSpeed;
        [SerializeField] private GameEvent bulletShot;
        [SerializeField] private ParticleSystem sparks;
        [SerializeField] private GameObject muzzleFlash; // Muzzle flash particle system

        [Header("Reloading Variables")]
        private Transform weaponTransform; // Current weapon's transform
        private Player_Recoil playerRecoil;

        [Header("Sound")]
        [SerializeField] AudioSource shootingSound;

        private void Start()
        {
            playerRecoil = FindObjectOfType<Player_Recoil>();
        }

        private void Update()
        {
            if (Input.GetButtonDown("Fire1") && tipOfTheBarrel != null)
            {
                Fire();
            }
        }

        public void BindWeapon(Transform weapon)
        {
            weaponTransform = weapon;
            tipOfTheBarrel = weaponTransform.Find("TipOfTheBarrel");
        }

        private void Fire()
        {
            CameraRippleEffect.Instance.Ripple(tipOfTheBarrel.transform.position);
            // Spawn a bullet
            GameObject spawnedBullet = ObjectPoolingPattern.Instance.GetPoolItem(ObjectPoolingPattern.TypeOfPool.BulletPool);

            if (spawnedBullet != null && tipOfTheBarrel != null)
            {
                spawnedBullet.transform.position = tipOfTheBarrel.position;
                spawnedBullet.transform.rotation = tipOfTheBarrel.rotation;

                Rigidbody2D bulletsRb = spawnedBullet.GetComponent<Rigidbody2D>();
                bulletsRb.AddForce(tipOfTheBarrel.right * bulletSpeed * 100);

                // Apply recoil
                playerRecoil?.ApplyRecoil(tipOfTheBarrel.right);

                if (muzzleFlash != null)
                {
                    GameObject flash = Instantiate(muzzleFlash, tipOfTheBarrel.position, tipOfTheBarrel.rotation);
                    Destroy(flash, 0.1f); // Destroy the flash after a short duration
                }
            }

            TriggerEffect();
        }

        private void TriggerEffect()
        {
            // Trigger effects
            bulletShot.Raise();
            sparks.Play();
            shootingSound.Play();
        }
    }
}
