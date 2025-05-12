using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Potion : Item
{
    private void Awake()
    {
        type = "Potion";
    }

    public override void Start()
    {
        base.Start();
    }

    public override void OnClick()
    {
        MessageHandler.instance.SendBoxMessage("Healed 1 point!");
        Player.instance.hp.HealHeart();
        base.OnClick();
    }


}

