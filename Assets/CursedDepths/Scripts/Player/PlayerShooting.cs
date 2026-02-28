using UnityEngine;
using CursedDepths.Core;
using CursedDepths.Combat;

namespace CursedDepths.Player
{
    public class PlayerShooting : MonoBehaviour
    {
        [SerializeField] private string projectilePoolKey = "Projectile";

        private int damage;
        private float projectileSpeed;
        private float fireRate;
        private float reloadTime;
        private int maxAmmo;
        private int currentAmmo;
        private float fireTimer;
        private float reloadTimer;
        private bool isReloading;

        public int CurrentAmmo => currentAmmo;
        public int MaxAmmo => maxAmmo;

        public void ApplyStats(PlayerStatsData data)
        {
            damage = data.damage;
            projectileSpeed = data.projectileSpeed;
            fireRate = data.fireRate;
            reloadTime = data.reloadTime;
            maxAmmo = data.maxAmmo;
            currentAmmo = maxAmmo;
            EventBus.RaisePlayerAmmoChanged(currentAmmo, maxAmmo);
        }

        public void Tick(float deltaTime, Vector2 aimDirection)
        {
            if (fireTimer > 0f)
            {
                fireTimer -= deltaTime;
            }

            if (isReloading)
            {
                reloadTimer -= deltaTime;
                if (reloadTimer <= 0f)
                {
                    isReloading = false;
                    currentAmmo = maxAmmo;
                    EventBus.RaisePlayerAmmoChanged(currentAmmo, maxAmmo);
                }
            }
        }

        public void TryShoot(Vector2 aimDirection)
        {
            if (fireTimer > 0f || isReloading)
            {
                return;
            }

            if (currentAmmo <= 0)
            {
                StartReload();
                return;
            }

            var projectile = GameSession.Instance.PoolManager.Spawn<Projectile>(projectilePoolKey, transform.position, Quaternion.identity);
            if (projectile != null)
            {
                projectile.Fire(aimDirection, damage, projectileSpeed);
            }

            currentAmmo--;
            fireTimer = fireRate;
            EventBus.RaisePlayerAmmoChanged(currentAmmo, maxAmmo);

            if (currentAmmo <= 0)
            {
                StartReload();
            }
        }

        public void IncreaseDamage(int amount)
        {
            damage += amount;
        }

        public void IncreaseFireRate(float amount)
        {
            fireRate = Mathf.Max(0.05f, fireRate - amount);
        }

        public void IncreaseAmmo(int amount)
        {
            maxAmmo += amount;
            currentAmmo = maxAmmo;
            EventBus.RaisePlayerAmmoChanged(currentAmmo, maxAmmo);
        }

        public void IncreaseProjectileSpeed(float amount)
        {
            projectileSpeed += amount;
        }

        private void StartReload()
        {
            isReloading = true;
            reloadTimer = reloadTime;
        }
    }
}
