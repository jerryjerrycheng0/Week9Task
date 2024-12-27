using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevWithMarco.DesignPattern
{
    public class ObjectPoolingPattern : Singleton<ObjectPoolingPattern>
    {
        [SerializeField] PoolData bulletPool;
        [SerializeField] PoolData muzzleFlashPool;


        public enum TypeOfPool
        {
            BulletPool,
            MuzzleFlash,

        }

        public GameObject currentWeapon; // Tracks the currently active weapon

        // Start is called before the first frame update
        void Start()
        {
            FillThePool(bulletPool);
            FillThePool(muzzleFlashPool);

        }

        private void FillThePool(PoolData poolData)
        {
            poolData.ResetThePool();

            for (int i = 0; i < poolData.poolAmount; i++)
            {
                GameObject thingToAddToPool = Instantiate(poolData.poolItem);
                thingToAddToPool.transform.parent = transform;
                thingToAddToPool.SetActive(false);
                poolData.pooledObjectContainer.Add(thingToAddToPool);
            }
        }

        public GameObject GetPoolItem(TypeOfPool poolToUse)
        {
            PoolData pool = ScriptableObject.CreateInstance<PoolData>();
            switch (poolToUse)
            {
                case TypeOfPool.BulletPool:
                    pool = bulletPool;
                    break;
                case TypeOfPool.MuzzleFlash:
                    pool = muzzleFlashPool;
                    break;

            }

            for (int i = 0; i < pool.pooledObjectContainer.Count; i++)
            {
                if (!pool.pooledObjectContainer[i].activeInHierarchy)
                {
                    pool.pooledObjectContainer[i].SetActive(true);
                    return pool.pooledObjectContainer[i];
                }
            }

            Debug.LogWarning("No Available Items Found or Pool Too Small!");
            return null;
        }

    }
}
