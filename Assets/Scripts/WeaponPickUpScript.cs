using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUpScript : MonoBehaviour
{
    public WeaponScriptableObject Weapon;
    public int CurrentAmmo;

    public void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            FindObjectOfType<PlayerShooting>().AddWeapon(Weapon, CurrentAmmo);
            Debug.Log($"Picked up {Weapon.name}.");
            Destroy(gameObject);
        }
    }
}
