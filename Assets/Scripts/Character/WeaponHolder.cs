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


        private PlayerController PlayerController;
        private Animator PlayerAnimator;
        private Camera MainCamera;
        private WeaponComponent EquippedWeapon;
        //hashes
        private readonly int AimVerticalHash = Animator.StringToHash("AimVertical");
        private readonly int AimHorizontalHash = Animator.StringToHash("AimHorizontal");
        private readonly int IsFiringHash = Animator.StringToHash("IsFiring");
        private readonly int IsReloadingHash = Animator.StringToHash("IsReloading");

        private void Awake()
        {
            PlayerController = GetComponent<PlayerController>();
            PlayerAnimator = GetComponent<Animator>();
            MainCamera = Camera.main;
        }
        void Start()
        {
            GameObject spawnedWeapon = Instantiate(weapon, WeaponSocket.position, WeaponSocket.rotation);
            if (!spawnedWeapon) return;
            spawnedWeapon.transform.parent = WeaponSocket;

            EquippedWeapon = spawnedWeapon.GetComponent<WeaponComponent>();

            EquippedWeapon.Initialize(this, PlayerController.CrosshairComponent);

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
            if (button.isPressed)
            {
                PlayerController.IsFiring = true;
                PlayerAnimator.SetBool(IsFiringHash, PlayerController.IsFiring);
                EquippedWeapon.StartFiring();
            }
            else
            {
                PlayerController.IsFiring = false;
                PlayerAnimator.SetBool(IsFiringHash, PlayerController.IsFiring);
                EquippedWeapon.StopFiring();
            }
        }

        public void OnReload(InputValue button)
        {
            PlayerController.IsReloading = true;
            PlayerAnimator.SetBool(IsReloadingHash, PlayerController.IsReloading);
        }

        public void StartReloading()
        {
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
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}