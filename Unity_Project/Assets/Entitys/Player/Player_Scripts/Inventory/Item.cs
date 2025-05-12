using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    PickUpItem thisItem;

    public int amount;

    public int index;

    public string type;

    public virtual void OnClick()
    {
        Inventory.instance.Remove(this);
    }

    public void OnDrop()
    {
        thisItem.Drop();

        amount = 0;

        Inventory.instance.Remove(this);


    }

    public virtual void SetImediatePowerUp()
    {

    }

    public virtual void Start()
    {


        if (type.Length < 1)
            gameObject.SetActive(false);
        else gameObject.SetActive(true);
    }

    public void Update()
    {
        
    }


}

