using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class MessageHandler : MonoBehaviour
{
    public static MessageHandler instance;

    [SerializeField]
    DisappearingMessageBox messageBox;

    private void Awake()
    {
        instance = this;   
    }

    public void Update()
    {

    }

    public void SendBoxMessage(string message)
    {
        messageBox.ShowMessage(message);
    }

}

