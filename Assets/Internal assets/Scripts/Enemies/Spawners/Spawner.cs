using Internal_assets.Scripts.Architecture.ObjectPool;
using Internal_assets.Scripts.Architecture.ObjectPool.EnemyPools;
using Internal_assets.Scripts.Architecture.ObjectPool.ProjectilePools;
using UnityEngine;

namespace Internal_assets.Scripts.Enemies.Spawners
{
	public class Spawner : MonoBehaviour {
		public float m_interval = 3;
		private ObjectPool<Monster> _pool;
	
		private float m_lastSpawn = -1;

		private void Start()
		{
			_pool = gameObject.GetComponent<CapsuleEnemyPool>().Pool;
		}
	
		void FixedUpdate () {
			if (Time.time > m_lastSpawn + m_interval) {
				var newMonster = _pool.GetFreeElement();
				newMonster.transform.position = this.transform.position;
				newMonster.m_hp = newMonster.m_maxHP;

				m_lastSpawn = Time.time;
			}
		}
	}
}
