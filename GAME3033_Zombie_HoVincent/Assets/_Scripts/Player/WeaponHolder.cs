using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Can use this script to hold weapons, even swap weapons 
/// </summary>
public class WeaponHolder : MonoBehaviour
{
    [Header("Weapon to Spawn"), SerializeField]
    GameObject weaponToSpawn;

    public PlayerController playerController;
    Animator animator;
    Sprite crosshairImage;
    WeaponComponent equippedWeapon;

    [SerializeField]
    GameObject weaponSocketLocation;
    [SerializeField]
    Transform GripIKSocketLocation;

    bool wasFiring = false;
    bool firingPressed = false;

    public readonly int isFiringHash = Animator.StringToHash("isFiring");
    public readonly int isReloadingHash = Animator.StringToHash("isReloading");

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        GameObject spawnedWeapon = Instantiate(weaponToSpawn, weaponSocketLocation.transform.position, weaponSocketLocation.transform.rotation, weaponSocketLocation.transform);

        equippedWeapon = spawnedWeapon.GetComponent<WeaponComponent>();
        equippedWeapon.Initialize(this);
        GripIKSocketLocation = equippedWeapon.gripLocation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnFire(InputValue value)
    {
        firingPressed = value.isPressed;

        if (firingPressed)
        {
            StartFiring();
        }
        else
        {
            Debug.Log("Stop Firing");
            StopFiring();
        }

        
    }

    public void OnReload(InputValue value)
    {
        playerController.isReloading = value.isPressed;
        animator.SetBool(isReloadingHash, playerController.isReloading);
    }


    private void OnAnimatorIK(int layerIndex)
    {
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, GripIKSocketLocation.transform.position);
    }

    private void StartFiring()
    {
        if (equippedWeapon.weaponStats.bulletsInClip <= 0) return;
        playerController.isFiring = true;
        animator.SetBool(isFiringHash, playerController.isFiring);
        equippedWeapon.StartFiringWeapon();
    }

    private void StopFiring()
    {
        playerController.isFiring = false;
        animator.SetBool(isFiringHash, playerController.isFiring);
        equippedWeapon.StopFiringWeapon();
        

    }

    public void StartReloading()
    {

    }

}
