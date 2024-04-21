using UnityEngine;

public abstract class CharacterHealth : MonoBehaviour
{
    [Header("Scriptable Object Reference")]
    public HealthData healthDataTemplate;           // Used as a template to create local copies

    private HealthData healthData;                  // Local instance copy of the HealthData

    [Header("Stats")]
    [SerializeField] private float maxHealth;       // Maximum health capacity
    [SerializeField] private float currentHealth;   // Current health
    [SerializeField] private float maxShield;       // Maximum shield capacity
    [SerializeField] private float currentShield;   // Current shield

    // Getters & Setters ---
    public float MaxHealth
    {
        get { return maxHealth; }
        protected set { maxHealth = value; }
    }

    public float CurrentHealth
    {
        get { return currentHealth; }
        protected set { currentHealth = value; }
    }

    public float MaxShield
    {
        get { return maxShield; }
        protected set { maxShield = value; }
    }

    public float CurrentShield
    {
        get { return currentShield; }
        protected set { currentShield = value; }
    }

    // Boolean to check if the character can currently take damage
    public bool CanTakeDamage { get; protected set; } = true;


    // On Awake, create a local copy of the HealthData from the template
    protected virtual void Awake()
    {
        CreateLocalHealthDataInstance();
    }

    // Creates a local instance of HealthData from the ScriptableObject template
    private void CreateLocalHealthDataInstance()
    {
        healthData = Instantiate(healthDataTemplate); // Create a deep copy
        MaxHealth = healthData.maxHealth;
        CurrentHealth = healthData.currentHealth;
        MaxShield = healthData.maxShield;
        CurrentShield = healthData.currentShield;
    }

    // Method to handle taking damage
    public virtual void TakeDamage(float damage)
    {
        if (!CanTakeDamage) return;

        float damageToApply = damage;

        // Apply damage to shield first if present
        if (currentShield > 0)
        {
            ApplyShieldDamage(ref damageToApply);
        }

        ApplyHealthDamage(damageToApply);           // Any remaining damage is applied to health
        CheckDeath();                               // Check if character should be considered dead
    }

    // Applies damage to the shield, reducing it before health
    private void ApplyShieldDamage(ref float damage)
    {
        if (damage > currentShield)
        {
            damage -= currentShield;                // calculate the remaining damage to apply to the health
            currentShield = 0;
        }
        else
        {
            currentShield -= damage;
            damage = 0;                             // No health damage needed if all taken by shield        
        }
    }

    // Applies damage directly to health
    private void ApplyHealthDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;                      // Clamp health to zero
        }
    }

    // Abstract method to define how death is checked and handled
    public abstract void CheckDeath();

    // Method to heal the character
    public virtual void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;              // Do not exceed maximum health
        }
    }

    // Method to recharge the shield
    public virtual void RechargeShield(float amount)
    {
        currentShield += amount;
        if (currentShield > maxShield)
        {
            currentShield = maxShield;              // Do not exceed maximum shield
        }
    }
}