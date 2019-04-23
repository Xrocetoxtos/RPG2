using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxigenBar : MonoBehaviour
{
    private HealthSystem healthSystem;

    public void Setup(HealthSystem healthSystem)
    {
        this.healthSystem = healthSystem;
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
    }

    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e)
    {
        transform.Find("OxigenBarPivot").localScale = new Vector3(healthSystem.GetOxigenPerunage(), 1);
    }
}