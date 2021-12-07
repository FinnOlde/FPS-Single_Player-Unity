using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public PlayerHpUiDisplay HPDisplay;
    private float hp = 100;
    public float HP
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
            HPDisplay.SetHP(hp);
        }
    }

}
