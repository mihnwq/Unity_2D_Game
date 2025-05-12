using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class InventoryList
{
        public static List<Item> itemList = new List<Item>();

        public static Item RemoveFirst()
        {
            Item first = itemList[0];

            itemList.RemoveAt(0);

            return first;
        }

        public static void Add(Item i)
        {
            itemList.Add(i);
        }

        public static bool NotEmpty()
        {
           return itemList.Count > 0;
        }
}


