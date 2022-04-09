using Internal_assets.Scripts.Projectiles.GuidedProjectile;
using UnityEngine;

namespace Internal_assets.Scripts.Architecture.ObjectPool.ProjectilePools
{
    public class GuidedProjectilesPool : MonoBehaviour
    {
        [SerializeField] private GuidedProjectile projectilePrefab;
        [SerializeField] private int poolSize = 10;
        [SerializeField] private bool autoExpand;

        public ObjectPool<GuidedProjectile> Pool { get; private set; }

        private void Awake()
        {
            Pool = new ObjectPool<GuidedProjectile>(projectilePrefab, poolSize, autoExpand, transform);
        }
    }
}
