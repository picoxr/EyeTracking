using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ETAvatar : MonoBehaviour {
    public int Avatar_ID = 0;
    public float eye_moving_scale = 0;

    public Transform left_eye;
    public Transform right_eye;

    private float open_eye_scale = 0.06f;
    private float close_eye_scale = 0.01f;

    private Vector3 left_eye_positon;
    private Vector3 right_eye_positon;

    private bool last_lefteye_opened = true;
    private bool last_righteye_opened = true;

    // Use this for initialization
    void Start () {
        left_eye_positon = left_eye.localPosition;
        right_eye_positon = right_eye.localPosition;
        //StartCoroutine(EyeOpenness(0.03f));
        StartCoroutine(EyeMoving(0.06f));
    }

    Pvr_UnitySDKAPI.EyeTrackingData trackingData;

    void Update()
    {
        EyeOpenness();
    }

    void EyeOpenness() {
        bool result = Pvr_UnitySDKAPI.System.UPvr_getEyeTrackingData(ref trackingData);
        if (result)
        {
            //睁闭眼
            switch (Avatar_ID)
            {
                case 1:
                    if (!last_lefteye_opened && (int)trackingData.leftEyeOpenness == 1)
                    {
                        left_eye.DOScaleY(open_eye_scale, 0);
                        last_lefteye_opened = true;
                    }
                    else if (last_lefteye_opened && (int)trackingData.leftEyeOpenness == 0)
                    {
                        left_eye.DOScaleY(close_eye_scale, 0);
                        last_lefteye_opened = false;
                    }

                    if (!last_righteye_opened && (int)trackingData.rightEyeOpenness == 1)
                    {
                        right_eye.DOScaleY(open_eye_scale, 0);
                        last_righteye_opened = true;
                    }
                    else if (last_righteye_opened && (int)trackingData.rightEyeOpenness == 0)
                    {
                        right_eye.DOScaleY(close_eye_scale, 0);
                        last_righteye_opened = false;
                    }
                    break;
                case 2:
                case 3:
                    if (!last_lefteye_opened && (int)trackingData.leftEyeOpenness == 1)
                    {
                        left_eye.GetComponent<SpriteRenderer>().enabled = true;
                        left_eye.Find("eyes-left-close").GetComponent<SpriteRenderer>().enabled = false;
                        last_lefteye_opened = true;
                    }
                    else if (last_lefteye_opened && (int)trackingData.leftEyeOpenness == 0)
                    {
                        left_eye.GetComponent<SpriteRenderer>().enabled = false;
                        left_eye.Find("eyes-left-close").GetComponent<SpriteRenderer>().enabled = true;
                        last_lefteye_opened = false;
                    }

                    if (!last_righteye_opened && (int)trackingData.rightEyeOpenness == 1)
                    {
                        right_eye.GetComponent<SpriteRenderer>().enabled = true;
                        right_eye.Find("eyes-right-close").GetComponent<SpriteRenderer>().enabled = false;
                        last_righteye_opened = true;
                    }
                    else if (last_righteye_opened && (int)trackingData.rightEyeOpenness == 0)
                    {
                        right_eye.GetComponent<SpriteRenderer>().enabled = false;
                        right_eye.Find("eyes-right-close").GetComponent<SpriteRenderer>().enabled = true;
                        last_righteye_opened = false;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator EyeMoving(float steptime)
    {
        while (true)
        {
            bool result = Pvr_UnitySDKAPI.System.UPvr_getEyeTrackingData(ref trackingData);
            if (result)
            {
                //眼球移动
                left_eye.DOLocalMove(new Vector3(left_eye_positon.x + trackingData.combinedEyeGazeVector.x * eye_moving_scale, left_eye_positon.y + trackingData.combinedEyeGazeVector.y * eye_moving_scale, left_eye_positon.z), steptime);
                right_eye.DOLocalMove(new Vector3(right_eye_positon.x + trackingData.combinedEyeGazeVector.x * eye_moving_scale, right_eye_positon.y + trackingData.combinedEyeGazeVector.y * eye_moving_scale, right_eye_positon.z), steptime);
            }
            yield return new WaitForSeconds(steptime);

        }
    }
}
