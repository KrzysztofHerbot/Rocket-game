using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    [SerializeField] float mainThrust = 100.0f;
    [SerializeField] float rotationSpeed = 100.0f;
    [SerializeField] AudioClip mainEngineAudio;
    [SerializeField] public float fuel = 100f;
    [SerializeField] public const float maxfuel = 100f;
    [SerializeField] float ThrustBurnrate = 10;
    [SerializeField] float TurnBurnrate = 2;

    [SerializeField] ParticleSystem ThrustParticles;
    [SerializeField] ParticleSystem ThrustRightParticles;
    [SerializeField] ParticleSystem ThrustLeftParticles;

    Rigidbody rb;
    AudioSource rocketSound;
    [SerializeField] FuelBar fb;

    public bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
        rb = GetComponent<Rigidbody>();
        rocketSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
        ResetLevel();
    }

    void ProcessThrust()
    {
        StartThrusting();
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A) && fuel > 0)
        {
            TurnLeft();
            LoseFuelTurn();
            if(!isActive)
            { isActive = true; }
        }

        else if (Input.GetKey(KeyCode.D) && fuel > 0)
        {
            TurnRight();
            LoseFuelTurn();
            if (!isActive)
            { isActive = true; }
        }
        else
        {
            ThrustLeftParticles.Stop();
            ThrustRightParticles.Stop();
        }
    }
   
    void StartThrusting()
    {
        if (Input.GetKey(KeyCode.Space) && fuel >0)
        {
            LoseFuelThrust();
            if (!isActive)
            { isActive = true; }

            rb.AddRelativeForce(Vector3.up * Time.deltaTime * mainThrust);
            if (!rocketSound.isPlaying)
            {
                rocketSound.PlayOneShot(mainEngineAudio);
            }
            if (!ThrustParticles.isPlaying)
            {
                ThrustParticles.Play();
            }
            Debug.Log("Space is pressed");
        }
        else
        {
            rocketSound.Stop();
            ThrustParticles.Stop();
        }
    }


    void TurnRight()
    {
        ApplyRotation(-rotationSpeed);
        if (!ThrustLeftParticles.isPlaying)
        {
            ThrustLeftParticles.Play();
        }
        Debug.Log("D is pressed");
    }

    void TurnLeft()
    {
        ApplyRotation(rotationSpeed);
        if (!ThrustRightParticles.isPlaying)
        {
            ThrustRightParticles.Play();
        }
        Debug.Log("A is pressed");
    }

    void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; // freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * Time.deltaTime * rotationThisFrame);
        rb.freezeRotation = false; // unfreezing rotation so the physics take over
    }
    void ResetLevel()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadLevel();
        }
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoseFuelThrust()
    {
        float currentTime = Time.deltaTime;
        fuel = fuel - currentTime * ThrustBurnrate;
        float fuelfraction = fuel / maxfuel;
        fb.UpdateFuel(fuelfraction);
    }
    void LoseFuelTurn()
    {
        float currentTime = Time.deltaTime;
        fuel = fuel - currentTime * TurnBurnrate;
        float fuelfraction = fuel / maxfuel;
        fb.UpdateFuel(fuelfraction);
        Debug.Log(fuel);
    }
}
