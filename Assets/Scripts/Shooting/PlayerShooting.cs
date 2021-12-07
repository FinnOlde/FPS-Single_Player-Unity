using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Transform ShotOrigin;

    public List<Weapon> WeaponList;
    private int equipedWeapon = 0;

    public GunRenderer GunDisplay;
    public AudioSource GunAudioSource;

    private float delay;
    private bool isReloading;

    private Shot shot = new Shot();
    private float recoil = 0;

    public GameObject TEMP_HitMarkerPrefab;

    public void Start()
    {
        WeaponList = new List<Weapon>();
    }

    public void Update()
    {
        delay -= Time.deltaTime;
        WeaponSwitching();
        WeaponReloading();
        WeaponShooting();
    }

    private void WeaponShooting()
    {
        if (WeaponList.Count > 0)
        {
            recoil = Mathf.Clamp(recoil - (WeaponList[equipedWeapon].data.RecoilRecovery * Time.deltaTime), 0, 1);
            if (Input.GetMouseButton(0))
            {
                while (delay < 0 && WeaponList[equipedWeapon].loadedAmmo > 0)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        delay = WeaponList[equipedWeapon].data.ShotDelay;
                    }
                    else if (WeaponList[equipedWeapon].data.FullAuto)
                    {
                        delay += WeaponList[equipedWeapon].data.ShotDelay;
                    }
                    else
                    {
                        break;
                    }
                    Shoot();
                    GunAudioSource.time = WeaponList[equipedWeapon].data.SoundStartTime;
                    GunAudioSource.Play();
                    recoil = Mathf.Clamp(recoil + WeaponList[equipedWeapon].data.Recoil, 0, 1);
                    WeaponList[equipedWeapon].loadedAmmo--;
                }
            }
        }
    }

    private void Shoot()
    {
        for (int i = 0; i < WeaponList[equipedWeapon].data.bulletsPerShot; i++)
        {
            shot.SetValues(ShotOrigin, Mathf.Lerp(WeaponList[equipedWeapon].data.MinSpread, WeaponList[equipedWeapon].data.MaxSpread, recoil));
            if (((bool)shot))
            {
                if (((RaycastHit)shot).collider.CompareTag("WorldGeometrie"))
                {
                    GameObject t = GameObject.Instantiate(TEMP_HitMarkerPrefab, ((RaycastHit)shot).point, Quaternion.identity);
                    StartCoroutine(nameof(TEMP_KillAfter), new TEMP_killInfo(15, t));
                }
            }
        }
    }

    private IEnumerator TEMP_KillAfter(TEMP_killInfo kill)
    {
        yield return new WaitForSeconds(kill.time);
        Destroy(kill.toKill);
    }

    struct TEMP_killInfo
    {
        public readonly float time;
        public readonly GameObject toKill;

        public TEMP_killInfo(float time, GameObject toKill)
        {
            this.time = time;
            this.toKill = toKill;
        }
    }

    private void WeaponReloading()
    {
        if (Input.GetKeyDown(KeyCode.R) && delay < 0)
        {
            if (WeaponList[equipedWeapon].loadedAmmo != WeaponList[equipedWeapon].data.AmmoClipSize)
            {
                isReloading = true;
                delay = WeaponList[equipedWeapon].data.ReloadTime;
            }
        }
        if (isReloading && delay < 0)
        {
            isReloading = false;
            WeaponList[equipedWeapon].loadedAmmo = WeaponList[equipedWeapon].data.AmmoClipSize;
            delay = WeaponList[equipedWeapon].data.AfterReloadDeployTime;
        }
    }

    private void WeaponSwitching()
    {
        if (WeaponList.Count > 0)
        {
            equipedWeapon += (int)Input.mouseScrollDelta.y + WeaponList.Count;
            equipedWeapon %= WeaponList.Count;
            if (Input.mouseScrollDelta.y != 0)
            {
                GunDisplay.SetWeapon(WeaponList[equipedWeapon].data.WeaponMesh, WeaponList[equipedWeapon].data.WeaponMaterial, WeaponList[equipedWeapon].data.RelPos, WeaponList[equipedWeapon].data.RelRot);
                GunAudioSource.clip = WeaponList[equipedWeapon].data.WeaponSound;
                delay = WeaponList[equipedWeapon].data.DeployTime;
                isReloading = false;
            }
        }
    }

    public void AddWeapon(WeaponScriptableObject newWeapon, int loadedAmmo)
    {
        WeaponList.Add(new Weapon(newWeapon, loadedAmmo));
        if (WeaponList.Count == 1)
        {
            GunDisplay.SetWeapon(WeaponList[equipedWeapon].data.WeaponMesh, WeaponList[equipedWeapon].data.WeaponMaterial, WeaponList[equipedWeapon].data.RelPos, WeaponList[equipedWeapon].data.RelRot);
            GunAudioSource.clip = WeaponList[equipedWeapon].data.WeaponSound;
        }
        else if (WeaponList.Count == 0)
        {
            GunDisplay.SetWeapon(null, null, Vector3.zero, Vector3.zero);
        }
    }

    public class Weapon
    {
        public WeaponScriptableObject data;
        public int loadedAmmo;

        public Weapon(WeaponScriptableObject data, int loadedAmmo)
        {
            this.data = data;
            this.loadedAmmo = loadedAmmo;
        }
    }

    private class Shot
    {
        private ShotPositionData shotInfo;
        private float spreadAngle;

        private RaycastHit hit;
        private Ray ray;
        private bool didHit;

        public void SetValues(Transform shotOrigin, float maxSpread)
        {
            spreadAngle = maxSpread;
            shotInfo = new ShotPositionData(shotOrigin.position, shotOrigin.rotation);
            SetRay();
            didHit = Physics.Raycast(ray, out hit);
        }

        ///<summary>
        ///Infos of the shot hit if it hit something.
        ///</summary>
        public static explicit operator RaycastHit(Shot s)
        {
            return s.hit;
        }

        ///<summary>
        ///Did the Shot hit something.
        ///</summary>
        public static explicit operator bool(Shot s)
        {
            return s.didHit;
        }

        public void SetRay()
        {
            Vector3 dir = shotInfo.forward;
            Vector2 spreadAxis = new Vector2(UnityEngine.Random.Range(0, spreadAngle), UnityEngine.Random.Range(0, 360));
            dir = Quaternion.AngleAxis(spreadAxis.x, shotInfo.right) * dir;
            dir = Quaternion.AngleAxis(spreadAxis.y, shotInfo.forward) * dir;
            ray.origin = shotInfo.position;
            ray.direction = dir;
        }

        private struct ShotPositionData
        {
            public readonly Vector3 position;
            public readonly Quaternion rotation;

            public ShotPositionData(Vector3 position, Quaternion rotation)
            {
                this.position = position;
                this.rotation = rotation;
            }

            public Vector3 forward { get { return rotation * Vector3.forward; } }
            public Vector3 right { get { return rotation * Vector3.right; } }
            public Vector3 up { get { return rotation * Vector3.up; } }
        }
    }
}
