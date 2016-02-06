using UnityEngine;
using System.Collections;

public class SethController : MonoBehaviour {

    bool isFloating = true;

    public float moveSpeed;
    Transform currentPoint;
    public Transform[] points;
    public GameObject[] arrowShooters;
    public GameObject[] stoneSpawners;
    public int moveToPoint = 1;
    public float ShokedTime;
    public float minSpawnTime = 0.5f;
    public float maxSpawnTime = 0.8f;
    public float attackTime = 3;
    public ParticleSystem deathParticle;
    public GameObject ChargeStones;
    public GameObject ChargeArrows;
    public GameObject BurstStones;
    public GameObject BurstArrows;


    Rigidbody2D rb;
	// Use this for initialization
	void Start () {
        currentPoint = points[moveToPoint];
        rb = gameObject.GetComponent<Rigidbody2D>();
        Attack();

	}
	
	// Update is called once per frame
	void Update () {
        if (isFloating)
        {
            Float();
        }
        
	
	}

    void Float()
    {
        rb.gravityScale = 0;
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position,
                                                            currentPoint.position,
                                                            Time.deltaTime * moveSpeed);

        
        if (Vector2.Distance(gameObject.transform.position,currentPoint.position) < 0.001)
        {
            moveToPoint++;
            if (moveToPoint == points.Length)
            {
                moveToPoint = 0;
            }
            currentPoint = points[moveToPoint];
        }
      
    }

    void StartFloating()
    {
        isFloating = true;
        Attack();
    }

    void FallDown()
    {
        rb.gravityScale = 10;
        isFloating = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.gameObject.GetComponent<KillPlayer>()!= null)
        {
            FallDown();
            Invoke("StartFloating", ShokedTime);
        }
        if (col.gameObject.tag == "SethKiller")
        {
            SethDeath();
        }
    }

    void Attack()
    {
        Invoke("AttackArrow", attackTime);
    }
    void AttackArrow()
    {
        foreach (GameObject stone in stoneSpawners) //Stops Stones
        {
            stone.GetComponent<FallingStones>().Reset();
        }
        if (isFloating)
        {
            Instantiate(BurstArrows, gameObject.transform.position, gameObject.transform.rotation);
            foreach (GameObject arrow in arrowShooters) //go arrows
            {
                arrow.GetComponent<ArrowShoter>().minSpawnTime = minSpawnTime;
                arrow.GetComponent<ArrowShoter>().maxSpawnTime = maxSpawnTime;
            }
            Invoke("AttackStones", attackTime);
            Instantiate(ChargeStones, gameObject.transform.position, gameObject.transform.rotation);
        }

        

    }

    void AttackStones()
    {
        foreach (GameObject arrow in arrowShooters)
        {
            arrow.GetComponent<ArrowShoter>().Reset();
        }
        if (isFloating)
        {
            Instantiate(BurstStones, gameObject.transform.position, gameObject.transform.rotation);
            foreach (GameObject stone in stoneSpawners)
            {
                stone.GetComponent<FallingStones>().minSpawnTime = minSpawnTime;
                stone.GetComponent<FallingStones>().maxSpawnTime = maxSpawnTime;

            }
            Invoke("AttackArrow", attackTime);
            Instantiate(ChargeArrows, gameObject.transform.position, gameObject.transform.rotation);
        }

    }
    public void SethDeath()
    {
        Instantiate(deathParticle, gameObject.transform.position, gameObject.transform.rotation);
        // exiting the two characters
        FindObjectOfType<LevelManager>().ExitBothPlayer();
        //yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
