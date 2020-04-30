using Pvr_UnitySDKAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : DSingleton<Main>
{
    private Dictionary<int, string> avatar_id_dic = new Dictionary<int, string>();
    //场景ID
    private int sceneIndex = 0;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        avatar_id_dic.Add(1, "Avatar_boy");
        avatar_id_dic.Add(2, "Avatar_jackfruit");
        avatar_id_dic.Add(3, "Avatar_watermelon");
    }

    float delaytime;
    bool shouldChange;
    void Update()
    {
        //长按APP键/返回键，进入眼球校准
        if (sceneIndex <= 1)
        {
            if (Input.GetKey(KeyCode.Escape) || Pvr_UnitySDKAPI.Controller.UPvr_GetKey(0, Pvr_UnitySDKAPI.Pvr_KeyCode.APP) || Pvr_UnitySDKAPI.Controller.UPvr_GetKey(1, Pvr_UnitySDKAPI.Pvr_KeyCode.APP))
            {
                delaytime += Time.deltaTime;
                if (delaytime >= 2)
                {
                    delaytime = 0;
                    GoCalibration();
                }
            }
        }

        //返回menu场景按键逻辑设定
        if ((Input.GetKeyUp(KeyCode.Escape) || Pvr_UnitySDKAPI.Controller.UPvr_GetKeyUp(0, Pvr_UnitySDKAPI.Pvr_KeyCode.APP) || Pvr_UnitySDKAPI.Controller.UPvr_GetKeyUp(1, Pvr_UnitySDKAPI.Pvr_KeyCode.APP)) && SceneManager.GetSceneByBuildIndex(0) != SceneManager.GetActiveScene())
        {
            delaytime = 0;
            GoMenu();
        }

        //Game Demo下按确定键切换体验场景
        if (sceneIndex >= 2 && sceneIndex <= 4)
        {
            if (!shouldChange)
            {
                delaytime += Time.deltaTime;
                if (delaytime >= 1)
                {
                    shouldChange = true;
                    delaytime = 0;
                }
            }
            if (shouldChange && (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.JoystickButton0) || Pvr_UnitySDKAPI.Controller.UPvr_GetKeyUp(0, Pvr_UnitySDKAPI.Pvr_KeyCode.TRIGGER) || Pvr_UnitySDKAPI.Controller.UPvr_GetKeyUp(1, Pvr_UnitySDKAPI.Pvr_KeyCode.TRIGGER)))
            {
                switch (sceneIndex)
                {
                    case 2:
                        GoET_lighting();
                        break;
                    case 3:
                        GoET_shooting();
                        break;
                    case 4:
                        GoET_moving();
                        break;
                }

            }
        }

        //Mirror下的逻辑
        if (sceneIndex == 5)
        {
            if (!shouldChange)
            {
                delaytime += Time.deltaTime;
                if (delaytime >= 1)
                {
                    shouldChange = true;
                    delaytime = 0;
                }
            }

            if (shouldChange && (Input.GetKeyUp(KeyCode.JoystickButton0) || Pvr_UnitySDKAPI.Controller.UPvr_GetKeyUp(0, Pvr_UnitySDKAPI.Pvr_KeyCode.TRIGGER) || Pvr_UnitySDKAPI.Controller.UPvr_GetKeyUp(1, Pvr_UnitySDKAPI.Pvr_KeyCode.TRIGGER)))
            {
                GameObject _smoketx = Instantiate(Resources.Load("Bang"), transform.Find("Head")) as GameObject;
                changeAvatar();
            }
        }

        //手柄显示/隐藏的逻辑
        if (SceneManager.GetSceneByBuildIndex(0) != SceneManager.GetActiveScene())
        {
            transform.Find("ControllerManager/PvrController0").localScale = Vector3.zero;
            transform.Find("ControllerManager/PvrController1").localScale = Vector3.zero;
        }
        else
        {
            transform.Find("ControllerManager/PvrController0").localScale = Vector3.one;
            transform.Find("ControllerManager/PvrController1").localScale = Vector3.one;
        }

        //打印位置
        //Debug.Log("SX---PicoEyeTrackingData---Head Position---(" + transform.Find("Head").position.x + ","+ transform.Find("Head").position.y + ","+ transform.Find("Head").position.z+")");
    }

    public void GoET_shooting()
    {
        sceneIndex = 4;
        SceneManager.LoadScene(sceneIndex);
    }

    public void GoET_lighting()
    {
        sceneIndex = 3;
        SceneManager.LoadScene(sceneIndex);
    }

    public void GoET_moving()
    {
        sceneIndex = 2;
        transform.Find("Head/LeftEye").GetComponent<Camera>().backgroundColor = new Color(206f / 255f, 160f / 255f, 118f / 255f, 5f / 255f);
        transform.Find("Head/RightEye").GetComponent<Camera>().backgroundColor = new Color(206f / 255f, 160f / 255f, 118f / 255f, 5f / 255f);
        SceneManager.LoadScene(sceneIndex);
    }

    public void GoCoordinates()
    {
        sceneIndex = 1;
        GameObject _zbx = Instantiate(Resources.Load("Quad"), transform.Find("Head/Canvas")) as GameObject;
        _zbx.name = "Quad";
        SceneManager.LoadScene(sceneIndex);
    }

    public void GoCalibration()
    {
        openPackage("com.tobii.usercalibration.pico");
    }

    public void GoMirror()
    {
        sceneIndex = 5;
        _nowAvatarId = 0;
        delaytime = 0;
        shouldChange = false;
        changeAvatar();
        SceneManager.LoadScene(sceneIndex);
    }

    public void GoMenu()
    {
        sceneIndex = 0;
        delaytime = 0;
        shouldChange = false;
        if (transform.Find("Head/Canvas/Quad")) Destroy(transform.Find("Head/Canvas/Quad").gameObject);
        if (transform.Find("Head/Avatar")) Destroy(transform.Find("Head/Avatar").gameObject);
        SceneManager.LoadScene(sceneIndex);
        transform.Find("Head/LeftEye").GetComponent<Camera>().backgroundColor = new Color(0, 0, 0, 5f / 255f);
        transform.Find("Head/RightEye").GetComponent<Camera>().backgroundColor = new Color(0, 0, 0, 5f / 255f);
    }

    public void openPackage(string pkgName)
    {
        using (AndroidJavaClass jcPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject joActivity = jcPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (AndroidJavaObject joPackageManager = joActivity.Call<AndroidJavaObject>("getPackageManager"))
                {
                    using (AndroidJavaObject joIntent = joPackageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", pkgName))
                    {
                        if (null != joIntent)
                        {
                            joActivity.Call("startActivity", joIntent);
                        }
                    }
                }
            }
        }
    }


    int _nowAvatarId = 0;
    void changeAvatar()
    {
        if (transform.Find("Head/Avatar") != null) Destroy(transform.Find("Head/Avatar").gameObject);
        _nowAvatarId++;
        if (_nowAvatarId > 3) _nowAvatarId = 1;
        GameObject _avatar = Instantiate(Resources.Load(avatar_id_dic[_nowAvatarId]), transform.Find("Head")) as GameObject;
        _avatar.name = "Avatar";
    }

}
