using UnityEngine;

[CreateAssetMenu(fileName = "HealthData", menuName = "Character/Health")]
public class HealthData : ScriptableObject
{
    public float maxHealth = 100;
    public float currentHealth = 100;
    public float maxShield = 100;
    public float currentShield = 0;

}
