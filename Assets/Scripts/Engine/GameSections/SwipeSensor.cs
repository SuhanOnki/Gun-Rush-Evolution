using UnityEngine;

namespace Engine.GameSections
{
    public class SwipeSensor : MonoBehaviour
    {
        public static event OnSwipeInput SwipeEvent;
        public static SwipeSensor swipeSensor;

        public delegate void OnSwipeInput(float x);

        public float swipeDetectionValue;

        private float tapPositionX;
        private float swipeDeltaX;

        public bool isSwiping;
        private bool isMobile;

        private void Awake()
        {
            SwipeEvent = null;
            swipeSensor = this;
        }

        private void Start()
        {
            isMobile = Application.isMobilePlatform;
        }

        public void SpecialFunc()
        {
            
        }
        private void Update()
        {
            if (!isMobile)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    isSwiping = true;
                    tapPositionX = Input.mousePosition.x;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    ResetSwipe();
                }
            }
            else
            {
                if (Input.touchCount > 0)
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        isSwiping = true;
                        tapPositionX = Input.GetTouch(0).position.x;
                    }
                    else if (Input.GetTouch(0).phase == TouchPhase.Canceled ||
                             Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        ResetSwipe();
                    }
                }
            }

            CheckSwipe();
        }

        private void CheckSwipe()
        {
            swipeDeltaX = 0;

            if (isSwiping)
            {
                if (!isMobile && Input.GetMouseButton(0))
                    swipeDeltaX = Input.mousePosition.x - tapPositionX;
                else if (Input.touchCount > 0)
                    swipeDeltaX = Input.GetTouch(0).position.x - tapPositionX;
            }

            if (isSwiping)
            {
                if (SwipeEvent != null)
                {
                    swipeDeltaX %= 150;
                    SwipeEvent(swipeDeltaX * swipeDetectionValue * Time.deltaTime);
                }

                if (!isMobile && Input.GetMouseButton(0))
                    tapPositionX = Input.mousePosition.x;
                else if (Input.touchCount > 0)
                    tapPositionX = Input.GetTouch(0).position.x;
            }
        }

        private void ResetSwipe()
        {
            isSwiping = false;

            tapPositionX = 0;
            swipeDeltaX = 0;
        }
    }
}