using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedProjectilesPool : MonoBehaviour
{
    [SerializeField] private GuidedProjectile projectilePrefab;
    [SerializeField] private int poolSize = 10;
    [SerializeField] private bool autoExpand;

    public ObjectPool<GuidedProjectile> Pool { get; private set; }

    private void Start()
    {
        Pool = new ObjectPool<GuidedProjectile>(projectilePrefab, poolSize, autoExpand, transform);
    }
}
