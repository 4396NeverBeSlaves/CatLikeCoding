using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSText : MonoBehaviour
{
    public enum DisplayType { FPS, MS }
    [SerializeField] DisplayType displayType = DisplayType.FPS;
    [SerializeField] TextMeshProUGUI fpsText;
    [SerializeField,Range(0.1f,2f)] float SamplingDuration = 1f;


    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    int framesCounter = 0;
    float timeCounter = 0f;
    float maxDuration = 0f, minDuration = float.MaxValue;
    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.unscaledDeltaTime;
        timeCounter += deltaTime;
        framesCounter += 1;

        if (deltaTime > maxDuration)
        {
            maxDuration = deltaTime;
        }
        if (deltaTime < minDuration)
        {
            minDuration = deltaTime;
        }

        if (timeCounter >= SamplingDuration)
        {
            if(displayType == DisplayType.FPS)
            {
                fpsText.SetText("FPS\nAvg:{0:00.0}\nMax:{1:00.0}\nMin:{2:00.0}", framesCounter / timeCounter, 1f / minDuration, 1f / maxDuration);
            }
            else if (displayType == DisplayType.MS)
            {
                fpsText.SetText("MS\nAvg:{0:00.0}\nMax:{1:00.0}\nMin:{2:00.0}", timeCounter/framesCounter * 1000f, maxDuration * 1000f, minDuration * 1000f);
            }
            framesCounter = 0;
            timeCounter = 0f;
            maxDuration = 0;
            minDuration = float.MaxValue;
        }


        
    }
}
