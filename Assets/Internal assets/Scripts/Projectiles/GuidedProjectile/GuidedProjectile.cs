using Internal_assets.Scripts.Enemies.Entities;
using UnityEngine;

namespace Internal_assets.Scripts.Projectiles.GuidedProjectile
{
	public class GuidedProjectile : MonoBehaviour {
		public GameObject target;
		public float speed = 0.2f;
		public int damage = 10;

		void FixedUpdate () {
			if (target == null) {
				gameObject.SetActive(false);
				return;
			}
			if (!target.GetComponent<Monster>().isActiveAndEnabled)
			{
				gameObject.SetActive(false);
			}

			var translation = target.transform.position - transform.position;
			if (translation.magnitude > speed) {
				translation = translation.normalized * speed;
			}
			transform.Translate (translation);
		}

		void OnTriggerEnter(Collider other) {
			var monster = other.gameObject.GetComponent<Monster> ();
			if (monster == null)
				return;

			monster.currentHp -= damage;
			gameObject.SetActive(false);
		}
	}
}
