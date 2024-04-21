using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] private float timerToDestroy;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timerToDestroy);
    }
}

