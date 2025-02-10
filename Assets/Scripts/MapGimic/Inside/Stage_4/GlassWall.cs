using RayFire;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GlassWall : MonoBehaviour
{
    public GameObject realWall;
    public GameObject glassWall;
    public RayfireRigid rayFire;
    public RayfireBomb bomb;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            CrashGlass();
        }
    }

    public void CrashGlass()
    {
        realWall.SetActive(false);
        glassWall.SetActive(true);

        rayFire.Demolish();
        bomb.Explode(0.3f);
    }
}
