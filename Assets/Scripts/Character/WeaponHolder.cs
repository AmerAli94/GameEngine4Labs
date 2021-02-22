using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class WeaponHolder : MonoBehaviour
{
    [SerializeField] private GameObject weapon;

    [SerializeField] private Transform WeaponSocket;


    private PlayerController PlayerController;
    private Animator PlayerAnimator;
    private Camera MainCamera;

    //hashes
    private readonly int AimVerticalHash = Animator.StringToHash("AimVertical");
    private readonly int AimHorizontalHash = Animator.StringToHash("AimHorizontal");
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
    }

    public void OnLook(InputValue delta)
    {
        Vector3 independantMousePosition = MainCamera.ScreenToViewportPoint(PlayerController.CrosshairComponent.CurrentMousePosition);

        PlayerAnimator.SetFloat(AimVerticalHash, independantMousePosition.y);
        PlayerAnimator.SetFloat(AimHorizontalHash, independantMousePosition.x);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
