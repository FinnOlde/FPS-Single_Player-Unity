using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRenderer : MonoBehaviour
{
    public MeshFilter Mesh;
    public MeshRenderer Material;
    public Vector3 RelPos;
    public Vector3 RelRot;

    private Vector3 originPos;
    private Vector3 originRot;

    public void Start()
    {
        originPos = transform.localPosition;
        originRot = transform.localRotation.eulerAngles;
    }

    public void Update()
    {
        transform.localPosition = originPos + RelPos;
        transform.localRotation = Quaternion.Euler(originRot + RelRot);
    }

    public void SetWeapon(Mesh weaponMesh, Material weaponMaterial, Vector3 relPos, Vector3 relRot)
    {
        Mesh.mesh = weaponMesh;
        Material.material = weaponMaterial;
        RelPos = relPos;
        RelRot = relRot;
    }
}
