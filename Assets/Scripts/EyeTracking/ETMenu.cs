
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pvr_UnitySDKAPI;
using System;
using DG.Tweening;

public class ETMenu : MonoBehaviour
{
    public GameObject pvr;

    private GameObject HeadSetController;
    private GameObject controller0;
    private GameObject controller1;

    private Transform lastHit;
    private Transform currentHit;

    private GameObject currentController;
    private Ray ray;
    public float rayDefaultLength = 4;
    private bool isHasController = false;

    void Awake()
    {
        //首次进入，创建Pvr_UnitySDK
        if (GameObject.Find("Pvr_UnitySDK") == null)
        {
            GameObject _pvr = Instantiate(pvr);
            _pvr.name = "Pvr_UnitySDK";
            _pvr.transform.parent = transform.parent;
        }

    }

    void Start()
    {
        //初始化变量
        GameObject _pvrsdk = GameObject.Find("Pvr_UnitySDK");
        HeadSetController = _pvrsdk.transform.Find("HeadControl/HeadSetControl").gameObject;
        controller0 = _pvrsdk.transform.Find("ControllerManager/PvrController0").gameObject;
        controller1 = _pvrsdk.transform.Find("ControllerManager/PvrController1").gameObject;

        //头戴和手柄控制的监听设定
        if (Controller.UPvr_GetControllerState(0) == ControllerState.Connected ||
            Controller.UPvr_GetControllerState(1) == ControllerState.Connected)
        {
            HeadSetController.SetActive(false);
            if (Controller.UPvr_GetMainHandNess() == 0)
            {
                currentController = controller0;
            }
            else if (Controller.UPvr_GetMainHandNess() == 1)
            {
                currentController = controller1;
            }
        }
        else HeadSetController.SetActive(true);

        ray = new Ray();
        if (Pvr_UnitySDKManager.SDK.isHasController)
        {
            Pvr_ControllerManager.PvrServiceStartSuccessEvent += ServiceStartSuccess;
            Pvr_ControllerManager.SetControllerStateChangedEvent += ControllerStateListener;
            Pvr_ControllerManager.ControllerStatusChangeEvent += CheckControllerStateForGoblin;
        }

#if UNITY_EDITOR
        currentController = controller0;
#endif
    }

    float delaytime = 0;
    void Update()
    {
        //头戴与手柄的逻辑
        if (HeadSetController.activeSelf)
        {
            HeadSetController.transform.parent.localRotation = Quaternion.Euler(Pvr_UnitySDKSensor.Instance.HeadPose.Orientation.eulerAngles.x, Pvr_UnitySDKSensor.Instance.HeadPose.Orientation.eulerAngles.y, 0);

            ray.direction = HeadSetController.transform.position - HeadSetController.transform.parent.parent.Find("Head").position;
            ray.origin = HeadSetController.transform.parent.parent.Find("Head").position;
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.red);
                if (lastHit == null)
                {
                    hit.transform.DOLocalMoveZ(-30, 0.3f);
                    lastHit = hit.transform;
                }
                else if (lastHit != hit.transform)
                {
                    lastHit.DOLocalMoveZ(0, 0.3f);
                    hit.transform.DOLocalMoveZ(-30, 0.3f);
                    lastHit = hit.transform;
                }
                if (Pvr_ControllerManager.Instance.LengthAdaptiveRay)
                {
                    HeadSetController.transform.position = hit.point;
                    HeadSetController.transform.position -= (hit.point - HeadSetController.transform.parent.parent.Find("Head").position).normalized * 0.02f;
                    float scale = 0.008f * hit.distance / 4f;
                    Mathf.Clamp(scale, 0.002f, 0.008f);
                    HeadSetController.transform.localScale = new Vector3(scale, scale, 1);
                }
            }
            else
            {
                if (lastHit != null)
                {
                    lastHit.DOLocalMoveZ(0, 0.3f);
                    lastHit = null;
                }

                if (Pvr_ControllerManager.Instance.LengthAdaptiveRay)
                {
                    HeadSetController.transform.position = HeadSetController.transform.parent.parent.Find("Head").position + ray.direction.normalized * (0.5f + rayDefaultLength);
                    HeadSetController.transform.localScale = new Vector3(0.008f, 0.008f, 1);
                }
            }

