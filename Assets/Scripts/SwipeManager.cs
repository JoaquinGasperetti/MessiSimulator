using UnityEngine;

public class SwipeManager : MonoBehaviour
{
    public static bool swipeLeft;
    public static bool swipeRight;
    public static bool swipeUp;
    public static bool swipeDown;

    private Vector2 startTouchPosition;
    private Vector2 swipeDelta;
    private const float swipeThreshold = 50f; // Distancia mínima para considerar un swipe

    private void Update()
    {
        swipeLeft = swipeRight = swipeUp = swipeDown = false;

        // Para PC
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            swipeDelta = (Vector2)Input.mousePosition - startTouchPosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            DetectSwipe();
            startTouchPosition = swipeDelta = Vector2.zero;
        }

        // Para Android
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                swipeDelta = touch.position - startTouchPosition;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                DetectSwipe();
                startTouchPosition = swipeDelta = Vector2.zero;
            }
        }
    }

    private void DetectSwipe()
    {
        if (swipeDelta.magnitude > swipeThreshold)
        {
            float x = swipeDelta.x;
            float y = swipeDelta.y;

            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                // Swipe horizontal
                if (x < 0)
                    swipeLeft = true;
                else
                    swipeRight = true;
            }
            else
            {
                // Swipe vertical
                if (y < 0)
                    swipeDown = true;
                else
                    swipeUp = true;
            }
        }
    }
}
