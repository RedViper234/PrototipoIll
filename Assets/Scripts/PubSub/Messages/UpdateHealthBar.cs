using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct UpdateHealthBar : IMessage
{
    public string currentHealth;
    public string maxHealth;
    public float percentageBar;

    public UpdateHealthBar(string currentHealth, string maxHealth, float percentageBar)
    {
        this.currentHealth = currentHealth;
        this.maxHealth = maxHealth;
        this.percentageBar = percentageBar;
    }
}
