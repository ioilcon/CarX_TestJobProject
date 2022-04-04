using UnityEngine;
using System.Collections;

public class CannonTower : MonoBehaviour {
	public float m_shootInterval = 0.5f;
	public float m_range = 4f;
	public GameObject m_projectilePrefab;
	public Transform m_shootPoint;
	
	private ObjectPool<CannonProjectile> _pool;

	private float m_lastShotTime = -0.5f;
	
	private void Start()
	{
		_pool = gameObject.GetComponent<CannonProjectilesPool>().Pool;
	}

	void Update () {
		if (m_projectilePrefab == null || m_shootPoint == null)
			return;

		foreach (var monster in FindObjectsOfType<Monster>()) {
			if (Vector3.Distance (transform.position, monster.transform.position) > m_range)
				continue;

			if (m_lastShotTime + m_shootInterval > Time.time)
				continue;

			// shot
			var projectile = _pool.GetFreeElement();
			var projectileTransform = projectile.transform;
			projectileTransform.position = m_shootPoint.position;
			projectileTransform.rotation = m_shootPoint.rotation;

			m_lastShotTime = Time.time;
		}

	}
}
