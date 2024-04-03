using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Titan : MonoBehaviour
{
    public float titanhealth = 100.0f;
    [SerializeField] HealthBar healthBar;

    private void Start()
    {
        //StartCoroutine(SimulateDamage());
    }

    IEnumerator SimulateDamage()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            TakeDamage(5.0f);
        }
    }

    public void TakeDamage(float damage)
    {
        titanhealth -= damage;
        healthBar.UpdateHealthBar(titanhealth / 100.0f);

        if (titanhealth < 0)
            Debug.Log("GAME OVER");
    }
}
