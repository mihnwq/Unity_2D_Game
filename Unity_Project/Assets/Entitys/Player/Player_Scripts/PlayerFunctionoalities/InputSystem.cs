using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public class InputSystem
    {

       static KeyCode up = KeyCode.W;
       static KeyCode down = KeyCode.S;
       static KeyCode left = KeyCode.A;
       static KeyCode right = KeyCode.D;


        public static int GetHorizontal()
        {
            if (Input.GetKey(right))
                return 1;
            else if (Input.GetKey(left))
                return -1;
            return 0;
        }

        public static int GetVertical()
        {
            if (Input.GetKey(up))
                return 1;
            else if (Input.GetKey(down))
                return -1;
            return 0;
        }

   
    }
