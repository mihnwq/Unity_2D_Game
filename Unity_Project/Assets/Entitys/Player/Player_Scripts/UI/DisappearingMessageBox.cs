using UnityEngine;
using UnityEngine.UI;

public class DisappearingMessageBox : MonoBehaviour
{
    public GameObject messageBox;  
    public Text messageText;       
    public float displayDuration = 3f;

    private float timer;
    private bool isShowing;

    void Start()
    {
        messageText.fontSize = 16; 
        messageText.resizeTextForBestFit = false;
        messageText.lineSpacing = 1;
        HideMessage();
    }

    void Update()
    {
        if (isShowing)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                HideMessage();
            }
        }
    }

    public void ShowMessage(string message)
    {
        messageText.text = message;
        messageBox.SetActive(true);
        timer = displayDuration;
        isShowing = true;
    }

    

    void HideMessage()
    {
        messageBox.SetActive(false);
        isShowing = false;
    }
}

