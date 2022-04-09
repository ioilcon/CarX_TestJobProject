using Internal_assets.Scripts.Enemies.Entities;
using UnityEngine;

namespace Internal_assets.Scripts.Architecture.ObjectPool.EnemyPools
{
    public class CapsuleEnemyPool : MonoBehaviour
    {
        [SerializeField] private Monster capsuleEnemy;
        [SerializeField] private int poolSize = 10;
        [SerializeField] private bool autoExpand;

        public ObjectPool<Monster> Pool { get; private set; }

        private void Awake()
        {
            Pool = new ObjectPool<Monster>(capsuleEnemy, poolSize, autoExpand, transform);
        }
    }
}
