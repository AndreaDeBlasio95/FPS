using UnityEngine;

// In game Power Up that Heal the Player
public class Heal : MonoBehaviour
{
    [SerializeField] private float value;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth _player = other.gameObject.GetComponent<PlayerHealth>();
            _player.Heal(value);
            Destroy(gameObject);
        }
    }
}
