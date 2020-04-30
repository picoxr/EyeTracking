using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Suriyun.MobileTPS
{
    public class Game : MonoBehaviour
    {

        public static Game instance;

        public GameObject target;
        public GameObject enemy_prefab;
        public GameObject bullet_prefab;
        public MapData map_data;

        public UnityEvent EventGameStart;
        public UnityEvent EventGameRestart;
        public UnityEvent EventGameOver;

        void Awake()
        {
            instance = this;
            // Perform game setttings here //
            //Application.targetFrameRate = 60;
            StartCoroutine(FiringMechanism());
            GameStart();
        }

        IEnumerator Spawner()
        {
            while (true)
            {
                int rand = UnityEngine.Random.Range(0, map_data.enemy_spawn_point.Count - 1);
                GameObject g = (GameObject)Instantiate(enemy_prefab);
                g.transform.parent = null;
                g.transform.position = map_data.enemy_spawn_point[rand].position;
                yield return new WaitForSecondsRealtime(1f);
            }
        }


        public void GameStart()
        {
            EventGameStart.Invoke();
            StartCoroutine(Spawner());
        }

        public void GameRestart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void ShowGameOverMenu()
        {
            EventGameOver.Invoke();
            Debug.Log("Game Over");
            StopCoroutine(Spawner());
        }

        bool firing = true;
        Pvr_UnitySDKAPI.EyeTrackingGazeRay gazeRay;
        IEnumerator FiringMechanism()
        {
            float gun_delay = 0.1f;
            yield return new WaitForSeconds(0.15f);
            while (firing)
            {
                bool result = Pvr_UnitySDKAPI.System.UPvr_getEyeTrackingGazeRay(ref gazeRay);
                if (result)
                {
                    RaycastHit hit;
                    Ray ray = new Ray(gazeRay.Origin, gazeRay.Direction);
                    if (Physics.Raycast(ray, out hit, 100))
                    {
                        target.transform.position = hit.point;
                    }
                    else
                    {
                        target.transform.position = ray.origin + ray.direction * 20;
                    }
                    if (gazeRay.Direction != new Vector3(0.0f,0.0f,0.0f)) {
                        var bullet = Instantiate(bullet_prefab);
                        bullet.transform.position = new Vector3(ray.origin.x, ray.origin.y-0.03f, ray.origin.z);
                        bullet.transform.LookAt(target.transform);
                    }
                    
                }
                else Debug.Log("PicoEyeTrackingData---GazeRay---Fail!!");
                yield return new WaitForSeconds(gun_delay);
            }
            
        }

    }

    [Serializable]
    public class MapData
    {
        // Destinations data for actor navigation
        // Player is in Red team so it will only move along red_move_pos
        public List<Transform> red_move_pos;
        public List<Transform> blue_move_pos;
        public List<Transform> enemy_spawn_point;
    }
}