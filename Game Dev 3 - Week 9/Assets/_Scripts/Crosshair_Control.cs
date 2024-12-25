using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevWithMarco.Crosshair
{
    public class Crosshair_Control : MonoBehaviour
    {
        private Vector2 mousePosition;

        // Start is called before the first frame update
        void Start()
        {
            transform.localScale = new Vector3(10, 10, 0);
            Cursor.visible = false;     //Sets the cursor to be invisible
        }

        // Update is called once per frame
        void Update()
        {
            SetCrosshairPosition();
        }

        private void SetCrosshairPosition()
        {
            //Gets the mouse poistion in world points
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Moves the position of whatever this script is in to the mouse position
            transform.position = mousePosition;
        }


    }
}
