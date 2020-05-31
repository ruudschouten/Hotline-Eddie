using System.Collections;
using UnityEngine;

namespace Characters
{
    public class MOONMOON : Boss
    {
        [SerializeField] protected float bulletPlacementRandomization;
        [SerializeField] protected int bulletsToFireAtOnce;
        [SerializeField] protected float secondsBetweenShots;

        public override void Shoot()
        {
            base.Shoot();
            StartCoroutine(ShootRoutine());
        }

        private IEnumerator ShootRoutine()
        {
            for (var i = 0; i < bulletsToFireAtOnce; i++)
            {
                if (i % 10 == 0)
                {
                    onShootEvent.Invoke();
                }

                var randomPlacement = new Vector3(
                    Random.Range(-bulletPlacementRandomization, bulletPlacementRandomization),
                    Random.Range(-bulletPlacementRandomization, bulletPlacementRandomization));
                var bullet = Instantiate(bulletPrefab, bulletEmitTransform.position + randomPlacement,
                    bulletEmitTransform.rotation);
                bullet.ShouldUpdate = true;

                yield return new WaitForSecondsRealtime(secondsBetweenShots);
            }

            ShouldInvokeRangeTell = true;
            IsRangeRecharging = true;
        }
    }
}