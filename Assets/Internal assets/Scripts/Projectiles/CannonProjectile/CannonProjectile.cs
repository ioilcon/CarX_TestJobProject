using Internal_assets.Scripts.Enemies.Entities;
using UnityEngine;

namespace Internal_assets.Scripts.Projectiles.CannonProjectile
{
	public class CannonProjectile : MonoBehaviour {
		public int damage = 10;

		[SerializeField] public float wickLength = 5.0f;
		public float currentWickLength;

		private void Start()
		{
			currentWickLength = wickLength;
		}

		void Update () {
			currentWickLength -= Time.deltaTime;
			if (currentWickLength < 0)
			{
				currentWickLength = wickLength;
				this.gameObject.SetActive(false);
			}
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
