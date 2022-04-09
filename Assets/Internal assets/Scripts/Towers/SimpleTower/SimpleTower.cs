using Internal_assets.Scripts.Architecture.ObjectPool;
using Internal_assets.Scripts.Architecture.ObjectPool.ProjectilePools;
using Internal_assets.Scripts.Enemies.Entities;
using Internal_assets.Scripts.Projectiles.GuidedProjectile;
using UnityEngine;

namespace Internal_assets.Scripts.Towers.SimpleTower
{
	public class SimpleTower : MonoBehaviour {
		public float shootInterval = 0.5f;
		public float range = 4f;
		private ObjectPool<GuidedProjectile> _pool;

		private float _lastShotTime = 0.0f;
	
		private void Start()
		{
			_pool = gameObject.GetComponent<GuidedProjectilesPool>().Pool;
		}
	
		void Update () {
			_lastShotTime += Time.deltaTime;
			foreach (var monster in FindObjectsOfType<Monster>()) {
				if (Vector3.Distance (transform.position, monster.transform.position) > range)
					continue;

				if (_lastShotTime > shootInterval)
				{
					var projectile = _pool.GetFreeElement();
					var projectileTransform = projectile.transform;
					projectileTransform.position = transform.position + Vector3.up * 1.5f;
					projectileTransform.rotation = Quaternion.identity;
					projectile.GetComponent<GuidedProjectile>().target = monster.gameObject;

					_lastShotTime = 0.0f;
				}
			}
	
		}
	}
}
