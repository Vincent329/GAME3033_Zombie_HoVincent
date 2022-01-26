using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Can use this script to hold weapons, even swap weapons 
/// </summary>
public class WeaponHolder : MonoBehaviour
{
    [Header("Weapon to Spawn"), SerializeField]
    GameObject weaponToSpawn;

    PlayerController playerController;
    Sprite crosshairImage;

    [SerializeField]
    GameObject weaponSocketLocation;
    // Start is called before the first frame update
    void Start()
    {
        GameObject spawnedWeapon = Instantiate(weaponToSpawn, weaponSocketLocation.transform.position, weaponSocketLocation.transform.rotation, weaponSocketLocation.transform);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
