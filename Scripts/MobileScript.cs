using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileScript : MonoBehaviour
{
    private const float DEADZONE = 100.0f; // 100 pixel
    public static MobileScript instance { set; get; }

    private bool tap, swipeLeft, swipeRight, swipeUp, swipeDown;
    private Vector2 swipeDelta, startTocuh;

    public bool Tap { get { return tap; } }
    public Vector2 SwipeDelta { get { return swipeDelta; } }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }

    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        //resetting all the booleans
        tap = swipeLeft = swipeRight = swipeUp = swipeDown = false;

        #region Standalone Inputs
        if (Input.GetMouseButtonDown(0))
        {
            tap = true;
            startTocuh = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            startTocuh = swipeDelta = Vector2.zero;
        }
        #endregion


        #region Mobile Inputs
        if (Input.touches.Length != 0)
        {
            if(Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                startTocuh = Input.mousePosition;
            }

            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                startTocuh = swipeDelta = Vector2.zero;
            }
        }

        #endregion

        //calculate distance
        swipeDelta = Vector2.zero;
        if(startTocuh != Vector2.zero)
        {
            //lets check for mobile
            if(Input.touches.Length != 0)
            {
                swipeDelta = Input.touches[0].position - startTocuh;
            }

            //lets check for standalone
            if (Input.GetMouseButton(0))
            {
                swipeDelta = (Vector2)Input.mousePosition - startTocuh;
            }
        }

        //lets check if we are beyond the deadzone
        if(swipeDelta.magnitude > DEADZONE)
        {
            float x = swipeDelta.x;   
            float y = swipeDelta.y;   

            if(Mathf.Abs(x) > Mathf.Abs(y))
            {
                if(x < 0)
                {
                    swipeLeft = true;
                }

                else 
                {
                    swipeRight = true;
                }
            }
            else
            {
                if (y < 0)
                {
                    swipeDown = true;
                }

                else
                {
                    swipeUp = true;
                }
            }

            startTocuh = swipeDelta = Vector2.zero;
        }
    }
}
