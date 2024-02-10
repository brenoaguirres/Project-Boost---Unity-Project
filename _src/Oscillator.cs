using UnityEngine;

public class Oscillator : MonoBehaviour
{
    Vector3 startingPosition;
    [SerializeField] Vector3 movementVector = new Vector3(0, -10, 0);
    [SerializeField][Range(0, 1)] float movementFactor;
    [SerializeField] float period = 2f;

    void Start() {
        startingPosition = transform.position;
    }

    void Update() {
        if (period <= Mathf.Epsilon) return; // NaN error fix
        
        float cycles = Time.time / period; // continually growing over time
        const float tau = Mathf.PI * 2; // constant value of 6.283
        float rawSinWave = Mathf.Sin(cycles * tau); // going from -1 to 1
        movementFactor = (rawSinWave + 1) / 2;
        // rawSinWave +1 so it never goes below 0 - itll range 0-2
        // / 2 so it never goes more than 1 - itll range 0-1

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }
}
