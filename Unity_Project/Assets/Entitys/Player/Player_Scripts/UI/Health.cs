using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health
{
    private List<Image> health;

    private List<Sprite> sprites;

    public int[] points = { 1, 1, 1, 1, 1, 1 };

    int hp;

    int maxHealthIndex = 2;

    int healthIndex;

    int lastPoint;

    public Health(int maxHealthIndex, List<Image> health, List<Sprite> sprites)
    {
        this.maxHealthIndex = maxHealthIndex;
        this.health = health;
        this.sprites = sprites;

        healthIndex = maxHealthIndex;

        Player.instance.health = 60;
        hp = 60;
    }

    public void TakeDamage()
    {
        hp = Mathf.Clamp(hp - 10, 0, 60);

        if (hp <= 0)
            Player.instance.alive = false;

        Player.instance.health = hp;

        int index = 2 * healthIndex + 1;


        if (points[index] != 0)
            points[index] = 0;
        else points[index - 1] = 0;


        changeHeart(points[index], points[index - 1] , healthIndex);

        //I evade a double check :P
        // if (points[index] == 0 && points[index - 1] == 0)
        if (points[index] + points[index - 1] == 0)
        { 
            healthIndex = Mathf.Clamp(healthIndex - 1, 0 , 2);
            lastPoint = 2 * healthIndex + 1;
        }
        else lastPoint = index;

    }

    public void HealHeart()
    {
        hp = Mathf.Clamp(hp + 10, 0, 60);

        Player.instance.health = hp;

        healthIndex = (points[lastPoint] != 0) ?  Mathf.Clamp(healthIndex + 1, 0, 2) : healthIndex;

        int index = 2 * healthIndex + 1;

        
        if (points[index - 1] != 1)
            points[index - 1] = 1;
        else points[index] = 1;

        changeHeart(points[index], points[index - 1], healthIndex);

        lastPoint = index;


    }

    private void changeHeart(int i1, int i2, int index)
    {
        int sum = i1 + i2;

        switch(sum)
        {
            case 2:
                health[index].sprite = sprites[2];
                break;
            case 1:
                health[index].sprite = sprites[1];
                break;
            case 0:
                health[index].sprite = sprites[0];
                break;
        }
    }
}
