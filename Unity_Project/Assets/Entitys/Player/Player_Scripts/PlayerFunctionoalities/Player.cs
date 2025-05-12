using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : Entity
{
    public List<Item> iiii = new List<Item>();

    public bool alive = true;

    public Item item1, item2, item3;

    public Inventory inv;

    public List<int> l = new List<int>();

   // [SerializeField]
    public List<Sprite> heart;

    public Health hp;

    public List<Image> playerHealth;

    public static Player instance;

    private void Awake()
    {
        instance = this;
    }

    //Reminder:
    //Use transfrom.up as transform.forward

    //The main player empty body.
    [SerializeField]
    Transform playerHusk;

    //The actual player object.
    public Transform playerObject;

    //The inputs of the player
    [SerializeField]
    public int horizontal;
    [SerializeField]
    public int vertical;

    //Speed variables.
    [SerializeField]
    float speed;
    private float originalSpeed;
    private Vector3 smoothVelocity = Vector3.zero;


    //All the rigidbodies and collider related stuff.
    public Rigidbody2D rb;
    public float relativeObjectMass = 4f;

    //All the abilities and mechanics.
    [SerializeField]
    DashAbility dash;

    //The player's rotation speed for when moving or when it has to attack.
    [SerializeField]
    float movingRotationSpeed = 4f;

    [SerializeField]
    float attackingRotationSpeed = 5f;

    public static bool playerAttacking;

    //Start is called once at the begining of the script.
    public void Start()
    {
        originalSpeed = speed;
        isAttacking = false;

        hp = new Health(2, playerHealth, heart);
    }

    //The update it's being called constantly every frame.
    public override void Update()
    {
        if(alive)
        {
            if (!dash.dashing)
            {
                PlayerMovement();
            }

            if (Input.GetKeyDown(KeyCode.I))
                InteractInventory();

            if (Input.GetKeyDown(KeyCode.Z))
                inv.Add(item1);

            if (Input.GetKeyDown(KeyCode.X))
                inv.Add(item2);

            if (Input.GetKeyDown(KeyCode.C))
                inv.Add(item3);

            if (Input.GetKeyDown(KeyCode.B))
                inv.Remove(item1);

            if (Input.GetKeyDown(KeyCode.N))
                inv.Remove(item2);

            if (Input.GetKeyDown(KeyCode.M))
                inv.Remove(item3);

            l = inv.currentChangingIndex;

            
            if (Input.GetKeyDown(KeyCode.L))
                hp.TakeDamage();

            iiii = InventoryList.itemList;
        }

        PlayerStates.GetLivelihood(alive);

        if(!alive)
        {
            InitiateGameOver();
        }
        
    }

    private void InteractInventory()
    {
        if (!Inventory.IsActive())
            inv.OnOpen();
        else inv.OnClose();
    }


    //Handeling the Player movement by keys.
    private void PlayerMovement()
    {
        horizontal = InputSystem.GetHorizontal();
        vertical = InputSystem.GetVertical();

        PlayerStates.getAxes(vertical, horizontal);

        Vector2 direction = new Vector2(horizontal, vertical).normalized;

        rb.linearVelocity = direction * speed;

    }

    public bool PlayerMoving()
    {
        return horizontal != 0 || vertical != 0;
    }

    public override void OnTakeDamage(int damage)
    {
        hp.TakeDamage();

        health -= damage;
    }

    private void InitiateGameOver()
    {
        StopPlayer();
        StopAllCoroutines();
        StaticManager.ResetAll();
        PlayerAnimations.instance.SetAllBackToDefaults();
        Invoke(nameof(ReloadCurrentScene), 1.1f);
    }

    public void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    private void StopPlayer()
    {
        rb.linearVelocity = Vector2.zero;
    }
}
