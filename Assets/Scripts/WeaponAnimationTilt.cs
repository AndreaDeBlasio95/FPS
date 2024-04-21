using System.Collections;
using UnityEngine;

public class WeaponAnimationTilt : MonoBehaviour
{
    [SerializeField] private WeaponManager weaponManager;   // Reference to the WeaponManager

    private float recoilIntensity;
    private float recoilSpeed;
    private float recoilDuration;
    private Vector3 originalPosition;

    private Coroutine recoilCoroutine;                      // Reference to the recoil coroutine

    private void Start()
    {
        StartCoroutine(DelaySetUp(0.1f));                   // Always start with a delay setup
    }

    private IEnumerator DelaySetUp(float delay)
    {
        // Wait for the specified delay time or until weaponManager.currentWeapon is available
        float elapsedTime = 0f;
        while (weaponManager.currentWeapon == null && elapsedTime < delay)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        if (weaponManager.currentWeapon != null)
        {
            SetUp();                                        // Setup once the weapon is available
        }
        else
        {
            Debug.LogError("WeaponManager.currentWeapon is not set after delay.");
        }
    }

    public void SetUp()
    {
        recoilIntensity = weaponManager.currentWeapon.recoilIntensity;
        recoilSpeed = weaponManager.currentWeapon.recoilSpeed;
        recoilDuration = weaponManager.currentWeapon.recoilDuration;
        originalPosition = transform.localPosition;
    }

    public void RecoilAnimation()
    {
        if (recoilCoroutine != null)                        // Ensure any ongoing recoil animation is stopped
        {
            StopCoroutine(recoilCoroutine);
        }
        recoilCoroutine = StartCoroutine(RecoilGun());      // Start a new recoil animation
    }

    private IEnumerator RecoilGun()
    {
        Vector3 recoilPosition = originalPosition + Vector3.back * recoilIntensity; // Calculate recoil position
        float elapsedTime = 0;

        // Move the gun back quickly
        while (elapsedTime < recoilDuration)
        {
            transform.localPosition = Vector3.Lerp(originalPosition, recoilPosition, elapsedTime / recoilDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Return to original position smoothly
        transform.localPosition = recoilPosition;           // Set position explicitly before returning
        elapsedTime = 0;
        while (elapsedTime < recoilDuration)
        {
            transform.localPosition = Vector3.Lerp(recoilPosition, originalPosition, elapsedTime / (recoilDuration * recoilSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;         // Ensure it's exactly back to original
    }
}