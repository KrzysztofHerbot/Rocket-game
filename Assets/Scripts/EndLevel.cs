using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndLevel : MonoBehaviour
{
    [SerializeField] TimeShow ts;
    Canvas cv;

    // Start is called before the first frame update
    void Start()
    {
        cv = GetComponent<Canvas>();
        cv.enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ts.isEnd)
        {
            cv.enabled = true;
        }
    }
}
