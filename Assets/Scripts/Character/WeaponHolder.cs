using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Weapons
{
    public class WeaponHolder : MonoBehaviour
    {
        [SerializeField] private GameObject weapon;

        [SerializeField] private Transform WeaponSocket;

        public PlayerController Controller => PlayerController;
        private PlayerController PlayerController;
        private Animator PlayerAnimator;
        private Camera MainCamera;
        private WeaponComponent EquippedWeapon;

        private bool WasFiring = false;
        private bool FiringPressed = false;

        //hashes
        private readonly int AimVerticalHash = Animator.StringToHash("AimVertical");
        private readonly int AimHorizontalHash = Animator.StringToHash("AimHorizontal");
        private readonly int IsFiringHash = Animator.StringToHash("IsFiring");
        private readonly int IsReloadingHash = Animator.StringToHash("IsReloading");
        private static readonly int WeaponTypeHash = Animator.StringToHash("WeaponType");

        private void Awake()
        {
            PlayerController = GetComponent<PlayerController>();
            PlayerAnimator = GetComponent<Animator>();
            MainCamera = Camera.main;
        }
        private void Start()
        {
            GameObject spawnedWeapon = Instantiate(weapon, WeaponSocket.position, WeaponSocket.rotation);
            if (!spawnedWeapon) return;
            spawnedWeapon.transform.parent = WeaponSocket;

            EquippedWeapon = spawnedWeapon.GetComponent<WeaponComponent>();

            EquippedWeapon.Initialize(this, PlayerController.CrosshairComponent);

            PlayerAnimator.SetInteger(WeaponTypeHash, (int)EquippedWeapon.WeaponStats.weapontype);
            PlayerEvents.Invoke_OnWeaponEquipped(EquippedWeapon);
        }

        public void OnLook(InputValue delta)
        {
            Vector3 independantMousePosition = MainCamera.ScreenToViewportPoint(PlayerController.CrosshairComponent.CurrentMousePosition);

            PlayerAnimator.SetFloat(AimVerticalHash, independantMousePosition.y);
            PlayerAnimator.SetFloat(AimHorizontalHash, independantMousePosition.x);
        }
        public void OnFire(InputValue button)
        {

            FiringPressed = button.isPressed;

            if (button.isPressed)
                StartFiring();
            else
                StopFiring();
            
        }

        private void StartFiring()
        {
            if (EquippedWeapon.WeaponStats.TotalBulletsAvailabale <= 0 && EquippedWeapon.WeaponStats.BulletsInClip <= 0) return;
         
            PlayerController.IsFiring = true;
            PlayerAnimator.SetBool(IsFiringHash, PlayerController.IsFiring);
            EquippedWeapon.StartFiring();
        }

        private void StopFiring()
        {
            PlayerController.IsFiring = false;
            PlayerAnimator.SetBool(IsFiringHash, PlayerController.IsFiring);
            EquippedWeapon.StopFiring();
        }

        public void OnReload(InputValue button)
        {
            PlayerController.IsReloading = true;
            PlayerAnimator.SetBool(IsReloadingHash, PlayerController.IsReloading);
        }

        public void StartReloading()
        {
            if(EquippedWeapon.WeaponStats.TotalBulletsAvailabale <= 0 && PlayerController.IsFiring)
            {
                StopFiring();
                return;
            }
            PlayerController.IsReloading = true;
            PlayerAnimator.SetBool(IsReloadingHash, PlayerController.IsReloading);
            EquippedWeapon.StartReloading();

            InvokeRepeating(nameof(StopRloading), 0, 0.1f);
        }

        public void StopRloading()
        {
            if (PlayerAnimator.GetBool(IsReloadingHash)) return;

            PlayerController.IsReloading = false;
            EquippedWeapon.StopReloading();

            CancelInvoke(nameof(StopRloading));

            if(WasFiring || FiringPressed)
            {
                StartFiring();
                WasFiring = false;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}