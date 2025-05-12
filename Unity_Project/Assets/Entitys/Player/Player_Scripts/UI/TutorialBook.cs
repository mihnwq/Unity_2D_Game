using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

class TutorialBook : MonoBehaviour
{
    public GameObject openBook;
    public GameObject buttonBook;
    public void OnClick()
    {
        openBook.SetActive(!openBook.active);

    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))
            openBook.SetActive(false);
    }


}

