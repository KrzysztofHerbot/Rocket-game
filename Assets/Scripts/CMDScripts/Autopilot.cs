using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autopilot : MonoBehaviour
{
    //[SerializeField] float mainThrust = 100.0f;
    //[SerializeField] float rotationSpeed = 100.0f;
    [SerializeField] AudioClip mainEngineAudio;
    [SerializeField] public float fuel = 100f;
    [SerializeField] public const float maxfuel = 100f;
    [SerializeField] float ThrustBurnrate = 10;
    [SerializeField] float TurnBurnrate = 2;

    [SerializeField] ParticleSystem ThrustParticles;
    [SerializeField] ParticleSystem ThrustRightParticles;
    [SerializeField] ParticleSystem ThrustLeftParticles;

    List<FlyCommand> commands = new List<FlyCommand>();
    float startTime = 0f;

    Rigidbody rb;
    AudioSource rocketSound;
    [SerializeField] FuelBarCMD fb;

    public bool isActive = false;
    bool isCommandActive = false;

    void Start()
    {
        isActive = false;
        rb = GetComponent<Rigidbody>();
        rocketSound = GetComponent<AudioSource>();
        //commands.RemoveRange(0, commands.Count);
        commands.Add(new FlyCommand(1, 2f, 70f));
        commands.Add(new FlyCommand(3, 0.5f, 40f));
        commands.Add(new FlyCommand(1, 2f, 60f));
        commands.Add(new FlyCommand(2, 0.5f, 90f));
        commands.Add(new FlyCommand(1, 2f, 100f));
        //SetAutopilot();
    }

    void FixedUpdate()
    {
        if (commands.Count != 0)
        {
            RunAutopilot(commands);
        }

    }
   /* void SetAutopilot() //sets command list for the spaceship to carry out
    {
        
        
        
    }*/

    void RunAutopilot(List<FlyCommand> commandsList)  //carries out commands
    {
        //int listNumber;
        FlyCommand commands = commandsList[0];
        
        switch (commands.type)
        {
            case 1:
                StartThrusting(commands.timeUse, commands.strenght);
                Debug.Log("IM in case1");
                break;
            case 2:
                TurnLeft(commands.timeUse, commands.strenght);
                Debug.Log("IM in case2");
                break;
            case 3:
                TurnRight(commands.timeUse, commands.strenght);
                Debug.Log("IM in case3");
                break;
        }
    }
    void StartThrusting(float timeFly, float mainThrust)
    {
        if (!isCommandActive)
        {
            startTime = Time.time;
            isCommandActive = !isCommandActive;
        }
        else if (fuel > 0 && Time.time<startTime+timeFly)
        {
            LoseFuelThrust();

            if (!isActive)
            { isActive = true; }
            Debug.Log("Thrust " + Time.time);
            rb.AddRelativeForce(Vector3.up * Time.deltaTime * mainThrust);

            if (!rocketSound.isPlaying)
            {
                rocketSound.PlayOneShot(mainEngineAudio);
            }
            else if (!ThrustParticles.isPlaying)
            {
                ThrustParticles.Play();
            }
        }
        else
        {
            rocketSound.Stop();
            ThrustParticles.Stop();
            isCommandActive = false;
            if (commands.Count != 0)
            {
                commands.RemoveAt(0);
            }
        }
    }

    void TurnLeft(float timeFly, float rotationSpeed)
    {
        if (!isCommandActive)
        {
            Debug.Log("I'm in commandActive");
            startTime = Time.time;
            isCommandActive = !isCommandActive;
        }
        else if (fuel > 0 && Time.time < startTime + timeFly)
        {
            Debug.Log("I'm in elsefuel");
            LoseFuelThrust();
            ApplyRotation(-rotationSpeed);
            if (!isActive)
            { isActive = true; }
            //Debug.Log("TurnLeft "+Time.time);
            else if (!ThrustRightParticles.isPlaying)
            {
                ThrustRightParticles.Play();
                Debug.Log("I'm in rightparticles");
            }
        }
        else
        {
            Debug.Log("I'm in ELSE");
            ThrustLeftParticles.Stop();
            ThrustRightParticles.Stop();
            isCommandActive = false;
            if (commands.Count != 0)
            {
                commands.RemoveAt(0);
            }
        }
    }
    void TurnRight(float timeFly, float rotationSpeed)
    {
        if (!isCommandActive)
        {
            startTime = Time.time;
            isCommandActive = !isCommandActive;
        }
        else if (fuel > 0 && Time.time < startTime + timeFly)
        {
            LoseFuelThrust();
            ApplyRotation(rotationSpeed);
            if (!isActive)
            { isActive = true; }
            //Debug.Log("TurnRight " + Time.time);
            else if (!ThrustLeftParticles.isPlaying)
            {
                ThrustLeftParticles.Play();
            }
        }
        else
        {
            ThrustLeftParticles.Stop();
            ThrustRightParticles.Stop();
            isCommandActive = false;
            if (commands.Count != 0)
            {
                commands.RemoveAt(0);
            }
        }
    }

    void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; // freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * Time.deltaTime * rotationThisFrame);
        rb.freezeRotation = false; // unfreezing rotation so the physics take over
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

public class FlyCommand
{
    public int type; //type of command: 1-Thrust, 2-Turn Left, 3-Turn Right
    public float timeUse;
    public float strenght;

    public FlyCommand(int type, float timeUse, float strenght)
    {
        this.type = type;
        this.timeUse = timeUse;
        this.strenght = strenght;
    }

}
