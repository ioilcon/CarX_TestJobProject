using Internal_assets.Scripts.Architecture.ObjectPool;
using Internal_assets.Scripts.Architecture.ObjectPool.ProjectilePools;
using UnityEngine;

namespace Internal_assets.Scripts.Towers.CannonTower
{
	public class CannonTower : MonoBehaviour {
		public float m_shootInterval = 0.5f;
		public float m_range = 4f;
		public GameObject m_projectilePrefab;
		[SerializeField] public Transform cannonTransform;
	
		private ObjectPool<CannonProjectile> _pool;
		[SerializeField] private float aimSpeed = 10.0f;
		[SerializeField] private float shootTime = 3.0f;
		private float m_lastShotTime = -0.5f;
		private bool isTargetLocked = false;
		private bool isAimed = false;
	
		private void Start()
		{
			_pool = gameObject.GetComponent<CannonProjectilesPool>().Pool;
		}

		void FixedUpdate () {
			if (m_projectilePrefab == null || cannonTransform == null)
				return;

			foreach (var monster in FindObjectsOfType<Monster>()) {
				if (Vector3.Distance (transform.position, monster.transform.position) > m_range)
					continue;

				AimOnTarget(monster);
				
				if (m_lastShotTime + m_shootInterval > Time.time)
					continue;

				var monsterTarget = monster.m_moveTarget.transform.position;
				var monsterMove = (monsterTarget - monster.transform.position).normalized * monster.m_speed * 50 * shootTime;
				var predictedMonsterPosition = monster.transform.position + monsterMove;

				// shot
				Vector3 fromTo = predictedMonsterPosition - cannonTransform.position;
				Vector3 fromToXZ = new Vector3(fromTo.x, 0.0f, fromTo.z);
				
				float targetX = fromToXZ.magnitude;
				float targetY = Mathf.Abs(fromTo.y);
				float g = Physics.gravity.y;

				float radAngleTan = (targetY + (g * shootTime * shootTime / 2)) / targetX;
				float degAngle = Mathf.Atan(radAngleTan) * 180 / Mathf.PI;
				float shootVelocity = targetX / (shootTime * Mathf.Cos(degAngle / 180 * Mathf.PI));

				//float dist = (shootVelocity * shootVelocity) / (g * targetX);
				//if (trajectoryType == TrajectoryType.Mounted)
				//	radAngle = dist - Mathf.Sqrt(1 + dist * dist - dist * 2 * targetY / targetX);
				//if (trajectoryType == TrajectoryType.Flatbed)
				//	radAngle = dist + Mathf.Sqrt(1 + dist * dist - dist * 2 * targetY / targetX);
				
				//transform.rotation = Quaternion.LookRotation(fromToXZ, Vector3.up);
				//transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(fromToXZ), rotateSpeed * Time.deltaTime);
				//cannonTransform.rotation = Quaternion.LookRotation(fromToXZ, Vector3.up);
				//cannonTransform.Rotate(degAngle, 0.0f, 0.0f);
				//m_shootPoint.rotation = Quaternion.LookRotation(fromToXZ, Vector3.up);
				//m_shootPoint.Rotate(degAngle, 0.0f, 0.0f);
				
				if (isTargetLocked && isAimed)
				{
					var projectile = _pool.GetFreeElement();
					//Debug.DrawLine(predictedMonsterPosition + Vector3.down * 10, predictedMonsterPosition + Vector3.up * 2, Color.red, shootTime);
					projectile.transform.SetParent(null);
					projectile.transform.position = cannonTransform.position;
					projectile.transform.rotation = cannonTransform.rotation;
					projectile.currentWickLength = projectile.wickLength;
					//Debug.DrawLine(predictedMonsterPosition, m_shootPoint.position, Color.red, shootTime);
					projectile.GetComponent<Rigidbody>().velocity = cannonTransform.forward * shootVelocity;
					isTargetLocked = false;

					//var projectileTransform = projectile.transform;
					//projectileTransform.position = m_shootPoint.position;
					//projectileTransform.rotation = m_shootPoint.rotation;

					m_lastShotTime = Time.time;
				}
			}

		}

		private void AimOnTarget(Monster monster)
		{
			var monsterTarget = monster.m_moveTarget.transform.position;
			var monsterMove = (monsterTarget - monster.transform.position).normalized * monster.m_speed * 50 * shootTime;
			var predictedMonsterPosition = monster.transform.position + monsterMove;

			Vector3 fromTo = predictedMonsterPosition - cannonTransform.position;
			Vector3 fromToXZ = new Vector3(fromTo.x, 0.0f, fromTo.z);

			float targetX = fromToXZ.magnitude;
			float targetY = Mathf.Abs(fromTo.y);
			float g = Physics.gravity.y;

			float radAngleTan = (targetY + (g * shootTime * shootTime / 2)) / targetX;
			float degAngle = Mathf.Atan(radAngleTan) * 180 / Mathf.PI;
			
			var aimVector = Quaternion.Euler(degAngle, 0.0f, 0.0f) * fromToXZ;
			
			Debug.Log("fromToXZ: " + fromToXZ + "\nquat: " + Quaternion.Euler(degAngle, 0.0f, 0.0f) + "\naimVector: " + aimVector + "\n-------------------------");
			Debug.DrawRay(transform.position, transform.forward * 7, Color.yellow);
			Debug.DrawRay(transform.position, fromToXZ, Color.red);
			Debug.DrawRay(cannonTransform.position, cannonTransform.forward * 7, Color.green);
			Debug.DrawRay(cannonTransform.position, aimVector * 7, Color.magenta);
			Debug.Log( "Needed angle: " + degAngle +"\nReal angle: " + Vector3.Angle(fromToXZ, aimVector));
			
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(fromToXZ),
				aimSpeed * Time.deltaTime);
			cannonTransform.rotation = Quaternion.RotateTowards(cannonTransform.rotation,
				Quaternion.LookRotation(aimVector), aimSpeed * Time.deltaTime);

			
			
			var angleToTarget = Vector3.Angle(this.transform.forward, fromToXZ);
			var angleToAim = Vector3.Angle(cannonTransform.forward, aimVector);
			isTargetLocked = false;
			isAimed = false;
			if (angleToTarget < 1.0f)
			{
				isTargetLocked = true;
			}
			if (angleToAim < 1.0f)
			{
				isAimed = true;
			}
		}

	}
}
