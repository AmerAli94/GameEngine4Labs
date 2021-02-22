using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public crosshairscript CrosshairComponent => Crosshairscript;
    [SerializeField] private crosshairscript Crosshairscript;
    public bool IsFiring, IsReloading, IsJumping, IsRunning;
}

