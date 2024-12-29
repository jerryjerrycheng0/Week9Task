using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevWithMarco.DesignPattern
{
    [CreateAssetMenu(fileName = "WeaponPoolData", menuName = "Scriptable Objects/WeaponPoolData", order = 1)]
    public class WeaponPoolData : ScriptableObject
    {
        public string poolName; // Name of the pool (e.g., "ShotgunPool")
        public GameObject weaponPrefab; // The weapon prefab for this pool
        public int poolAmount = 10; // Amount of objects to pool
        public List<GameObject> pooledObjects = new List<GameObject>(); // List to hold pooled objects

        // Method to initialize the pool
        public void InitializePool()
        {
            // Instantiate the objects and set them inactive initially
            for (int i = 0; i < poolAmount; i++)
            {
                GameObject weapon = Instantiate(weaponPrefab);
                weapon.SetActive(false); // Disable the object initially
                pooledObjects.Add(weapon);
            }
        }

        // Method to get a weapon from the pool
        public GameObject GetWeapon()
        {
            foreach (GameObject weapon in pooledObjects)
            {
                if (!weapon.activeInHierarchy) // If the weapon is not active, return it from the pool
                {
                    weapon.SetActive(true);
                    return weapon;
                }
            }
            // If no inactive weapon is available, return null or instantiate a new one
            Debug.LogWarning("No available weapons in the pool for " + poolName);
            return null;
        }

        // Method to return a weapon to the pool
        public void ReturnWeaponToPool(GameObject weapon)
        {
            weapon.SetActive(false);
        }
    }
}
