using Internal_assets.Scripts.Architecture.ObjectPool;
using Internal_assets.Scripts.Architecture.ObjectPool.ProjectilePools;
using UnityEngine;

namespace Internal_assets.Scripts.Towers.SimpleTower
{
	public class SimpleTower : MonoBehaviour {
		public float m_shootInterval = 0.5f;
		public float m_range = 4f;
		public GameObject m_projectilePrefab;
		private ObjectPool<GuidedProjectile> _pool;

		private float m_lastShotTime = -0.5f;
	
		private void Start()
		{
			_pool = gameObject.GetComponent<GuidedProjectilesPool>().Pool;
		}
	
		void Update () {
			if (m_projectilePrefab == null)
				return;

			foreach (var monster in FindObjectsOfType<Monster>()) {
				if (Vector3.Distance (transform.position, monster.transform.position) > m_range)
					continue;

				if (m_lastShotTime + m_shootInterval > Time.time)
					continue;

				// shot
				var projectile = _pool.GetFreeElement();
				var projectileTransform = projectile.transform;
				projectileTransform.position = transform.position + Vector3.up * 1.5f;
				projectileTransform.rotation = Quaternion.identity;
				projectile.GetComponent<GuidedProjectile>().m_target = monster.gameObject;

				m_lastShotTime = Time.time;
			}
	
		}
	}
}
