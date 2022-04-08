using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class CannonProjectile : MonoBehaviour {
	public int m_damage = 10;

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

		monster.m_hp -= m_damage;
		if (monster.m_hp <= 0) {
			monster.gameObject.SetActive(false);
		}
		
		this.gameObject.SetActive(false);
	}
}
