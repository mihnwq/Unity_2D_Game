using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Entity : MonoBehaviour
{

    public int health;

    public int attack;

    public int defense;

    public bool isAttacking;

    public bool hasDied
    {
        get
        {
            return health <= 0;
        }
        set
        {
           hasDied = value;
        }
    }

    public virtual void OnTakeDamage(int damage)
    {

    }

    public virtual void Start()
    {
        
    }

    public virtual void Update()
    {
     
    }

}

