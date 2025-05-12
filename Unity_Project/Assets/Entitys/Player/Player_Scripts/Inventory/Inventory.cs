using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    Canvas invCanvas;

    [SerializeField]
    Item defaultItem;


    public List<Item> inventory = new List<Item>();

    public List<int> currentChangingIndex = new List<int>();

    public HashSet<string> type = new HashSet<string>();

    public List<string> typer = new List<string>();

    private static bool isActive = false;


    private void OnEnable()
    {
        isActive = true;

        while (InventoryList.NotEmpty())
            Add(InventoryList.RemoveFirst());
    }


    private void OnDisable()
    {
        isActive = false;
    }

    public static bool IsActive() => isActive;

    private  int currentIndex = 0;

    public static Inventory instance;

    private void Awake()
    {
        instance = this;
    }

   

    public void OnClose()
    {
       // GameStates.changeGameState();
        invCanvas.gameObject.SetActive(false);
    }

    public void OnOpen()
    {
       // GameStates.changeGameState();
        invCanvas.gameObject.SetActive(true);
    }

    public void Add(Item i)
    {

        if (!type.Contains(i.type))
        {
            if(currentChangingIndex.Count == 0)
            {
               
                i.index = inventory[currentIndex].index;
                
               type.Add(i.type);

                typer.Add(i.type);

                ReplaceItems(i, currentIndex);

                inventory[currentIndex].gameObject.SetActive(true);

                inventory[currentIndex].amount = 1;
                currentIndex++;
 
            }
            else
            {
                i.index = currentChangingIndex[0];

                inventory[i.index].amount = 1;

                ReplaceItems(i, i.index);

                type.Add(i.type);
                typer.Add(i.type);

                currentChangingIndex.RemoveAt(0);
            }
        }
        else inventory[i.index].amount++;

        
    }

    public void Remove(Item i)
    {
        int index = i.index;

      

        if (inventory[index].amount <= 1)
        {
            ReplaceItems(defaultItem, index);

            currentChangingIndex.Add(index);

            inventory[index].amount = 0;

            inventory[currentIndex].gameObject.SetActive(false);

            type.Remove(i.type);
            typer.Remove(i.type);
        }
        else inventory[index].amount--;


    }

    private void ReplaceItems(Item i1, int i)
    {
     
        Vector3 position;
        Quaternion rotation;
        Transform parentTransform = invCanvas.gameObject.transform;

        position = inventory[i].transform.position;
        rotation = inventory[i].transform.rotation;

      
        Destroy(inventory[i].gameObject);
       

       
        Item newInstance = Instantiate(i1, position, rotation, parentTransform);


        newInstance.index = i;
        newInstance.gameObject.name = i1.name + "_Slot_" + i;

        newInstance.gameObject.SetActive(true);

        inventory[i] = newInstance;
      
     
        
       
    }

    public void Swap(Item i1, Item i2)
    {
        Item aux = i1;

        Remove(i1);
    }
}
