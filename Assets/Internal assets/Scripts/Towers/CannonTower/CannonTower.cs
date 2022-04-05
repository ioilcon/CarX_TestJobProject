using Internal_assets.Scripts.Architecture.ObjectPool;
using Internal_assets.Scripts.Architecture.ObjectPool.ProjectilePools;
using UnityEngine;

namespace Internal_assets.Scripts.Towers.CannonTower
{
	public class CannonTower : MonoBehaviour {
		public float m_shootInterval = 0.5f;
		public float m_range = 4f;
		public GameObject m_projectilePrefab;
		public Transform m_shootPoint;
	
		private ObjectPool<CannonProjectile> _pool;
		[SerializeField] private float shootTime = 30.0f;

		private enum TrajectoryType
		{
			Mounted,
			Flatbed
		};
		[SerializeField] private TrajectoryType trajectoryType = TrajectoryType.Mounted;

		private float m_lastShotTime = -0.5f;
	
		private void Start()
		{
			_pool = gameObject.GetComponent<CannonProjectilesPool>().Pool;
		}

		void FixedUpdate () {
			if (m_projectilePrefab == null || m_shootPoint == null)
				return;

			foreach (var monster in FindObjectsOfType<Monster>()) {
				if (Vector3.Distance (transform.position, monster.transform.position) > m_range)
					continue;

				if (m_lastShotTime + m_shootInterval > Time.time)
					continue;

				var monsterTarget = monster.m_moveTarget.transform.position;
				var monsterMove = (monsterTarget - monster.transform.position).normalized * monster.m_speed * 60 * shootTime;
				var predictedMonsterPosition = monster.transform.position + monsterMove;
				
				// shot
				Vector3 fromTo = predictedMonsterPosition - this.transform.position;
				Vector3 fromToXZ = new Vector3(fromTo.x, 0.0f, fromTo.z);

				float targetX = fromToXZ.magnitude;
				float targetY = fromTo.y;
				float g = Physics.gravity.y;

				float radAngleTan = ((g * shootTime * shootTime / 2) - targetY) / targetX;
				float degAngle = Mathf.Atan(radAngleTan) * 180 / Mathf.PI;
				float shootVelocity = targetX / (shootTime * Mathf.Cos(degAngle / 180 * Mathf.PI));

				//float dist = (shootVelocity * shootVelocity) / (g * targetX);
				//if (trajectoryType == TrajectoryType.Mounted)
				//	radAngle = dist - Mathf.Sqrt(1 + dist * dist - dist * 2 * targetY / targetX);
				//if (trajectoryType == TrajectoryType.Flatbed)
				//	radAngle = dist + Mathf.Sqrt(1 + dist * dist - dist * 2 * targetY / targetX);
				
				transform.rotation = Quaternion.LookRotation(fromToXZ, Vector3.up);
				transform.Rotate(degAngle, 0.0f, 0.0f);

				var projectile = _pool.GetFreeElement();
				projectile.transform.SetParent(null);
				projectile.transform.position = m_shootPoint.position;
				projectile.GetComponent<Rigidbody>().velocity = m_shootPoint.forward * shootVelocity;
				
				//var projectileTransform = projectile.transform;
				//projectileTransform.position = m_shootPoint.position;
				//projectileTransform.rotation = m_shootPoint.rotation;

				m_lastShotTime = Time.time;
			}

		}
	}
}
