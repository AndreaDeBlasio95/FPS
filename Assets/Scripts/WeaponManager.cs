using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInputHandler playerInputHandler;
    [SerializeField] private UIController healthUIController;
    [SerializeField] private Transform weaponContainer;
    [SerializeField] private WeaponAnimationTilt weaponAnimationTilt;
    [SerializeField] private Animator anim;
    [SerializeField] private Transform spawnPositionBullet;
    [SerializeField] private ParticleSystem fxSpawnBullet;
    [SerializeField] private Weapon weaponTemplate;             // Original weapon template as a ScriptableObject

    public Weapon currentWeapon;                               // Runtime clone of the weapon


    [Header("Aim")]
    [SerializeField] private CameraAnimationManager cameraAnimationManager; // Reference to Camera Animation Manager

    private bool isFiring = false;
    private bool isReloading = false;
    private bool isAiming = false;
    [SerializeField] private bool fixAiming = false;

    private Coroutine fireCoroutine = null;

    [Header("Shooter Tag")]
    [SerializeField] private string shooterTag;

    void Awake()
    {
        InitializeWeapon();
    }

    private void InitializeWeapon()
    {
        // Clone the weapon ScriptableObject to avoid altering the original
        currentWeapon = InstantiateWeapon(weaponTemplate);

        GameObject weaponPrefab = Instantiate(currentWeapon.weaponPrefab, weaponContainer);
        weaponPrefab.transform.localPosition = Vector3.zero;
        weaponPrefab.transform.localRotation = Quaternion.identity;

        // Set current ammo to max ammo based on the cloned weapon
        currentWeapon.currentAmmo = currentWeapon.maxAmmo;
    }

    private Weapon InstantiateWeapon(Weapon original)
    {
        Weapon clone = ScriptableObject.CreateInstance<Weapon>();

        // Copy the properties from the original to the new clone
        clone.weaponPrefab = original.weaponPrefab;
        clone.damage = original.damage;
        clone.range = original.range;
        clone.fireRate = original.fireRate;
        clone.maxAmmo = original.maxAmmo;
        clone.currentAmmo = original.maxAmmo; // Start with full ammo
        clone.reloadTime = original.reloadTime;
        clone.recoilIntensity = original.recoilIntensity;
        clone.recoilSpeed = original.recoilSpeed;
        clone.recoilDuration = original.recoilDuration;

        return clone;
    }

    private void Update()
    {
        // Shoot
        if (playerInputHandler.ShootTriggered && !isFiring)
        {
            Fire();
        }
        if (!playerInputHandler.ShootTriggered && isFiring)
        {
            DisableFire();
        }
        // Aim
        if (playerInputHandler.AimTriggered && !isAiming && !fixAiming)
        {
            Aim();
        }
        if (playerInputHandler.AimTriggered && isAiming && fixAiming)
        {
            DisableAim();
        }
    }

    public void Fire()
    {
        if (fireCoroutine == null && currentWeapon.currentAmmo > 0 && !isReloading)
        {
            isFiring = true;
            fireCoroutine = StartCoroutine(FireCoroutine());
        }
    }
    public void DisableFire()
    {
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
            fireCoroutine = null;
        }
        isFiring = false;
    }

    private IEnumerator FireCoroutine()
    {
        // this is useful to auto fire while holding the button down
        while (isFiring && currentWeapon.currentAmmo > 0 && !isReloading)
        {
            fxSpawnBullet.Play();
            SpawnBullet();
            currentWeapon.currentAmmo--;
            weaponAnimationTilt.RecoilAnimation();          // recoil animation
            healthUIController.UpdateAmmo();                // update the ui
            healthUIController.AnimateCanvasTarget();       // animate the target UI in the canvas
            if (currentWeapon.currentAmmo <= 0)
            {
                Reload();
            }
            yield return new WaitForSeconds(currentWeapon.fireRate);
        }
        isFiring = false;
        fireCoroutine = null;  // Clear the coroutine reference
    }


    /// <summary>
    /// Spawn Bullet at the SpawnPosition as the Parent in order to display the bullet shooted by the gun
    /// </summary>
    private void SpawnBullet()
    {
        GameObject _bullet = BulletPool.Instance.GetBullet();
        if (_bullet != null)
        {
            _bullet.transform.SetParent(spawnPositionBullet);

            // Reset local position and scale
            _bullet.transform.localPosition = Vector3.zero;
            _bullet.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

            // Set rotation to match the spawn position's rotation
            _bullet.transform.localRotation = Quaternion.identity;  // Reset rotation if needed
            _bullet.transform.rotation = spawnPositionBullet.rotation;  // Align bullet's rotation with the spawn point

            // Set damage
            Bullet _b = _bullet.GetComponent<Bullet>();
            _b.SetDamage(currentWeapon.damage);
            _b.ShooterTag = shooterTag;

            _bullet.SetActive(true);
        }
    }

    public void Reload()
    {
        if (!isReloading && currentWeapon.currentAmmo < currentWeapon.maxAmmo)
        {
            isFiring = false;
            isReloading = true;
            StartCoroutine(ReloadCoroutine());
        }
    }

    private IEnumerator ReloadCoroutine()
    {
        // Implement reloading logic here
        anim.SetTrigger("Reload");
        yield return new WaitForSeconds(currentWeapon.reloadTime);
        currentWeapon.currentAmmo = currentWeapon.maxAmmo;
        isReloading = false;
        healthUIController.UpdateAmmo();                // update the ui
    }

    // Aiming
    private void Aim()
    {
        isAiming = true;
        anim.SetBool("Aim", isAiming);
        // animate the camera FieldView
        cameraAnimationManager.AimAnimation(isAiming);
        // add a delay between aim and not aim
        StartCoroutine(FixAim(0.3f));
    }
    private void DisableAim()
    {
        isAiming = false;
        anim.SetBool("Aim", isAiming);
        // animate the camera FieldView
        cameraAnimationManager.AimAnimation(isAiming);
        // add a delay between aim and not aim
        StartCoroutine(FixAim(0.3f));
    }
    // To avoid to continuos switch between true and false
    IEnumerator FixAim(float delay)
    {
        yield return new WaitForSeconds(delay);
        fixAiming = !fixAiming;
    }
    // -------

}