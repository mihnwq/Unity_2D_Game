using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class StaticManager
{
    public static void ResetAll()
    {
        InventoryList.itemList.Clear();
        GameStates.ChangeGameStateTo(true);
        PlayerStates.aquiredSword = false;

        if(Inventory.IsActive())
        Inventory.instance.OnClose();
    }

}

