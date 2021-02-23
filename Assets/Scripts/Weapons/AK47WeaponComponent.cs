using UnityEngine;

namespace Weapons
{
    public class AK47WeaponComponent : WeaponComponent
    {
        private Camera ViewCamera;
        private RaycastHit HitLocation;
        private new void Awake()
        {
            ViewCamera = Camera.main;
        }
        protected new void FireWeapon()
        {
            Ray screenRay = ViewCamera.ScreenPointToRay(new Vector3(Crosshair.CurrentMousePosition.x, Crosshair.CurrentMousePosition.y, 0));

            if (!Physics.Raycast(screenRay, out RaycastHit hit, WeaponStats.FireDistance, WeaponStats.WeaponHitLayer))
            {
                return;
            }
            HitLocation = hit;
        }

        private void OnDrawGizmos()
        {
            if(HitLocation.transform)
            {
                Gizmos.DrawSphere(HitLocation.point, 0.2f);
            }
        }
    }
}
