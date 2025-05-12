using UnityEngine;

public class SlimeBehaivior : Entity
{
    string[] orderList = { "Direct", "Area", "PurePursuit" };

    public Astar AI;

    public Transform target;

    EnemyAgro agro;

    [SerializeField]
    float distance;

    [SerializeField]
    LayerMask solid,player;

    [SerializeField]
    Rigidbody2D rb;

    EnemyMovement movemet;

    [SerializeField]
    float smoothTime = 0.25f;

    [SerializeField]
    float speed;

    private void Awake()
    {
        AI.SetNewOrderCommand(orderList[0]);

        AI.SetDummyTarget(target);
    }

    public void Start()
    {
        health = 100;

        AI.SetStartAndEndTargets(transform, target);

        agro = new EnemyAgro(distance,solid,player);

        movemet = new EnemyMovement(rb, transform, speed, smoothTime);

        AI.SetMovementObject(movemet);
    }

    public bool wasAgroed = false;

    public override void Update()
    {
        if(!PlayerStates.hadDied)
        {
            base.Update();

            HandleAI();
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }



    }

    private void HandleAI()
    {
        isAttacking = canAttack();

        wasAgroed = isAgro();

        if (!seenHim)
            seenHim = wasAgroed;

        if (isAgro())
        {

            gotToPlayer();
        }
        else if (seenHim)
        {
            findPlayer();
        }

        //  gotToPlayer();
    }

    private bool shouldResetPath = false;

    private Vector2 lastPlayerPosition;

    private Transform pathLastTarget;

    private void gotToPlayer()
    {
        AI.SetNewOrderCommand(orderList[0]);
        pathLastTarget = target;
        AI.AstarUpdate();
    }

    private void findPlayer()
    {

        if (!AI.goalReached)
        {
            AI.SetNewOrderCommand(orderList[2]);
            AI.SetDummyTarget(pathLastTarget);
            AI.AstarUpdate();
        }
        else AI.FollowPath(transform , AI.lastShorthestPath);

       


    }

    public bool seenHim = false;

    private bool isAgro()
    {
        agro.setOrigin(new Vector2(transform.position.x, transform.position.y));

        agro.setDirection(new Vector2(target.position.x - transform.position.x, target.position.y - transform.position.y));

        return agro.CanSeePlayer();
    }

    [SerializeField]
    float attackDistance;

    private bool canAttack()
    {
        return movemet.nearObject(attackDistance, target.position);
    }

    public override void OnTakeDamage(int damage)
    {
        health = Mathf.Clamp(health - damage, 0, int.MaxValue);
    }

}
