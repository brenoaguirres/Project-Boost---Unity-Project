using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private float levelLoadDelay = 2f;
    [SerializeField] AudioClip spaceshipCrash;
    [SerializeField] AudioClip finishedLevel;
    [SerializeField] ParticleSystem finishedLevelParticles;
    [SerializeField] ParticleSystem spaceshipCrashParticles;
    

    AudioSource audioSource;

    bool isTransitioning = false;
    bool canDie = true;

    void Awake() {
        audioSource = GetComponent<AudioSource>();        
    }

    void Update() {
        RespondToDebugKeys();
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L)) {
            LoadNextLevel();
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            canDie = !canDie;
            Debug.Log("Player can die: " + canDie);
        }
    }

    private void OnCollisionEnter(Collision other) {

        if (isTransitioning) {return;}

        switch(other.gameObject.tag) {
            case "Friendly":
                Debug.Log("You've hit a friendly object!");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            case "Fuel":
                Debug.Log("Fuel obtained!");
                break;
            default:
                if(canDie)
                    StartCrashSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        isTransitioning = true;
        GetComponent<Movement>().enabled = false;
        audioSource.Stop();
        audioSource.PlayOneShot(finishedLevel);
        finishedLevelParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void StartCrashSequence() {
        isTransitioning = true;
        GetComponent<Movement>().enabled = false;
        audioSource.Stop();
        audioSource.PlayOneShot(spaceshipCrash);
        spaceshipCrashParticles.Play();
        Invoke("ReloadLevel", levelLoadDelay);
    }

    private void ReloadLevel() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void LoadNextLevel() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
            nextSceneIndex = 0;
        SceneManager.LoadScene(nextSceneIndex);
    }
}
