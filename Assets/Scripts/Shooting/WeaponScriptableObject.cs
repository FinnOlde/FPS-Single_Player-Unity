using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/New Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    public string DisplayName = "No Name";

    //timeings
    public float ShotDelay;
    public float ReloadTime;
    public float AfterReloadDeployTime;
    public float DeployTime;

    public bool FullAuto;

    //Ammo
    public int AmmoClipSize;

    //Shot
    public float BaseDamage;
    public int bulletsPerShot;

    //Spread
    public float MinSpread;
    public float MaxSpread;

    [Range(0, 1)]
    public float Recoil;
    public float RecoilRecovery;

    //Art
    public Mesh WeaponMesh;
    public Material WeaponMaterial;
    public AudioClip WeaponSound;
    public float SoundStartTime;
    public Vector3 RelPos;
    public Vector3 RelRot;

    public void OnValidate()
    {
        if(ShotDelay <= 0){
            ShotDelay = .001f;
            Debug.Log("Shot Delay can't be set any lower than 0.001 seconds.");
        }
    }
}
