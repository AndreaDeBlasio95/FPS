using UnityEngine;

// In game Power Up that increases Shield to the Player
public class Shield : MonoBehaviour
{
    [SerializeField] private float value;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth _player = other.gameObject.GetComponent<PlayerHealth>();
            _player.RechargeShield(value);
            Destroy(gameObject);
        }
    }
}
