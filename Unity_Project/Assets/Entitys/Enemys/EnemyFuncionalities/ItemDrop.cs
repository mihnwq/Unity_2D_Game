using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{

    [SerializeField]
    Entity currentEntity;

    [SerializeField]
    Item dropingItem;

    public void FixedUpdate()
    {
        if (currentEntity.hasDied)
            InventoryList.Add(dropingItem);
    }


}

