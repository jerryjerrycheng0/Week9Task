using System.Collections;
using UnityEngine;

namespace GameDevWithMarco.Player
{
    public class Player_MuzzleFlash : MonoBehaviour
    {
       public IEnumerator ReturnToPool()
        {
            yield return new WaitForSeconds(1);

            gameObject.SetActive(false);
        }
    }
}