            if (Input.GetKeyUp(KeyCode.JoystickButton0) || Input.GetMouseButtonUp(0))
            {
                if (lastHit != null)
                {
                    lastHit.DOKill();
                    lastHit.localPosition = new Vector3(lastHit.localPosition.x, lastHit.localPosition.y, 0);
                    OnClick(lastHit.gameObject);
                }
            }
        }
        else
        {
            if (currentController != null)
            {
                ray.direction = currentController.transform.Find("dot").position - currentController.transform.Find("start").position;
                ray.origin = currentController.transform.Find("start").position;
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (lastHit == null)
                    {
                        hit.transform.DOLocalMoveZ(-30, 0.3f);
                        lastHit = hit.transform;
                    }
                    else if (lastHit != hit.transform)
                    {
                        lastHit.DOLocalMoveZ(0, 0.3f);
                        hit.transform.DOLocalMoveZ(-30, 0.3f);
                        lastHit = hit.transform;
                    }
                    Debug.DrawLine(ray.origin, hit.point, Color.red);
                    currentController.transform.Find("dot").position = hit.point;
                    if (Pvr_ControllerManager.Instance.LengthAdaptiveRay)
                    {
                        float scale = 0.178f * currentController.transform.Find("dot").localPosition.z / 3.3f;
                        Mathf.Clamp(scale, 0.05f, 0.178f);
                        currentController.transform.Find("dot").localScale = new Vector3(scale, scale, 1);
                    }
                }
                else
                {
                    if (lastHit != null)
                    {
                        lastHit.DOLocalMoveZ(0, 0.3f);
                        lastHit = null;
                    }
                    if (Pvr_ControllerManager.Instance.LengthAdaptiveRay)
                    {
                        currentController.transform.Find("dot").localScale = new Vector3(0.178f, 0.178f, 1);
                        currentController.transform.Find("dot").position = currentController.transform.position + currentController.transform.forward.normalized * (0.5f + rayDefaultLength);
                    }
                }
            }

            if (Controller.UPvr_GetKeyUp(Controller.UPvr_GetMainHandNess(), Pvr_KeyCode.TRIGGER) || Input.GetMouseButtonUp(0))
            {
                if (lastHit != null)
                {
                    lastHit.DOKill();
                    lastHit.localPosition = new Vector3(lastHit.localPosition.x, lastHit.localPosition.y, 0);
                    OnClick(lastHit.gameObject);
                }
            }
        }
    }

    private void OnClick(GameObject obj)
    {
        switch (obj.name)
        {
            case "calibration_bt":
                Main.Instance.GoCalibration();
                break;
            case "coordinates_bt":
                Main.Instance.GoCoordinates();
                break;
            case "ET_moving_bt":
                Main.Instance.GoET_moving();
                break;
            case "ET_lighting_bt":
                Main.Instance.GoET_lighting();
                break;
            case "ET_shooting_bt":
                Main.Instance.GoET_shooting();
                break;
            case "gamedemo_bt":
                Main.Instance.GoET_moving();
                break;
            case "mirror_bt":
                Main.Instance.GoMirror();
                break;
            default:
                break;
        }
    }


    private void ServiceStartSuccess()
    {
        if (Controller.UPvr_GetControllerState(0) == ControllerState.Connected ||
            Controller.UPvr_GetControllerState(1) == ControllerState.Connected)
        {
            HeadSetController.SetActive(false);
        }
        else
        {
            HeadSetController.SetActive(true);
        }
        if (Controller.UPvr_GetMainHandNess() == 0)
        {
            currentController = controller0;
        }
        if (Controller.UPvr_GetMainHandNess() == 1)
        {
            currentController = controller1;
        }
    }

    private void ControllerStateListener(string data)
    {

        if (Controller.UPvr_GetControllerState(0) == ControllerState.Connected ||
            Controller.UPvr_GetControllerState(1) == ControllerState.Connected)
        {
            HeadSetController.SetActive(false);
        }
        else
        {
            HeadSetController.SetActive(true);
        }

        if (Controller.UPvr_GetMainHandNess() == 0)
        {
            currentController = controller0;
        }
        if (Controller.UPvr_GetMainHandNess() == 1)
        {
            currentController = controller1;
        }
    }

    private void CheckControllerStateForGoblin(string state)
    {
        HeadSetController.SetActive(!Convert.ToBoolean(Convert.ToInt16(state)));
    }

    void OnDestroy()
    {
        GameObject.Find("Pvr_UnitySDK").transform.Find("HeadControl/HeadSetControl").gameObject.SetActive(false);
        if (Pvr_UnitySDKManager.SDK.isHasController)
        {
            Pvr_ControllerManager.PvrServiceStartSuccessEvent -= ServiceStartSuccess;
            Pvr_ControllerManager.SetControllerStateChangedEvent -= ControllerStateListener;
            Pvr_ControllerManager.ControllerStatusChangeEvent -= CheckControllerStateForGoblin;
        }
    }
}
