using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform enemyTrans;
    private Rigidbody enemyRigidbody;

    [SerializeField] private float speed = 5.0f;

    private bool damageCooldownHasPassed = true;
    private float damageCooldown = 0.5f;

    [HideInInspector] public bool shouldFollowPlayer = true;

    private int enemyLives = 2;

    [HideInInspector] public Animator enemyAnim;

    [HideInInspector] public bool isDead = false;



    void Start()
    {
        enemyTrans = GetComponent<Transform>();
        enemyRigidbody = GetComponent<Rigidbody>();

        speed = (float)(GameManager.singleton.rand.NextDouble() * 4.0f) - 1.0f;

        speed = GameManager.singleton.rand.Next(1, 5);

        transform.rotation = Quaternion.Euler(0, 90, 0);

        enemyAnim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (shouldFollowPlayer)
        {
            followPlayer();
        }

    }

    void followPlayer()
    {
        float step = speed * Time.deltaTime;

        if (GameManager.singleton.player != null)
        {
            enemyRigidbody.MovePosition(Vector3.MoveTowards(enemyTrans.position, GameManager.singleton.player.transform.position, step));
        }
    }

    IEnumerator takeDamage()
    {
        damageCooldownHasPassed = false;

        enemyLives--;

        if (enemyLives <= 0)
        {

            isDead = true;
            GameManager.singleton.numberOfEnemies--;

            if (GameManager.singleton.numberOfEnemies <= 0)
            {
                if (GameManager.singleton.waveNumber % 2 == 0)
                {
                    GameManager.singleton.initialEnemies++;
                }
                GameManager.singleton.newWave(GameManager.singleton.initialEnemies);
            }

            shouldFollowPlayer = false;
            enemyAnim.SetBool("Walk", false);
            //enemyAnim.SetBool("Attack", false);
            enemyAnim.Play("RobotDeath");
            yield return new WaitForSeconds(2.5f);
            GameManager.singleton.robotDeathSound.Play();
            Destroy(this.gameObject);
        }

        GetComponent<SpriteRenderer>().color = Color.red;

        yield return new WaitForSeconds(damageCooldown);

        GetComponent<SpriteRenderer>().color = Color.white;

        damageCooldownHasPassed = true;
    }


    public void removeLife()
    {
        if (damageCooldownHasPassed)
        {
            StartCoroutine(takeDamage());
        }
    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log(col.gameObject.name);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Spaceship")
        {
            GetComponent<SpriteRenderer>().enabled = true; //Change to SpriteRenderer if using a sprite for enemies.
        }
    }
}
