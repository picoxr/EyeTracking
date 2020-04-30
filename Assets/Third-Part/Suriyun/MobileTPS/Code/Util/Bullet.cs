using UnityEngine;
using System.Collections;

namespace Suriyun.MobileTPS
{
	public class Bullet : MonoBehaviour
	{
        public GameObject sfx_shoot;
		public GameObject fx_on_hit;
		public float damage = 6.66f;
		[Range (0.0f, 100.0f)]
		public float accuraycy = 99f;

		public float speed = 128;
		public float lifetime = 3f;

		Rigidbody rig;
		float dev = 0.016f;

		void Start ()
		{
			// Calculate accuracy //
			Vector3 rand = new Vector3 (
				               Random.Range (-dev, dev),
				               Random.Range (-dev, dev), 
				               0);
			rand = Vector3.zero;
			float offset = 100f - accuraycy;
			rand += transform.right * Random.Range (-offset, offset);
			rand += transform.up * Random.Range (-offset, offset);
			rand = rand.normalized * dev;

			// Fire bullet //
			rig = GetComponent<Rigidbody> ();
			rig.AddForce ((transform.forward ) * speed, ForceMode.VelocityChange);

			// Set Bullet lifetime //
			StartCoroutine (Expire (lifetime));

			// Set pointlight duration a bullet is fired //
			StartCoroutine (ExpireLight ());

            // Play sfx
            Instantiate(sfx_shoot, transform.position, Quaternion.identity);
        }

		IEnumerator Expire (float t)
		{
			yield return new WaitForSeconds (t);
			Destroy (gameObject);
		}

		IEnumerator ExpireLight ()
		{
            yield return 0;//new WaitForSeconds (0.08f);
			Destroy (GetComponent<Light> ());
		}

		void OnCollisionEnter (Collision col)
		{
			rig.useGravity = true;
			if (col.gameObject.tag == "Enemy") {
				Instantiate (fx_on_hit, col.contacts [0].point, fx_on_hit.transform.rotation);
				Enemy e = col.gameObject.GetComponent<Enemy> ();
				e.hp -= damage;
				Destroy (this.gameObject);
			}
		}
	}
}