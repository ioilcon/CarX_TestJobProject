using UnityEngine;

namespace Internal_assets.Scripts.Architecture.ObjectPool.ProjectilePools
{
    public class CannonProjectilesPool : MonoBehaviour
    {
        [SerializeField] private CannonProjectile projectilePrefab;
        [SerializeField] private int poolSize = 10;
        [SerializeField] private bool autoExpand;

        public ObjectPool<CannonProjectile> Pool { get; private set; }

        private void Awake()
        {
            Pool = new ObjectPool<CannonProjectile>(projectilePrefab, poolSize, autoExpand, transform);
        }
    }
}
