using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Ammo
{
    private List<Image> ammo = new List<Image>();

    private List<Sprite> ammoSprites = new List<Sprite>();

    private int maxAmmo;

    public Ammo(List<Image> ammo , List<Sprite> ammoSprites)
    {
        //Points to the ammo list we use.
        this.ammo = ammo;
        this.ammoSprites = ammoSprites;

        maxAmmo = PlayerStates.playerMaxAmmo;
    }

    public void OnAmmoGone()
    {
        int index = PlayerStates.GetPlayerAmmo() - 1;

        ammo[index].sprite = ammoSprites[0];
    }
    
    public void OnAmmoRegend()
    {
        int index = PlayerStates.GetPlayerAmmo() - 1;

        ammo[index].sprite = ammoSprites[1];
    }
}

