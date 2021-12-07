using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpUiDisplay : MonoBehaviour
{
    public float MaxHP = 100;
    public RectTransform HealthCrossInfill;
    public Text HealthText;

    public void SetHP(float currentHP){
        HealthCrossInfill.localScale = new Vector3(1, Mathf.Lerp(0, 1, Mathf.InverseLerp(0, 100, currentHP)), 1);
        HealthText.text = Mathf.CeilToInt(currentHP).ToString();
    }
}
