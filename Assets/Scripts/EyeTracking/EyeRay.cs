using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EyeRay : MonoBehaviour
{
    Transform greenpoint;


    // Use this for initialization
    void Start()
    {
        greenpoint = Main.Instance.transform.Find("Head/Canvas/Quad/zbx/point");

        StartCoroutine(EyeRaycast(0.04f));
    }

    Pvr_UnitySDKAPI.EyeTrackingGazeRay gazeRay;
    float pgBar_num = 0;

    IEnumerator EyeRaycast(float steptime)
    {
        while (true)
        {
            bool result = Pvr_UnitySDKAPI.System.UPvr_getEyeTrackingGazeRay(ref gazeRay);
            if (result)
            {
                Ray ray = new Ray(gazeRay.Origin, gazeRay.Direction);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 20))
                {
                    if (hit.collider.transform.name.Equals("zbx"))
                    {
                        greenpoint.gameObject.SetActive(true);
                        greenpoint.DOMove(hit.point, steptime).SetEase(Ease.Linear);
                    }
                }
                else
                {
                    greenpoint.gameObject.SetActive(false);
                }
            }
            yield return new WaitForSeconds(steptime);
        }
    }

}
