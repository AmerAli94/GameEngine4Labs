using System;
using UnityEngine;


namespace Weapons
{
    public enum WeaponType
    {
        None,
        Machinegun,
        Pistol
    }

    [Serializable]
     public struct WeaponStats
    {
        public WeaponType weapontype;
        public string Name;
        public float Dmage;
        public int BulletsInClip;
        public int Clipsize, TotalBulletsAvailabale;

        public float FireStartDelay, FireRate, FireDistance;
        public bool Repeating;

        public LayerMask WeaponHitLayer;
    }
    public class WeaponComponent : MonoBehaviour
    {
        public bool Firing { get; private set; }

        public bool Reloading { get; private set; }

        [SerializeField] public WeaponStats WeaponStats;

        protected WeaponHolder WeaponHolder;
        protected crosshairscript Crosshair;

        public void Initialize(WeaponHolder weaponHolder, crosshairscript crosshair)
        {
            WeaponHolder = weaponHolder;
            Crosshair = crosshair;
        }


        public virtual void StartFiring()
        {
            Firing = true;
            if (WeaponStats.Repeating)
            {
                InvokeRepeating(nameof(FireWeapon), WeaponStats.FireStartDelay, WeaponStats.FireRate);
            }
            else
            {
                FireWeapon();
            }
        }

        public virtual void StopFiring()
        {
            Firing = true;
            CancelInvoke(nameof(FireWeapon));
        }

        protected virtual void FireWeapon()
        {
            WeaponStats.BulletsInClip--;
        }

        public virtual void StartReloading()
        {
            Reloading = true;
            ReloadWeapon();
        }
        public virtual void StopReloading()
        {
            Reloading = false;
        }

        protected virtual void ReloadWeapon()
        {
            int bulletToReload = WeaponStats.Clipsize - WeaponStats.TotalBulletsAvailabale;
            if (bulletToReload < 0)
            {
                WeaponStats.BulletsInClip += WeaponStats.Clipsize;
                WeaponStats.TotalBulletsAvailabale -= WeaponStats.Clipsize;
            }
            else
            {
                Debug.Log("Reload");
                WeaponStats.BulletsInClip = WeaponStats.TotalBulletsAvailabale;
                WeaponStats.TotalBulletsAvailabale -= 0;
            }
        }

    }

}