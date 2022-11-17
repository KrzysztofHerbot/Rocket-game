using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float reloadDelay = 0.5f;
    [SerializeField] AudioClip Win;
    [SerializeField] AudioClip Lose;

    AudioSource SoundOnCollision;
    [SerializeField] ParticleSystem WinParticles;
    [SerializeField] ParticleSystem LoseParticles;
    [SerializeField] float FuelCollected = 50;
    Movement mv;

    
    public bool isChanging = false;
    public bool isWon = false;
    bool collisionDisable = false;

    void Start()
    {
        GetSound();
        SoundOnCollision.Stop();
        mv = GetComponent<Movement>();
    }

    void Update()
    {
        ProcessDebug();
    }

    private void OnCollisionEnter(Collision other)
    {

        if (isChanging || collisionDisable)
        {
            return;
        }
        

            switch (other.gameObject.tag)
            {
                case "Friendly":
                    Debug.Log("Friendly");
                    break;
                case "Finish":
                    StartSuccess();
                    Debug.Log("Finish");
                    break;
                case "Fuel":
                    Debug.Log("Fuel");
                    break;
                default:

                    StartCrash();
                    Debug.Log("Untagged");
                    break;
            }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Fuel")
        {
            if (mv.fuel + FuelCollected < Movement.maxfuel)
            {
                mv.fuel = mv.fuel + FuelCollected;
            }
            else mv.fuel = Movement.maxfuel;
        }
        Destroy(other.gameObject);
    }

void StartCrash()
    {
        isChanging = true;

        SoundOnCollision.Stop();
        SoundOnCollision.PlayOneShot(Lose);
        LoseParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", reloadDelay);

        
    }

    void StartSuccess()
    {
        isChanging = true;
        isWon = true;
        SoundOnCollision.Stop();
        SoundOnCollision.PlayOneShot(Win);
        WinParticles.Play();
        GetComponent<Movement>().enabled = false;
        //Invoke("LoadNextLevel", reloadDelay+3.0f); // it loaded next level after X amount of time, is useless after UI element included
        

        
    }

    void ProcessDebug()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisable = !collisionDisable;  //toogles disable
        }
        else if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            GoPreviousLevel();
        }
        else if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            //LoadNextLevel();
        }

    }
    void GoPreviousLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex - 1;
        if (nextSceneIndex < 0)
        {
            nextSceneIndex = SceneManager.sceneCountInBuildSettings - 1;
        }

        SceneManager.LoadScene(nextSceneIndex);
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
  /*  void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        SceneManager.LoadScene(nextSceneIndex);
    }*/

    void GetSound()
    {
        SoundOnCollision = GetComponent<AudioSource>();
    }
}
