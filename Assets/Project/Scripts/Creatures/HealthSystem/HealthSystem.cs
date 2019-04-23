using System;

public class HealthSystem
{
    public event EventHandler OnHealthChanged;

    private float currentHealth, maxHealth;
    private float currentEnergy, maxEnergy;
    private float currentOxigen, maxOxigen;

    public bool isDrowning;

    public HealthSystem(int health, int energy, int oxigen=10)
    {
        maxHealth = (float)health;
        currentHealth = (float)health;
        maxEnergy = (float)energy;
        currentEnergy = (float)energy;
        maxOxigen = (float)oxigen;
        currentOxigen = (float)oxigen;
        isDrowning = false;
    }

    // Directe toegang tot waarden
    public int GetHealth()
    {
        return (int)currentHealth;
    }
    public int GetEnergy()
    {
        return (int)currentEnergy;
    }
    public int GetOxigen()
    {
        return (int)currentOxigen;
    }
    public float GetHealthPerunage()
    {
        return currentHealth / maxHealth;
    }
    public float GetEnergyPerunage()
    {
        return currentEnergy / maxEnergy;
    }
    public float GetOxigenPerunage()
    {
        return currentOxigen / maxOxigen;
    }
    
    // wijzigingen met int-parameters
    public void DamageHealth(int damage)
    {
        currentHealth -= (float)damage;
        if (currentHealth < 0f) currentHealth = 0f;
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }
    public void HealHealth(int heal)
    {
        currentHealth += (float)heal;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }
    public void DamageEnergy(int damage)
    {
        currentEnergy -= (float)damage;
        if (currentEnergy < 0) currentEnergy = 0;
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }
    public void HealEnergy(int heal)
    {
        currentEnergy += (float)heal;
        if (currentEnergy > maxEnergy) currentEnergy = maxEnergy;
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }
    public void DrainOxigen(int oxigen)
    {
        currentOxigen -= (float)oxigen;
        if (currentOxigen < 0) 
        {
            currentOxigen = 0;
            // als geen oxigen meer, health en energy omlaag
            isDrowning = true;
        }
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }


    //en de float-versie voor berekende damage/heal
    public void DamageHealth(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0f) currentHealth = 0f;
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }
    public void HealHealth(float heal)
    {
        currentHealth += heal;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }
    public void DamageEnergy(float damage)
    {
        currentEnergy -= damage;
        if (currentEnergy < 0) currentEnergy = 0;
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }
    public void HealEnergy(float heal)
    {
        currentEnergy += heal;
        if (currentEnergy > maxEnergy) currentEnergy = maxEnergy;
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }
    public void DrainOxigen(float oxigen)
    {
        currentOxigen -= oxigen;
        if (currentOxigen < 0)
        {
            currentOxigen = 0;
            // als geen oxigen meer, health en energy omlaag
            isDrowning = true;
        }
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }

    // Oxigen helemaal herstellen.Dat gaat in één keer.
    public void RecoverOxigen()
    {
        isDrowning = false;
        currentOxigen = maxOxigen;
    }
}
