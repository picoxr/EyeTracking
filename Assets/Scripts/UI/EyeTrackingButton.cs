using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EyeTrackingButton : MonoBehaviour {

	private Vector3 normalPosition;
	public float targetPositionZ = -50f;
	private float time = 0.15f;
	private Tween hoverTween;
    bool isSelected;

	// Use this for initialization
	void Start () {
        StartCoroutine(SetNormalPos());
	}

    IEnumerator SetNormalPos() {
        yield return new WaitForEndOfFrame();
        normalPosition = this.transform.localPosition;
    }

    Pvr_UnitySDKAPI.EyeTrackingGazeRay gazeRay;
    // Update is called once per frame
    void Update()
    {
        bool result = Pvr_UnitySDKAPI.System.UPvr_getEyeTrackingGazeRay(ref gazeRay);
        if (result)
        {
            Ray ray = new Ray(gazeRay.Origin, gazeRay.Direction);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 20))
            {
                if (hit.transform.name.Equals(transform.name)) OnHover(true);
                else OnHover(false);
            }
            else OnHover(false);

            if (isSelected && (Input.GetKeyDown(KeyCode.JoystickButton0) || Pvr_UnitySDKAPI.Controller.UPvr_GetKeyDown(0, Pvr_UnitySDKAPI.Pvr_KeyCode.TRIGGER) || Pvr_UnitySDKAPI.Controller.UPvr_GetKeyDown(1, Pvr_UnitySDKAPI.Pvr_KeyCode.TRIGGER))) {
                OnClick();
            }
        }
    }

    private void OnHover(bool isHover)
	{
		if (!this.enabled)
			return;
		if (isHover) {
            if (transform.localPosition.z != targetPositionZ) {
                isSelected = true;
                hoverTween.Kill();
                hoverTween = this.transform.DOLocalMoveZ(targetPositionZ, time);
            }
            
		} else {
            if (transform.localPosition.z != normalPosition.z)
            {
                isSelected = false;
                hoverTween.Kill();
                hoverTween = this.transform.DOLocalMoveZ(normalPosition.z, time);
            }
		}
	}

	private void OnClick()
	{
		if (!this.enabled)
			return;
		hoverTween.Kill ();
		this.transform.localPosition = normalPosition;
        switch (gameObject.name)
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

	void OnDestroy()
	{
//		PUIEventListener.Get(this.gameObject).onHover -= OnHover;
//		PUIEventListener.Get(this.gameObject).onClick -= OnClick;
	}

}
