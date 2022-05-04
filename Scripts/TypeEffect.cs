using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeEffect : MonoBehaviour
{
    public int CharPerSeconds;
    public GameObject EndCursor;
    public bool isAnimating;

    string targetMsg;
    Text msgText;
    int index;
    float interval;

    void Awake()                                    //타이핑 오디오도 추가
    {
        msgText = GetComponent<Text>();
    }

    public void SetMsg(string msg)
    {
        if (isAnimating) {
            msgText.text = targetMsg;
            CancelInvoke();
            EffectEnd();
        }
        else {
            targetMsg = msg;
            EffectStart();
        }
    }
    void EffectStart()
    {
        msgText.text = "";
        index = 0;
        EndCursor.SetActive(false);
        isAnimating = true;

        interval = 1.0f / CharPerSeconds;
        Invoke("Effecting", interval);

    }

    void Effecting()
    {
        if(msgText.text == targetMsg)
        {
            EffectEnd();
            return;
        }

        msgText.text += targetMsg[index];
        index++;

        Invoke("Effecting", interval);
    }

    void EffectEnd()
    {
        isAnimating = false;
        EndCursor.SetActive(true);
    }
}
