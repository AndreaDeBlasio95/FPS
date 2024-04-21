using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private bool isAutomatic;                  // open the door automatically when the player's enter in the collider

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isAutomatic)
        {
            ToggleAnimationDoor(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isAutomatic)
        {
            ToggleAnimationDoor(false);
        }
    }

    public void ToggleAnimationDoor (bool _isOpen)
    {
        anim.SetBool("Open", _isOpen);
    }

}
