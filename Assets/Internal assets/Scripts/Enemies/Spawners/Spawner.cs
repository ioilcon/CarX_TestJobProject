using Internal_assets.Scripts.Architecture.ObjectPool;
using Internal_assets.Scripts.Architecture.ObjectPool.EnemyPools;
using Internal_assets.Scripts.Enemies.Entities;
using UnityEngine;

namespace Internal_assets.Scripts.Enemies.Spawners
{
	public class Spawner : MonoBehaviour {
		
		public float spawnInterval = 5;
		
		private ObjectPool<Monster> _pool;
		private float _lastSpawn = 0.0f;

		private void Start()
		{
			_pool = gameObject.GetComponent<CapsuleEnemyPool>().Pool;
		}
	
		void FixedUpdate ()
		{
			_lastSpawn += Time.deltaTime;
			if (_lastSpawn > spawnInterval) {
				var newMonster = _pool.GetFreeElement();
				newMonster.transform.position = transform.position;
				newMonster.currentHp = newMonster.maxHp;

				_lastSpawn %= spawnInterval;
			}
		}
	}
}
