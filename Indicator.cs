using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour
{
    public Transform target, cam;
    public RectTransform backraund_Image, returnRect_Image;
    public GameObject all_Image;
    public bool dontHide;
    public Camera mainCamera;
    public Vector3 mapCameraControlOffset;

    void Update()
    {
        ReturnImge();
    }
    private void ReturnImge(){
        // SetActive
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(target.position);
        bool isVisible = screenPoint.x > 0.1 && screenPoint.x < 0.9 && screenPoint.y > 0.1 && screenPoint.y < 0.9;
        if(isVisible)
        {
            //return_Image.SetActive(false);
            all_Image.SetActive(dontHide);
            
            backraund_Image.position = new Vector3
            (screenPoint.x * Screen.width, screenPoint.y * Screen.height, 0f);
        }else{
            //return_Image.SetActive(true);
            all_Image.SetActive(true);

            // поворот
            //Vector3 Return_Imge = new Vector3(cam.position.x, cam.position.y - 14f,cam.position.z + 9f); 
            Vector3 Return_Imge = cam.position - mapCameraControlOffset; 
            Vector3 directionToTarget = target.position - Return_Imge;

            float angleToTarget = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg - 45;// тут int

            returnRect_Image.rotation = Quaternion.Euler(0f, 0f, -angleToTarget);

            // переміщення якщо за екраном

            int x,y;
            float distance = 0.9f;
            float NormAngleToTarget = angleToTarget + 45;  // тут int

            if((NormAngleToTarget >= -45 && NormAngleToTarget <= 45) || NormAngleToTarget < -135 || NormAngleToTarget > 135){
                
                float PosSizeRight = Mathf.Sin(NormAngleToTarget * Mathf.Deg2Rad) + 1;
                x = (int)(PosSizeRight * Screen.width / 2);

                if(NormAngleToTarget <= 90 && NormAngleToTarget > -90){
                    y = (int)(Screen.height * distance);
                }else{
                    y = (int)(Screen.height * (1 - distance));
                }
            }
            else{
                float PosSizeRight = Mathf.Cos(NormAngleToTarget * Mathf.Deg2Rad) + 1;
                y = (int)(PosSizeRight * Screen.height / 2);

                if(NormAngleToTarget <= 0){
                    x = (int)(Screen.width * (1 - distance));
                }else{
                    x = (int)(Screen.width * distance);
                }
            }
            backraund_Image.position = new Vector3(x, y, 0f);
        }

    }
}
