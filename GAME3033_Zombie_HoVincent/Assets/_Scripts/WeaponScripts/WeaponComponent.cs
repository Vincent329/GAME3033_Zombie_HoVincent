using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    ASSAULTRIFLE,
    PISTOL,
    MACHINEGUN,
}
public enum WeaponFiringPattern
{
    SEMIAUTO,
    FULLAUTO,
    CANNON,
}

[System.Serializable]
public struct WeaponStats
{
    public WeaponType weaponType;
    public string weaponName;
    public float damage;
    public int bulletsInClip;
    public int clipSize;
    public float fireStartDelay;
    public float fireRate;
    public float fireDistance;
    public bool repeating;
    public LayerMask weaponHitLayers;
    public int totalBullets;
}


public class WeaponComponent : MonoBehaviour
{
    public Transform gripLocation;
    public Transform firingEffectLocation;

    public WeaponHolder weaponHolder;
    [SerializeField]
    protected ParticleSystem firingEffect;

    public bool isFiring = false;
    public bool isReloading = false;

    protected Camera mainCamera;

    [SerializeField]
    public WeaponStats weaponStats;
    // Start is called before the first frame update
    void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(WeaponHolder _weaponHolder)
    {
        weaponHolder = _weaponHolder;
    }

    // decide whether it is auto or semiauto here
    public virtual void StartFiringWeapon() { 
        isFiring = true; 
        if (weaponStats.repeating)
        {
            InvokeRepeating(nameof(FireWeapon), weaponStats.fireStartDelay, weaponStats.fireRate);
        }
        else
        {
            FireWeapon();
        }
    }
    public virtual void StopFiringWeapon() { 
        isFiring = false;
        
        CancelInvoke(nameof(FireWeapon));
        if (firingEffect)
        {
            firingEffect.Stop();
        }
    }

    protected virtual void FireWeapon() {
        Debug.Log("Firing Weapon");

        weaponStats.bulletsInClip--;
    }

    //Deal with Ammo Counts and perhaps Particle Effects
    public virtual void StartReloading()
    {
        isReloading = true;
        ReloadWeapon();
    }

    public virtual void StopReloading()
    {
        isReloading = false;
        if (firingEffect)
        {
            firingEffect.Stop();
        }
    }

    protected virtual void ReloadWeapon()
    {
        // if there's a firing effect, hide it here
        int bulletsToReload = weaponStats.clipSize - weaponStats.totalBullets;
        if (bulletsToReload < 0)
        {
            weaponStats.bulletsInClip = weaponStats.clipSize;
            weaponStats.totalBullets -= weaponStats.clipSize;
        }
        else
        {
            weaponStats.bulletsInClip = weaponStats.totalBullets;
            weaponStats.totalBullets = 0;
        }
    }

}
