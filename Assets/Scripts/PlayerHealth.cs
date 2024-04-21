using UnityEngine;

// Inherits from CharacterHealth, tailored specifically for the player character
public class PlayerHealth : CharacterHealth
{
    // Reference to the UIController to update health and shield visuals
    [SerializeField] private UIController uiController;

    // Override the TakeDamage method to include UI updates
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);                            // Call the base class method to handle damage calculation

        if (CurrentHealth > 0)
        {
            uiController.UpdateShield(CurrentShield);           // Update shield value in the UI
            uiController.UpdateHealth(CurrentHealth, true);     // Update health in the UI with damage indication
        }
        
    }

    // Override the Heal method to update the health bar in the UI when healed
    public override void Heal(float health)
    {
        base.Heal(health);                                  // Heal the player using the base class method
        uiController.UpdateHealth(CurrentHealth, false);    // Update the health bar in the UI without damage indication
    }

    // Override the RechargeShield method to update the shield bar in the UI
    public override void RechargeShield(float shield)
    {
        base.RechargeShield(shield);                        // Recharge shield using the base class method
        uiController.UpdateShield(CurrentShield);           // Update the shield value in the UI
    }

    // Override the CheckDeath method to implement game-specific death behavior
    public override void CheckDeath()
    {
        if (CurrentHealth <= 0)
        {
            CanTakeDamage = false;                          // Prevent further damage if player is dead

            gameObject.tag = "Untagged";
            // Trigger game over sequence via the UI
            uiController.EnableGameOver();
        }
    }

}
