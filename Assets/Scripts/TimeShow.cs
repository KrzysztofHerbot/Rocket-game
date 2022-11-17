using UnityEngine;
using UnityEngine.UI;

public class TimeShow : MonoBehaviour
{
    Text tm;
    float timeStart;
    [SerializeField] Movement mv;
    [SerializeField] CollisionHandler ch;

    bool isStarted = false;
    public bool isEnd = false;


    // Start is called before the first frame update
    void Start()
    {
        tm = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
            if (mv.isActive && !isStarted) //checks if player started moving
            {
                timeStart = Time.time;   //sets time when player started moving
                isStarted = true;
            }
     
        if (mv.isActive && !ch.isChanging)
        {
            tm.text = (Time.time - timeStart).ToString();
        }
        else if (mv.isActive && ch.isWon)
        {
            isEnd = true;
        }
    }
}
