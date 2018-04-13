using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMobile : InputSystem
{
    public static InputMobile instance;

    // Input axes
    private float horizontalInput;
    private float verticalInput;

    private Vector2 touchOrigin = -Vector2.one;
    private float dragDistance;

    private Vector2 firstTouchPosition1;
    private Vector2 lastTouchPosition1;
    private Vector2 firstTouchPosition2;
    private Vector2 lastTouchPosition2;

    private float screenCenterX;
    private float screenTopY;

    public InputMobile()
    {
        instance = this;

        screenCenterX = Screen.width * 0.4f;
        dragDistance = Screen.height * 5 / 100; //dragDistance is 15% height of the screen
    }

    public override float GetAxis(GameAction action)
    {
        switch (action)
        {
            case GameAction.MOVE_HORIZONTAL:
                return horizontalInput;
            case GameAction.JUMP:
                return verticalInput;
            case GameAction.DUCK:
                return verticalInput;
            default:
                return 0;
        }
    }

    public void GetTouch()
    {
        if (Input.touchCount > 0)
        {
            // get the first touch
            Touch firstTouch = Input.GetTouch(0);
            DetectTouch1(firstTouch);
        }
        if (Input.touchCount > 1)
        {
            // get the second touch
            Touch secondTouch = Input.GetTouch(1);
            DetectTouch2(secondTouch);
        }
    }

    private void DetectTouch1(Touch touch)
    {
        if (touch.phase == TouchPhase.Began)
        {
            firstTouchPosition1 = touch.position;
            lastTouchPosition1 = touch.position;
        }
        else if (touch.phase == TouchPhase.Moved)
        {
            lastTouchPosition1 = touch.position;

            if (touch.position.x > screenCenterX)
            {
                //Check if drag distance is greater than 20% of the screen height
                if (Mathf.Abs(lastTouchPosition1.x - firstTouchPosition1.x) > dragDistance || Mathf.Abs(lastTouchPosition1.y - firstTouchPosition1.y) > dragDistance)
                {
                    if (Mathf.Abs(lastTouchPosition1.x - firstTouchPosition1.x) > Mathf.Abs(lastTouchPosition1.y - firstTouchPosition1.y))
                    {   //the horizontal movement is greater than the vertical movement
                        if (lastTouchPosition1.x > firstTouchPosition1.x)  //If the movement was to the right
                        {
                            horizontalInput = 1;
                        }
                        if (lastTouchPosition1.x < firstTouchPosition1.x) // If the movement was to the left
                        {
                            horizontalInput = -1;
                        }
                    }
                }
            }
            else if (touch.position.x < screenCenterX)
            {
                //Check if drag distance is greater than 15% of the screen height
                if (Mathf.Abs(lastTouchPosition1.x - firstTouchPosition1.x) > dragDistance || Mathf.Abs(lastTouchPosition1.y - firstTouchPosition1.y) > dragDistance)
                {
                    if (Mathf.Abs(lastTouchPosition1.x - firstTouchPosition1.x) < Mathf.Abs(lastTouchPosition1.y - firstTouchPosition1.y))
                    {   //the vertical movement is greater than the horizontal movement
                        if (lastTouchPosition1.y > firstTouchPosition1.y)  //If the movement was upwards
                        {
                            verticalInput = 1;
                        }
                        if (lastTouchPosition1.y < firstTouchPosition1.y) // If the movement was downwards
                        {
                            verticalInput = -1;
                        }
                    }
                }
            }
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            verticalInput = 0;
            horizontalInput = 0;
        }
    }

    private void DetectTouch2(Touch touch)
    {
        if (touch.phase == TouchPhase.Began)
        {
            firstTouchPosition2 = touch.position;
            lastTouchPosition2 = touch.position;
        }
        else if (touch.phase == TouchPhase.Moved)
        {
            lastTouchPosition2 = touch.position;

            if (touch.position.x > screenCenterX)
            {
                //Check if drag distance is greater than 20% of the screen height
                if (Mathf.Abs(lastTouchPosition2.x - firstTouchPosition2.x) > dragDistance || Mathf.Abs(lastTouchPosition2.y - firstTouchPosition2.y) > dragDistance)
                {
                    if (Mathf.Abs(lastTouchPosition2.x - firstTouchPosition2.x) > Mathf.Abs(lastTouchPosition2.y - firstTouchPosition2.y))
                    {   //the horizontal movement is greater than the vertical movement
                        if (lastTouchPosition2.x > firstTouchPosition2.x)  //If the movement was to the right
                        {
                            horizontalInput = 1;
                        }
                        if (lastTouchPosition2.x < firstTouchPosition2.x) // If the movement was to the left
                        {
                            horizontalInput = -1;
                        }
                    }
                }
            }
            else if (touch.position.x < screenCenterX)
            {
                //Check if drag distance is greater than 15% of the screen height
                if (Mathf.Abs(lastTouchPosition2.x - firstTouchPosition2.x) > dragDistance || Mathf.Abs(lastTouchPosition2.y - firstTouchPosition2.y) > dragDistance)
                {
                    if (Mathf.Abs(lastTouchPosition2.x - firstTouchPosition2.x) < Mathf.Abs(lastTouchPosition2.y - firstTouchPosition2.y))
                    {   //the vertical movement is greater than the horizontal movement
                        if (lastTouchPosition2.y > firstTouchPosition2.y)  //If the movement was upwards
                        {
                            verticalInput = 1;
                        }
                        if (lastTouchPosition2.y < firstTouchPosition2.y) // If the movement was downwards
                        {
                            verticalInput = -1;
                        }
                    }
                }
            }
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            verticalInput = 0;
            horizontalInput = 0;
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