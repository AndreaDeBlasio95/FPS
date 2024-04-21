using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName;
    public GameObject weaponPrefab;
    public int damage;
    public float range;
    public float fireRate;
    public int maxAmmo;
    public int currentAmmo;
    public float reloadTime;
    // animation
    public float recoilIntensity;
    public float recoilSpeed;
    public float recoilDuration;
}