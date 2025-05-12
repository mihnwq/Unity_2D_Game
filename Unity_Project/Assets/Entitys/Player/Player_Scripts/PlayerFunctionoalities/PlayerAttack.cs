using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    List<Image> ammoUIimages = new List<Image>();

    [SerializeField]
    List<Sprite> ammoUIsprites = new List<Sprite>();

    Ammo ammoUI;

    //Player trnasform:
    [SerializeField]
    Transform playerObject;

    [SerializeField]
    GameObject throwableObject;

    [SerializeField]
    float throwForce;

    public Trigonometry trigo;

    public float attackDuration = 0.3f;

    public float attackCooldown = 1.5f;

    public float attackCooldownTimer = 0;

    public KeyCode attackKey;

    public KeyCode throwKey;

    public bool isAttackingcheck;

    public List<GameObject> hitboxes;

    private int hitbox;

    [SerializeField]
    int maxAmmo;

    public int currentAmmo;

    [SerializeField]
    private float replenishDelay = 3f; // How long to wait after firing before regen starts

    [SerializeField]
    private float regenerationInterval = 1f; // How fast to add +1 ammo

    private float replenishTimer;
    private float regenTimer;
    private bool isRegenerating;


    public void Start()
    {
        trigo = new Trigonometry();
        PlayerStates.setWatchDirection("left");
        ReplenishAmmo();

        ammoUI = new Ammo(ammoUIimages, ammoUIsprites);
    }

    private void ReplenishAmmo()
    {
        currentAmmo = maxAmmo;
        PlayerStates.SetAmmountAmunition(currentAmmo);
    }

    private void UseAmmo()
    {
        if (currentAmmo > 0)
        {
            ammoUI.OnAmmoGone();
            currentAmmo = Mathf.Clamp(currentAmmo - 1, 0, maxAmmo);
            PlayerStates.SetAmmountAmunition(currentAmmo);
            replenishTimer = 0f;
            isRegenerating = false;
        }

        
    }

    private void RegenerateAmmo()
    {
        currentAmmo = Mathf.Clamp(currentAmmo + 1, 0, maxAmmo);
        PlayerStates.SetAmmountAmunition(currentAmmo);

        ammoUI.OnAmmoRegend();
    }

    [SerializeField]
    float maxReplenishDuration;

    private float currentReplenishDuration;

    private void RegenerateAmmoOverTime()
    {
        if (currentAmmo < maxAmmo)
        {
            replenishTimer += Time.deltaTime;

            if (!isRegenerating && replenishTimer >= replenishDelay)
            {
                isRegenerating = true;
                regenTimer = regenerationInterval; 
            }

            if (isRegenerating)
            {
                regenTimer += Time.deltaTime;

                if (regenTimer >= regenerationInterval)
                {
                    regenTimer = 0f;
                    RegenerateAmmo();
                    if (currentAmmo >= maxAmmo)
                    {
                        isRegenerating = false;
                        replenishTimer = 0f;
                    }
                }

            }
        }
        else
        {
            isRegenerating = false;
            replenishTimer = 0f;
        }

    }

    private bool CanShoot()
    {
        return currentAmmo >= 1;
    }

    private void setAttackDuration(float attackDuration)
    {
        this.attackDuration = attackDuration;
    }

    private void setAttackCooldown(float attackCooldown)
    {
        this.attackCooldown = attackCooldown;
    }

    private Vector2 directionToMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 watchDirection = new Vector2(mousePos.x - playerObject.position.x, mousePos.y - playerObject.position.y).normalized;

        return watchDirection;
    }

    private float getMouseAngle()
    {
        Vector2 mouse = directionToMouse();

        float angle = trigo.getAngle(mouse.x, mouse.y);

        return angle;

    }

    private void GetAttackDirection()
    {
        float angle = getMouseAngle();

        

        if (angle >= 45 && angle <= 135)
            PlayerStates.setWatchDirection("up");
        else if (angle >= 135 && angle <= 225)
            PlayerStates.setWatchDirection("left");
        else if (angle >= 225 && angle <= 315)
            PlayerStates.setWatchDirection("down");
        else PlayerStates.setWatchDirection("right");
    }

    private void Attack()
    {
        if (attackCooldownTimer > 0)
        {
            return;
        }
        else
        {
            attackCooldownTimer = attackCooldown;
            GetAttackDirection();
            hitbox = GetHitbox();
            hitboxes[hitbox].SetActive(true);
        }

        isAttackingcheck = true;

        Invoke(nameof(resetAttack), attackDuration);
    }

    private void resetAttack()
    {
        isAttackingcheck = false;
        hitboxes[hitbox].SetActive(false);
    }
    private int GetHitbox()
    {
        return PlayerStates.getWatchDirection();
    }

    private void UpdateMeleAttack()
    {
        if (Input.GetKeyDown(attackKey)) Attack();

        if (attackCooldownTimer > 0)
            attackCooldownTimer -= Time.deltaTime;

        PlayerStates.setAttack(isAttackingcheck);
    }    

    public void Update()
    {
        if(!PlayerStates.hadDied)
        {
            if (PlayerStates.CanUseSword() && !InputBlocker.IsPointerOverUI())
                UpdateMeleAttack();

            UpdateRangedAttack();
        }

    }

    public float holdTime = 0f;
    private bool isHolding = false;

    [SerializeField]
    float maxHoldTime;

    private void UpdateRangedAttack()
    {

        if(CanShoot())
        {
            HandleShootingInput();
        }
       

        RegenerateAmmoOverTime();
    }

    private void HandleShootingInput()
    {
        if (Input.GetKeyDown(throwKey))
        {
            holdTime = 0;
        }

        if (Input.GetKey(throwKey))
            holdTime += Time.deltaTime;

        if (Input.GetKeyUp(throwKey))
        {
            UseAmmo();

            float straightness = Mathf.Clamp(Mathf.Clamp01(holdTime / maxHoldTime), 0.18f, 0.35f);

            shoot(straightness);
        }
    }

    ///shoot(..) is being called ONLY once i press the ,,shootingKey"
    private void shoot(float straightness)
    {
        ///Direction of the mouse in corespondence to the world.
           Vector2 dir = directionToMouse();

            Vector3 position = new Vector3(playerObject.position.x, playerObject.position.y, 0);

            GameObject projectile = Instantiate(throwableObject, position, playerObject.rotation);

            //So the sprite faces oppsoite of the direction it has been shoot.
            projectile.transform.right = dir;

            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

           

          ///straightness is a value between 0 and 1 and it evals how fast does it go
            rb.linearVelocity = dir * getThrowForce(throwForce , rb) * straightness;
        
    }

    public float getThrowForce(float force, Rigidbody2D rb)
    {
        //Acceleration.
        return force / rb.mass;
    }


}

