using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ETMovingLighting : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        //set the delaytime to getEyeTracking;
        StartCoroutine(Lighting(0.05f));
    }

    Pvr_UnitySDKAPI.EyeTrackingGazeRay gazeRay;
    IEnumerator Lighting(float steptime)
    {
        yield return new WaitForSeconds(0.5f);
        while (true)
        {
            bool result = Pvr_UnitySDKAPI.System.UPvr_getEyeTrackingGazeRay(ref gazeRay);
            if (result)
            {
                GetComponent<Light>().enabled = true;
                Ray ray = new Ray(gazeRay.Origin, gazeRay.Direction);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 20))
                {
                    transform.DOMove(hit.point, steptime).SetEase(Ease.Linear);
                    //transform.position = hit.point;
                }
                else GetComponent<Light>().enabled = false;
            }
            else
            {
                GetComponent<Light>().enabled = false;
            }
            yield return new WaitForSeconds(steptime);

        }
    }

}
