using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image Bar;

    public void UpdateHealthBar(float newValue)
    {
        Bar.fillAmount = newValue;
    }
}
