using System.Collections.Generic;
using UnityEngine;

namespace GameDevWithMarco.DesignPattern
{
    [CreateAssetMenu(fileName = "WeaponPoolData", menuName = "Scriptable Objects/WeaponPoolData", order = 1)]
    public class WeaponPoolData : ScriptableObject
    {
        public string poolName; 
        public GameObject weaponPrefab; 
        public int poolAmount = 10; 
        public List<GameObject> pooledObjects = new List<GameObject>(); 

        // Method to initialize the pool
        public void InitializePool()
        {
            // Instantiate the objects and set them inactive initially
            for (int i = 0; i < poolAmount; i++)
            {
                GameObject weapon = Instantiate(weaponPrefab);
                weapon.SetActive(false); 
                pooledObjects.Add(weapon);
            }
        }

        // Method to get a weapon from the pool
        public GameObject GetWeapon()
        {
            foreach (GameObject weapon in pooledObjects)
            {
                if (!weapon.activeInHierarchy) 
                {
                    weapon.SetActive(true);
                    return weapon;
                }
            }
            
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
