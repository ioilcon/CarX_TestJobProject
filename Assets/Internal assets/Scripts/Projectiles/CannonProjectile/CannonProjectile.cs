using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class CannonProjectile : MonoBehaviour {
	public float m_speed = 0.2f;
	public int m_damage = 10;

	[SerializeField] private float wickLength = 5.0f;
	private float _currentWickLength;

	private void Start()
	{
		_currentWickLength = wickLength;
	}

	void Update () {
		//var translation = transform.forward * m_speed;
		_currentWickLength -= Time.deltaTime;
		if (_currentWickLength < 0)
		{
			_currentWickLength = wickLength;
			this.gameObject.SetActive(false);
		}
		//transform.Translate (translation);
	}

	void OnTriggerEnter(Collider other) {
		var monster = other.gameObject.GetComponent<Monster> ();
		if (monster == null)
			return;

		monster.m_hp -= m_damage;
		if (monster.m_hp <= 0) {
			monster.gameObject.SetActive(false);
		}
		
		this.gameObject.SetActive(false);
	}
}
