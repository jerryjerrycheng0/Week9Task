using UnityEngine;

namespace GameDevWithMarco.Player
{
    public class Player_Recoil : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D playerRb;
        [SerializeField] private float pushBackForce;

        private void Start()
        {
            if (playerRb == null)
            {
                playerRb = GetComponent<Rigidbody2D>();
            }
        }

        public void ApplyRecoil(Vector2 firingDirection)
        {
            // Push back the player opposite to the firing direction
            Vector2 recoilDirection = -firingDirection.normalized;
            playerRb.AddForce(recoilDirection * pushBackForce * 100);
        }
    }
}
