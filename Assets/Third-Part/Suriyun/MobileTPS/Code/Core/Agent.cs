using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

using System.Collections;
using System;


namespace Suriyun.MobileTPS
{
    public class Agent : MonoBehaviour
    {

        public GameCamera game_camera;
        public BehaviourControl behaviour;

        [HideInInspector]
        public Transform trans;

        public float hp = 15;
        public bool is_alive = true;

        public GameObject fx_on_hit;

        public UnityEvent ActorDiedEvent;

        void Awake()
        {
            trans = transform;
            behaviour.Init(this);
        }

        public void Hit(float damage)
        {
            hp -= damage;
            Instantiate(fx_on_hit, trans.position, fx_on_hit.transform.rotation);
        }
    }

    [Serializable]
    public class BehaviourControl
    {

        [HideInInspector]
        public Button btn_fire;
        [HideInInspector]
        public Button btn_hide;
        [HideInInspector]
        public Button btn_right;
        [HideInInspector]
        public Button btn_left;

        Agent parent;

        [HideInInspector]
        public Transform destination;
        [HideInInspector]
        public int destination_index;
        [HideInInspector]
        public int current_index;
        [HideInInspector]
        public bool firing;
        [HideInInspector]
        public bool hiding;
        [HideInInspector]
        public float no_firing_delay;
        [HideInInspector]
        public NavMeshAgent agent;
        [HideInInspector]
        public Animator animator;

        public Transform gun_tip;
        public GameObject bullet_prefab;

        Coroutine c_firing;

        public void Init(Agent parent)
        {
            this.parent = parent;
            destination = GameObject.Find("+destination").transform;
            animator = parent.GetComponent<Animator>();
            agent = parent.GetComponent<NavMeshAgent>();

            if (btn_fire == null)
                btn_fire = GameObject.Find("+button.fire").GetComponent<Button>();
            if (btn_hide == null)
                btn_hide = GameObject.Find("+button.hide").GetComponent<Button>();
            if (btn_right == null)
                btn_right = GameObject.Find("+button.right").GetComponent<Button>();
            if (btn_left == null)
                btn_left = GameObject.Find("+button.left").GetComponent<Button>();

            parent.StartCoroutine(PseudoUpdate());
        }

        IEnumerator Die()
        {
            yield return new WaitForSecondsRealtime(2f);
        }

        IEnumerator PseudoUpdate()
        {
            while (true)
            {
                Update();
                if (parent.hp <= 0)
                {
                    parent.is_alive = false;
                    Game.instance.EventGameOver.Invoke();
                }
                yield return 0;
            }
        }

        public void StartFiring()
        {
            parent.game_camera.zoomed = true;

            if (parent.is_alive)
            {
                hiding = false;
                firing = true;
                if (c_firing != null)
                {
                    parent.StopCoroutine(c_firing);
                }
                c_firing = parent.StartCoroutine(FiringMechanism());
            }
        }

        public void StopFiring()
        {
            firing = false;
            parent.game_camera.zoomed = false;
        }

        IEnumerator FiringMechanism()
        {
            float gun_delay = 0.1f;
            yield return new WaitForSeconds(0.16f);
            while (firing)
            {
                UnityEngine.Object.Instantiate(bullet_prefab, gun_tip.position, gun_tip.rotation);
                yield return new WaitForSeconds(gun_delay);
            }
        }

        public void Hide()
        {
            if (parent.is_alive)
            {
                hiding = true;
                firing = false;
                no_firing_delay = 0.16f;
                agent.Stop();
            }
        }

        public void GoLeft()
        {
            if (parent.is_alive)
            {
                hiding = false;
                agent.Resume();
                if (current_index < destination_index)
                {
                    current_index = destination_index;
                }
                destination_index = current_index - 1;
            }
        }

        public void GoRight()
        {
            if (parent.is_alive)
            {
                hiding = false;
                agent.Resume();
                if (current_index > destination_index)
                {
                    current_index = destination_index;
                }
                destination_index = current_index + 1;
            }
        }

        public void Update()
        {

            UpdateCurrentPosition();

            #region :: Input Handler ::
            if (btn_left.pressed || Input.GetKey(KeyCode.A))
            {
                GoLeft();
            }

            if (btn_right.pressed || Input.GetKey(KeyCode.D))
            {
                GoRight();
            }

            if (btn_hide.pressed || Input.GetKey(KeyCode.S))
            {
                Hide();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartFiring();
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                StopFiring();
            }
            #endregion

            animator.SetBool("hiding", hiding);
            animator.SetBool("firing", firing);
            animator.SetFloat("speed", agent.velocity.magnitude);

            destination_index = Mathf.Clamp(destination_index, 0, Game.instance.map_data.red_move_pos.Count - 1);
            destination.position = Game.instance.map_data.red_move_pos[destination_index].position;
            agent.destination = destination.position;

            animator.SetFloat("hp", parent.hp);
            if (!parent.is_alive)
            {
                parent.StopAllCoroutines();
            }
        }

        private void UpdateCurrentPosition()
        {
            for (int i = 0; i < Game.instance.map_data.red_move_pos.Count; i++)
            {
                if (Vector3.Distance(parent.trans.position, Game.instance.map_data.red_move_pos[i].position) < 1f)
                {
                    current_index = i;
                }
            }
        }
    }

}