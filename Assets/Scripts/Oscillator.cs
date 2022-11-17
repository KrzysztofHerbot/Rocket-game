using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{

    Vector3 startingPosition;
    [SerializeField] Vector3 movementVector;
    [SerializeField] [Range(0,1)] float movementFactor = 0f;
    [SerializeField] float period = 2f;
    

    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) { return; }

        const float tau = Mathf.PI * 2;    // constant of 6.28
        float cycles = Time.time / period;   // continually growing over time
        float rawSinWave = Mathf.Sin(cycles/tau);  // going from -1 to 1
        movementFactor = (rawSinWave + 1f) / 2f;   // adjusted for 0 to 1

        Vector3 Offset = movementVector * movementFactor;
        transform.position = startingPosition + Offset;
    }
}
