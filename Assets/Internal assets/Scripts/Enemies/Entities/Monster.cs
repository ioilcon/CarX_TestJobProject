using UnityEngine;

namespace Internal_assets.Scripts.Enemies.Entities
{
	public class Monster : MonoBehaviour {

		public GameObject moveTarget;
		public float moveSpeed = 0.1f;
		public int maxHp = 30;
		
		const float ReachDistance = 0.3f;
		public int currentHp;

		void Start() {
			currentHp = maxHp;
		}

		void FixedUpdate () {
			if (moveTarget == null)
				return;

			if (currentHp <= 0) {
				gameObject.SetActive(false);
			}
			var translation = moveTarget.transform.position - transform.position;
			if (translation.magnitude > moveSpeed) {
				translation = translation.normalized * moveSpeed;
			}
			transform.Translate (translation);
			
			if (Vector3.Distance (transform.position, moveTarget.transform.position) <= ReachDistance)
			{
				gameObject.SetActive(false);
			}
		}
	}
}
