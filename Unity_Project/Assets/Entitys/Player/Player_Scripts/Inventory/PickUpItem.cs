using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class PickUpItem : MonoBehaviour
{
   
    public Item item;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (Inventory.IsActive())
                Inventory.instance.Add(item);
            else InventoryList.Add(item);

            item.SetImediatePowerUp();

            MessageHandler.instance.SendBoxMessage("You got a " + item.type);

            Destroy(gameObject);
        }
    }

    public void Drop()
    {
        Instantiate(this, Player.instance.playerObject.position + Vector3.right, Player.instance.playerObject.rotation);
    }



}

