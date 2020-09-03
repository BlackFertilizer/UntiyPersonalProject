using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityTemplateProjects
{
    public class SimpleCameraController : MonoBehaviour
    {
        private Vector2 touchStartPos;
        private Vector2 touchDirection;
        private float startSqtTouchDistance;

            // Start is called before the first frame update
        private Transform gridTran;

        private ScrollCircle scrollCircle;

        private bool isDrag;

        public bool showCanvas = true;

        void Start()
        {
            GameObject canvas =GameObject.Instantiate(Resources.Load<GameObject>("Canvas"));
            if(canvas == null || !showCanvas)
            {
                return;
            }

            gridTran = canvas.GetComponentInChildren<GridLayoutGroup>().transform;
            scrollCircle = canvas.GetComponentInChildren<ScrollCircle>();

            GameObject buttonProto = Resources.Load<GameObject>("Button");
            PoiInfo poiInfo = PoiParseTool.GetPoiObject();
            if(poiInfo != null && poiInfo.poiList != null)
            {
                foreach(var tempPoi in poiInfo.poiList)
                {
                    string strLog = $"poiid = {tempPoi.poiId}, posx ={tempPoi.posX},  posy = {tempPoi.posY},  posz = {tempPoi.posZ}";
                    GameObject button = GameObject.Instantiate(buttonProto);
                    button.GetComponentInChildren<Text>().text = tempPoi.poiId;
                    button.transform.SetParent(gridTran);
                    button.GetComponent<Button>().onClick.AddListener(()=>{this.transform.position = new Vector3(tempPoi.posX, tempPoi.posY, tempPoi.posZ); this.transform.localRotation = Quaternion.identity; Debug.Log("onclick"+this.transform.position);});
                }
            }

            if(scrollCircle != null)
            {
                scrollCircle._rotationAction += changeCameraAngle;
                scrollCircle._dragStateAction += dragState;
            }
        }

        Vector3 GetInputTranslationDirection()
        {
            Vector3 direction = new Vector3();
            #region 

            if (Input.GetKey(KeyCode.W))
            {
                direction -= transform.forward;
            }
            if (Input.GetKey(KeyCode.S))
            {
                direction += transform.forward;
            }
            if (Input.GetKey(KeyCode.A))
            {
                direction += transform.right;
            }
            if (Input.GetKey(KeyCode.D))
            {
                direction -= transform.right;
            }
            if (Input.GetKey(KeyCode.Q))
            {
                direction += transform.up;
            }
            if (Input.GetKey(KeyCode.E))
            {
                direction -= transform.up;
            }
            #endregion


            direction += getDirection();
            return direction;
        }


        private Vector3 getDirection()
        {
            Vector3 moveDirection = Vector3.zero;

            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        touchStartPos = touch.position;
                        break;
                    case TouchPhase.Moved:
                        touchDirection = touch.position - touchStartPos;
                        break;

                    case TouchPhase.Ended:
                        touchDirection = Vector2.zero;
                        break;
                }
                touchDirection /= 200;
                moveDirection = touchDirection.x * transform.right + touchDirection.y * transform.up;//new Vector3(-touchDirection.x, touchDirection.y, 0);
            }

            if (Input.touchCount == 2)
            {
                var touch1 = Input.GetTouch(0);
                var touch2 = Input.GetTouch(1);
                if (startSqtTouchDistance == 0)
                {
                    startSqtTouchDistance = Vector2.Distance(touch1.position, touch2.position);
                }
                else
                {
                    var newSqrTouchDistance = Vector2.Distance(touch1.position, touch2.position);
                    var dir = (touch1.position + touch2.position)/2;
                    Ray ray = Camera.current.ScreenPointToRay(new Vector3(dir.x, dir.y, 0));
                    
                    moveDirection = ray.direction * (startSqtTouchDistance - newSqrTouchDistance)/100;
                }
            }

            if(Input.touchCount == 0)
            {
                startSqtTouchDistance = 0;
            }

            return moveDirection;
        }

        void Update()
        {
            if(!isDrag)
            {
                Vector3 translation = Vector3.zero;
                translation = GetInputTranslationDirection() * Time.deltaTime;
                transform.localPosition -= translation * 20;
            }

        }

        private void changeCameraAngle(Vector3 direction)
        {
            direction /= 20;
            Debug.Log(direction);
            // transform.Rotate(direction.y,-direction.x,0,Space.Self);
            transform.RotateAround(transform.position,transform.up,direction.x);
            // if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            // {
            //     //transform.Rotate(direction.y,0,0,Space.Self);
            //     transform.RotateAround(transform.position,transform.up,direction.x);
            // }
            // else
            // {
            //      transform.RotateAround(transform.position,transform.forward,10);
            // }

           //transform.rotation = 
        }

        private void dragState(bool isDrag)
        {
            this.isDrag = isDrag;
        }

    }

}