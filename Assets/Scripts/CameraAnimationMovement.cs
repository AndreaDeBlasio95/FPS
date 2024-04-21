using UnityEngine;

public class CameraAnimationMovement : MonoBehaviour
{
    [SerializeField] private float bobbingSpeed = 0.05f;       // How fast the bobbing occurs
    [SerializeField] private float bobbingAmount = 0.15f;       // The amount of bob up and down movement

    private float defaultYPos = 1.0f;
    private float timer = 0;

    void Start()
    {
        defaultYPos = 1.0f;
    }

    private void OnEnable()
    {
        defaultYPos = 1.0f;
    }

    private void OnDisable()
    {
        // Reset the camera position when the player is not moving
        timer = 0;
        transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, defaultYPos, Time.deltaTime * bobbingSpeed), transform.localPosition.z);
    }

    void Update()
    {
        // Calculate the new Y position using a sine wave for smooth bobbing
        float waveslice = Mathf.Sin(timer);
        timer += bobbingSpeed;
        if (timer > Mathf.PI * 2)
        {
            timer -= (Mathf.PI * 2);
        }

        float adjustY = waveslice * bobbingAmount;
        transform.localPosition = new Vector3(transform.localPosition.x, defaultYPos + adjustY, transform.localPosition.z);
    }
}