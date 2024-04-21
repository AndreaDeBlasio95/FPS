using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyColliderDetector : MonoBehaviour
{
    [SerializeField] private EnemyAIMovement enemyAIMovement;

    [Header("Fetch Player's position")]
    [SerializeField] private Transform playerPosition;

    [Header("Manage Shooting")]
    [SerializeField] private bool canShoot;
    [SerializeField] private Transform spawnPositionBullet;

    [Header("Shooter's Tag")]
    [SerializeField] private string shooterTag;

    // Start is called before the first frame update
    void Start()
    {
        canShoot = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canShoot = true;
            playerPosition = other.gameObject.transform;
            enemyAIMovement.SetPlayerTargetPosition(playerPosition);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && canShoot)
        {
            playerPosition = other.gameObject.transform;
            enemyAIMovement.SetPlayerTargetPosition(playerPosition);

            // Shoot
            canShoot = false;
            StartCoroutine(DelayedShoot());
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") )
        {
            playerPosition = null;
            canShoot = false;
            enemyAIMovement.SetPlayerTargetPosition(playerPosition);
        }
    }


    private IEnumerator DelayedShoot()
    {
        yield return new WaitForSeconds(enemyAIMovement.currentWeapon.fireRate);
        SpawnBullet();
        canShoot = true;
    }

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
            _bullet.transform.localRotation = Quaternion.identity;              // Reset rotation if needed
            _bullet.transform.rotation = spawnPositionBullet.rotation;          // Align bullet's rotation with the spawn point

            // Set damage
            Bullet _b = _bullet.GetComponent<Bullet>();
            _b.SetDamage(enemyAIMovement.currentWeapon.damage);
            _b.ShooterTag = shooterTag;

            _bullet.SetActive(true);
        }
    }
}
