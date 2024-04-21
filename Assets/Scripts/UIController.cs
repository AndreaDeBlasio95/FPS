using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private WeaponManager weaponManager;

    [Header("Sliders")]
    [SerializeField] private Image healthSlider;
    [SerializeField] private Image shieldSlider;

    [Header("Texts")]
    [SerializeField] private TMP_Text tmpHealth;
    [SerializeField] private TMP_Text tmpShield;
    [SerializeField] private TMP_Text tmpCurrentAmmo;
    [SerializeField] private TMP_Text tmpMaxAmmo;

    [Header("Target Animation")]
    [SerializeField] private Animator animCanvasTarget;

    [Header("Player Take Damage UI")]
    [SerializeField] private Animator animPlayerTakeDamageUI;

    [Header("Game Over")]
    [SerializeField] private GameObject uiGameOver;
    [SerializeField] private GameObject uiGamePlay;

    private void Start()
    {
        healthSlider.fillAmount = 1;
        shieldSlider.fillAmount = 0;

        tmpHealth.text = "100";
        tmpShield.text = "0";

        LoadAmmo();
    }

    // The bool isDamage indicates if the player was hit by a bullet or a Power Up Heal
    public void UpdateHealth (float currentHealth, bool isDamage)
    {
        healthSlider.fillAmount = currentHealth / playerHealth.MaxHealth;
        tmpHealth.text = currentHealth.ToString();
        if (isDamage)
        {
            animPlayerTakeDamageUI.SetTrigger("Hit");
        }
    }

    public void UpdateShield(float currentShield)
    {
        shieldSlider.fillAmount = currentShield / playerHealth.MaxShield;
        tmpShield.text = currentShield.ToString();
    }

    public void UpdateAmmo()
    {
        tmpCurrentAmmo.text = weaponManager.currentWeapon.currentAmmo.ToString();
    }

    private IEnumerator DelayLoadingWeapon(float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadAmmo();
    }


    private void LoadAmmo()
    {
        if (weaponManager.currentWeapon)
        {
            tmpCurrentAmmo.text = weaponManager.currentWeapon.maxAmmo.ToString();
            tmpMaxAmmo.text = "/ " + weaponManager.currentWeapon.maxAmmo.ToString();
        }
        else
        {
            StartCoroutine(DelayLoadingWeapon(0.3f));
        }
    }

    public void AnimateCanvasTarget()
    {
        animCanvasTarget.SetTrigger("Shoot");
    }

    public void EnableGameOver()
    {
        uiGameOver.SetActive(true);
        uiGamePlay.SetActive(false);
    }

}
