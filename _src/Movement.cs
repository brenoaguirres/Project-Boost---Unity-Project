using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1000f;
    [SerializeField] private float rotationMoveSpeed = 100f;
    [SerializeField] private AudioClip mainEngine;
    [SerializeField] ParticleSystem mainEngineFire;
    [SerializeField] ParticleSystem leftEngineFire;
    [SerializeField] ParticleSystem rightEngineFire;

    private Rigidbody rb;
    private AudioSource audioSource;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust() {
        if (Input.GetKey(KeyCode.W))
        {
            Accelerate();
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Reverse();
        }
        else
        {
            DeactivateMainEngine();
        }
    }

    private void DeactivateMainEngine()
    {
        audioSource.Stop();
        mainEngineFire.Stop();
    }

    private void Reverse()
    {
        ApplyMovement(Vector3.down, -moveSpeed);
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(mainEngine);
        if (!mainEngineFire.isPlaying)
            mainEngineFire.Play();
    }

    private void Accelerate()
    {
        ApplyMovement(Vector3.up, moveSpeed);
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(mainEngine);
        if (!mainEngineFire.isPlaying)
            mainEngineFire.Play();
    }

    private void ApplyMovement(Vector3 vector, float speed)
    {
        rb.AddRelativeForce(vector * moveSpeed * Time.deltaTime);
    }

    private void ProcessRotation() {
        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
        else
        {
            DeactivateSideEngines();
        }
    }

    private void DeactivateSideEngines()
    {
        rightEngineFire.Stop();
        leftEngineFire.Stop();
    }

    private void RotateRight()
    {
        ApplyRotation(-rotationMoveSpeed);
        if (!leftEngineFire.isPlaying)
            leftEngineFire.Play();
    }

    private void RotateLeft()
    {
        ApplyRotation(rotationMoveSpeed); // z axis
        if (!rightEngineFire.isPlaying)
            rightEngineFire.Play();
    }

    private void ApplyRotation(float speed)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * speed * Time.deltaTime);
        rb.freezeRotation = false;
    }
}
