using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPC : InputSystem {

    public override float GetAxis(GameAction action) {
        switch (action) {
            case GameAction.MOVE_HORIZONTAL:
                return Input.GetAxis("Horizontal");
            case GameAction.JUMP:
                return Input.GetAxis("Vertical");
            case GameAction.DUCK:
                return Input.GetAxis("Vertical");
            default:
                return 0;
        }
    }
 
    //FOR THIS I HAVE ADDED A NEW INTERFACE (IINTERACTABLE) TO MAKE SURE EVERYTHING THAT HAS TO BE CLICKED CAN BE CLICKED, RAYCAST IN 3D AND CALL THE ONCLICK FUNC. SAME CAN BE DONE IN MOBILE
    public override void CheckInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Clicked");
            //determine location of camera and clicked position (assumes the blocks are at z = 0)
            Vector3 MouseLocation = Input.mousePosition;
            Vector3 sourcePos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
            MouseLocation.z = -sourcePos.z;
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(MouseLocation);

            //determine direction of raycast
            Vector3 direction = targetPos - sourcePos;

            //make the actual raycast and debug one in the scene view
            Debug.DrawRay(sourcePos, direction, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(sourcePos, direction, out hit, 250f))
            {
                if (hit.transform.GetComponent<IInteractable>() != null)
                {
                    //This is were the magic happens
                    hit.transform.GetComponent<IInteractable>().OnClick();
                }
            }
        }
    }
}
