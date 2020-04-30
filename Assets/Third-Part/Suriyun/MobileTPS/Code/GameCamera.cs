using UnityEngine;
using System.Collections;
using TouchScript;
using TouchScript.Gestures;

namespace Suriyun.MobileTPS
{
    public class GameCamera : MonoBehaviour
    {
        [HideInInspector]
        public GameObject player;
        public Transform cam_holder;
        public Transform target;
        public Vector3 offset_pos;
        public Vector3 offset_rot;
        public float smoothness = 1.66f;
        private Transform trans;
        public Vector3 target_rotation;

        public Transform aimer;
        public ScreenTransformGesture screen_gesture;
        public float screen_rotation_speed = 0.33f;
        public float screen_rotation_smoothness = 16.66f;

        public bool zoomed = false;
        public float zoomed_speed_multiplier = 0.33f;
        public float zoom_speed = 6f;
        public float fov_zoom = 30;
        public float fov_normal = 60;
        Camera cam;

        void Start()
        {
            trans = transform;
            target_rotation = trans.rotation.eulerAngles;
            aimer.rotation = trans.rotation;
            cam = GetComponent<Camera>();
            player = GameObject.FindObjectOfType<Agent>().gameObject;
        }


        void Update()
        {
            // Zoom control //
            if (zoomed)
            {
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov_zoom, zoom_speed * Time.deltaTime);
            }
            else {
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov_normal, zoom_speed * Time.deltaTime);
            }

            // Aim position //
            aimer.position = trans.position;

            Vector3 pos = player.transform.position + aimer.forward * offset_pos.z + aimer.up * offset_pos.y + aimer.right * offset_pos.x;
            pos.y = Mathf.Clamp(pos.y, player.transform.position.y, pos.y + 1);
            trans.position = Vector3.Slerp(
                trans.position,
                pos,
                smoothness * Time.deltaTime);

            RaycastHit hit;
            if (Physics.Raycast(trans.position, trans.forward, out hit))
            {
                target.transform.position = hit.point;
            }
            else {
                target.transform.position = trans.position + trans.forward * 20;
            }

            // Rotation control //




            /*
            Vector3 rotation = aimer.localEulerAngles;
            Debug.Log(rotation);
            if(rotation.x>180 && rotation.x < 360)
            {
                rotation.x = Mathf.Clamp(rotation.x, 270, 360);
            }
            if (rotation.x > 0 && rotation.x < 90)
            {
                rotation.x = Mathf.Clamp(rotation.x, 0, 90);
            }
            // rotation.x = Mathf.Clamp(rotation.x, -90, 90);
            aimer.localEulerAngles = rotation;*/

            // aimer.rotation = Quaternion.Slerp(aimer.rotation, Quaternion.Euler(Vector3.forward), 90);

            trans.rotation = Quaternion.Slerp(trans.rotation, aimer.rotation, screen_rotation_smoothness * Time.deltaTime);
        }

        private void OnEnable()
        {
            screen_gesture.Transformed += FullScreenHandler;
        }

        private void OnDisable()
        {
            screen_gesture.Transformed -= FullScreenHandler;
        }

        private void FullScreenHandler(object sender, System.EventArgs e)
        {
            // Aim camera //
            Vector3 rotate_horizontal = Vector3.up * screen_gesture.DeltaPosition.x * screen_rotation_speed;
            Vector3 rotate_vertical = aimer.right * (-1f) * screen_gesture.DeltaPosition.y * screen_rotation_speed;

            if (zoomed)
            {
                rotate_horizontal *= zoomed_speed_multiplier;
                rotate_vertical *= zoomed_speed_multiplier;
            }

            Quaternion tmp = aimer.rotation;
            aimer.Rotate( rotate_vertical, Space.World);
            if (aimer.up.y < 0)
            {
                aimer.rotation = tmp;
            }
            aimer.Rotate(rotate_horizontal, Space.World);

        }
    }
}