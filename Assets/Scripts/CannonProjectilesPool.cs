using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonProjectilesPool : MonoBehaviour
{
    [SerializeField] private CannonProjectile projectilePrefab;
    [SerializeField] private int poolSize = 10;
    [SerializeField] private bool autoExpand;

    public ObjectPool<CannonProjectile> Pool { get; private set; }

    private void Start()
    {
        Pool = new ObjectPool<CannonProjectile>(projectilePrefab, poolSize, autoExpand, transform);
    }
}
