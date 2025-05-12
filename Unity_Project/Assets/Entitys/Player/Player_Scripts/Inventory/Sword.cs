using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Sword : Item
{

    private void Awake()
    {
        type = "Sword";
    }

    public override void Start()
    {
        
        base.Start();
    }

    public override void OnClick()
    {
        MessageHandler.instance.SendBoxMessage("Sword already equiped!");
        //Nothing to do here
    }

    public override void SetImediatePowerUp()
    {
        PlayerStates.aquiredSword = true;
    }

}

