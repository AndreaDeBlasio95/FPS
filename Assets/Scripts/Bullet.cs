using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 20f;         // Speed at which the bullet moves
    [SerializeField] private float maxLifetime = 3f;    // Maximum time before the bullet gets deactivated
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float damage;
    [SerializeField] private GameObject canvasHit;      // Prefab to instantiate on hitting a target

    public string ShooterTag { get; set; }              // Tag of the shooter

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetDamage (float weaponDamage)
    {
        damage = weaponDamage;                          // Set the damage based on weapon settings
    }
    private void FixedUpdate()
    {
        if (rb)
        {
            rb.velocity = transform.forward * speed;    // Continuously move the bullet forward
        }
    }

    private void OnEnable()
    {
        gameObject.transform.SetParent(null);           // Ensure the bullet is not a child of the shooter
        rb.velocity = transform.forward * speed;        // Set the bullet's velocity in the forward direction
        StartCoroutine(DelayDeactivateBullet());        // Start a coroutine to deactivate the bullet after its lifetime
    }

    private IEnumerator DelayDeactivateBullet()
    {
        yield return new WaitForSeconds(maxLifetime);
        DeactivateBullet();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Enemy") && ShooterTag!="Enemy")
        {
            Vector3 positionOffset = transform.position + transform.forward * -2.0f;    // Adjust the position slightly back
            Quaternion cameraRotation = Camera.main.transform.rotation;                 // Get the main camera's rotation

            Instantiate(canvasHit, positionOffset, cameraRotation);                     // Instantiate the canvasHit prefab facing the camera

            // Damage System
            EnemyHealth _enemy = col.gameObject.GetComponent<EnemyHealth>();
            _enemy.TakeDamage(damage);

            StopCoroutine(DelayDeactivateBullet());
            DeactivateBullet();                                                         // Deactivate bullet on collision
        }
        if (col.gameObject.CompareTag("Player") && ShooterTag != "Player")
        {
            // Damage System
            PlayerHealth _player = col.gameObject.GetComponent<PlayerHealth>();
            _player.TakeDamage(damage);

            StopCoroutine(DelayDeactivateBullet());
            DeactivateBullet();                                                         // Deactivate bullet on collision
        }
        if (col.gameObject.CompareTag("Wall") || col.gameObject.CompareTag("Ground"))
        {
            StopCoroutine(DelayDeactivateBullet());
            DeactivateBullet();                                                         // Deactivate bullet on collision
        }
    }

    private void DeactivateBullet()
    {
        BulletPool.Instance.ReturnBullet(gameObject);
    }
}
