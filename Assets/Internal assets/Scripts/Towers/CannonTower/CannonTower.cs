using Internal_assets.Scripts.Architecture.ObjectPool;
using Internal_assets.Scripts.Architecture.ObjectPool.ProjectilePools;
using Internal_assets.Scripts.Enemies.Entities;
using Internal_assets.Scripts.Projectiles.CannonProjectile;
using UnityEngine;

namespace Internal_assets.Scripts.Towers.CannonTower
{
	public class CannonTower : MonoBehaviour {
		public float shootInterval = 0.5f;
		public float range = 4f;
		[SerializeField] public Transform cannonTransform;
		[SerializeField] private float aimSpeed = 10.0f;
		[SerializeField] private float shootTime = 3.0f;
		
		private ObjectPool<CannonProjectile> _pool;
		private float _lastShotTime = 0.0f;
		private bool _isTargetLocked;
		private bool _isAimed;
		private Monster _target = null;
	
		private void Start()
		{
			_pool = gameObject.GetComponent<CannonProjectilesPool>().Pool;
		}

		void FixedUpdate () {
			if (cannonTransform == null)
				return;

			_lastShotTime += Time.deltaTime;
			
			if (_target == null)
			{
				var monsters = FindObjectsOfType<Monster>();
				foreach (var monster in monsters)
				{
					if (Vector3.Distance(transform.position, monster.transform.position) < range)
					{
						_target = monster;
					}
				}
				if (_target == null)
					return;
			}
			else if (Vector3.Distance(transform.position, _target.transform.position) > range || !_target.isActiveAndEnabled) {
				_target = null;
				return;
			}
			
			//foreach (var monster in FindObjectsOfType<Monster>()) {
			//	if (Vector3.Distance (transform.position, monster.transform.position) > range)
			//		continue;

				float shootVelocity = AimOnTarget(_target);

				if (_lastShotTime > shootInterval)
				{
					if (_isTargetLocked && _isAimed)
					{
						var projectile = _pool.GetFreeElement();
						projectile.transform.SetParent(null);
						projectile.transform.position = cannonTransform.position;
						projectile.transform.rotation = cannonTransform.rotation;
						projectile.currentWickLength = projectile.wickLength;
						projectile.GetComponent<Rigidbody>().velocity = cannonTransform.forward * shootVelocity;

						_lastShotTime = 0;
						_isTargetLocked = false;
						_isAimed = false;
					}
				}
			//}

		}

		private float AimOnTarget(Monster monster)
		{
			// Упреждение выстрела. Вычисляется положение цели во время прилёта ядра
			// Примерно 50 "шагов" в секунду из-за перемещения цели через FixedUpdate()
			var monsterPosition = monster.transform.position;
			var monsterMove = (monster.moveTarget.transform.position - monsterPosition).normalized *
			                  monster.moveSpeed * 50 * shootTime;
			var predictedMonsterPosition = monsterPosition + monsterMove;

			Vector3 fromTo = predictedMonsterPosition - cannonTransform.position;
			// направление наобходимого для попадания поворота башни
			Vector3 fromToXZ = new Vector3(fromTo.x, 0.0f, fromTo.z);

			// Необходимые параметры для вычисления угла и скорости снаряда
			float targetX = fromToXZ.magnitude;
			float targetY = Mathf.Abs(fromTo.y);
			float g = Physics.gravity.y;

			float angleTan = (targetY + (g * shootTime * shootTime / 2)) / targetX;
			float degAngle = Mathf.Atan(angleTan) * Mathf.Rad2Deg;

			// направление необходимого для попадания подъёма ствола пушки
			var aimVector = transform.forward + Vector3.up * Mathf.Abs(angleTan);
			
			Debug.DrawRay(transform.position, transform.forward * 7, Color.yellow);
			Debug.DrawRay(transform.position, fromToXZ, Color.red);
			Debug.DrawRay(cannonTransform.position, cannonTransform.forward * 7, Color.green);
			Debug.DrawRay(cannonTransform.position, aimVector * 7, Color.magenta);
			
			// поворот башни и поворот ствола пушки
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(fromToXZ),
				aimSpeed * Time.deltaTime);
			cannonTransform.rotation = Quaternion.RotateTowards(cannonTransform.rotation,
				Quaternion.LookRotation(aimVector), aimSpeed * Time.deltaTime);

			_isTargetLocked = false;
			_isAimed = false;
			
			// проверка, захвачена ли цель и навелась ли пушка
			var angleToTarget = Vector3.Angle(this.transform.forward, fromToXZ);
			var angleToAim = Vector3.Angle(cannonTransform.forward, aimVector);
			if (angleToTarget < 0.1f)
			{
				_isTargetLocked = true;
			}
			if (angleToAim < 0.1f)
			{
				_isAimed = true;
			}
			return targetX / (shootTime * Mathf.Cos(degAngle / 180 * Mathf.PI));
		}

	}
}
