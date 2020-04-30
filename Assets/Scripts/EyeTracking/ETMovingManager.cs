using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETMovingManager : MonoBehaviour
{
    Transform _selectObj;

    Pvr_UnitySDKAPI.EyeTrackingGazeRay gazeRay;
    void Update()
    {
        bool result = Pvr_UnitySDKAPI.System.UPvr_getEyeTrackingGazeRay(ref gazeRay);
        if (result)
        {
            Ray ray = new Ray(gazeRay.Origin, gazeRay.Direction);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 20))
            {
                if (hit.collider.transform.name.Equals("line")) return;
                if (_selectObj != null && _selectObj != hit.transform)
                {
                    _selectObj.GetComponent<ETMEnity>().StopAnimation();
                    _selectObj = null;
                }
                else if (_selectObj == null)
                {
                    _selectObj = hit.transform;
                    _selectObj.GetComponent<ETMEnity>().PlayAnimation();
                }

            }
            else
            {
                if (_selectObj != null)
                {
                    _selectObj.GetComponent<ETMEnity>().StopAnimation();
                    _selectObj = null;
                }

            }
        }
        else
        {
            if (_selectObj)
            {
                _selectObj.GetComponent<ETMEnity>().StopAnimation();
                _selectObj = null;
            }
        }
    }
}
