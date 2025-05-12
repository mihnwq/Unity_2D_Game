using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    Sprite sprite;

    private float currentLifeTime;

    public float maxLifeTime;

    private float throwForce;

    [SerializeField]
    string self;

    [SerializeField]
    bool canPierce;

    public void getForce(float throwForce)
    {
        this.throwForce = throwForce;
    }

    private void Start()
    {
        currentLifeTime = maxLifeTime;
    }

 

    private void FixedUpdate()
    {
        if (currentLifeTime > 0)
            currentLifeTime -= Time.deltaTime;
        else Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(!canPierce && other.tag != self)
        Destroy(gameObject);
    }

   
}
