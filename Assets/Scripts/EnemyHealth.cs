using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inherits from CharacterHealth, tailored specifically for enemy characters
public class EnemyHealth : CharacterHealth
{
    // Reference to the Level script to notify about enemy death
    [SerializeField] private Level level;

    // Override the CheckDeath method to add enemy-specific death handling
    public override void CheckDeath()
    {
        if (CurrentHealth <= 0)
        {
            CanTakeDamage = false;          // Prevent further damage after death
            level.EnemyKilled();            // Notify the level that an enemy has been killed
            Destroy(gameObject);            // Remove the enemy from the game
        }
    }
}