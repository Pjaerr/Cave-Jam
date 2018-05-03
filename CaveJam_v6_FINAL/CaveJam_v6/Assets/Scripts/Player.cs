using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //External References
    private Transform playerTrans;
    private Rigidbody playerRigidbody;
    [SerializeField] private GameObject rock;

    private SpriteRenderer playerSpriteRenderer;

    private Animator anim;

    //Player Attributes
    [SerializeField] private float movementSpeed = 10.0f;
    public int playerLives = 5;
    [SerializeField] private float rockThrowSpeed = 20.0f;

    //Rock Throwing
    public int ammo = 6;
    private bool canThrowRock = true;
    [SerializeField] private float throwCooldown = 1.0f;


    private bool isDead = false;




    // Use this for initialization
    void Start()
    {
        playerTrans = GetComponent<Transform>();
        playerRigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        playerSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (!isDead)
        {


            playerMovement();

            if (ammo > 0)
            {
                if (canThrowRock)
                {
                    if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        StartCoroutine(throwRock("up"));
                    }
                    else if (Input.GetKey(KeyCode.UpArrow))
                    {
                        StartCoroutine(throwRock("left"));
                    }
                    else if (Input.GetKey(KeyCode.DownArrow))
                    {
                        StartCoroutine(throwRock("down"));
                    }
                    else if (Input.GetKey(KeyCode.RightArrow))
                    {
                        StartCoroutine(throwRock("right"));
                    }
                }
            }
            else
            {
                Debug.Log("Out of ammo");
                //SHOW OUT OF AMMO UI MESSAGE.
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator throwRock(string dir)
    {
        canThrowRock = false;

        if (dir == "up")
        {
            GameObject tempRock = Instantiate(rock, new Vector3(playerTrans.position.x, playerTrans.position.y, playerTrans.position.z + 1), Quaternion.identity);

            tempRock.GetComponent<Rock>().throwSelf(new Vector3(0, 0, rockThrowSpeed));

        }
        else if (dir == "left")
        {
            GameObject tempRock = Instantiate(rock, new Vector3(playerTrans.position.x + 1, playerTrans.position.y, playerTrans.position.z), Quaternion.identity);

            tempRock.GetComponent<Rock>().throwSelf(new Vector3(rockThrowSpeed, 0, 0));
        }
        else if (dir == "down")
        {
            GameObject tempRock = Instantiate(rock, new Vector3(playerTrans.position.x - 1, playerTrans.position.y, playerTrans.position.z), Quaternion.identity);

            tempRock.GetComponent<Rock>().throwSelf(new Vector3(-rockThrowSpeed, 0, 0));
        }
        if (dir == "right")
        {
            GameObject tempRock = Instantiate(rock, new Vector3(playerTrans.position.x, playerTrans.position.y, playerTrans.position.z - 1), Quaternion.identity);

            tempRock.GetComponent<Rock>().throwSelf(new Vector3(0, 0, -rockThrowSpeed));
        }


        ammo--;
        //Waits for the given cooldown time before allowing throwing again.
        yield return new WaitForSeconds(throwCooldown);
        anim.SetBool("Throw", false);
        canThrowRock = true;
    }

    public void playRockAnim()
    {
        anim.SetBool("Throw", true);
    }

    void playerMovement()
    {


        //Getting axis
        var x = Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed;
        var z = Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeed;


        float Animspeed = Input.GetAxis("Horizontal") + Input.GetAxis("Vertical");


        if (z > 0)
        {
            playerSpriteRenderer.flipX = false;
            anim.SetBool("WalkRight", true);

        }
        else if (z < 0)
        {
            playerSpriteRenderer.flipX = true;
            playerTrans.localScale.Set(-1, 1, 1);
            anim.SetBool("WalkRight", true);
        }
        else
        {
            playerSpriteRenderer.flipX = false;
            anim.SetBool("WalkRight", false);
            anim.SetBool("WalkLeft", false);
        }

        if (x != 0)
        {
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
        }


        /* if (x != 0 || z != 0)
         {
             anim.SetBool("Walk", true);
         }
         else
         {
             anim.SetBool("Walk", false);
         }*/


        //Moving the player
        playerRigidbody.MovePosition(new Vector3(playerRigidbody.position.x + x, playerRigidbody.position.y, playerRigidbody.position.z - z));
    }


    void OnCollisionStay(Collision col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            if (canBeAttacked)
            {
                StartCoroutine(resetInvincibility());



                int randomIndex = GameManager.singleton.rand.Next(0, GameManager.singleton.gruntSounds.Length);

                GameManager.singleton.gruntSounds[randomIndex].Play();


                col.gameObject.GetComponent<Animator>().SetBool("Attack", true);

                if (playerLives <= 0)
                {
                    StartCoroutine(playerDeath());
                }

                col.gameObject.GetComponent<Enemy>().shouldFollowPlayer = false;
            }
            else
            {
                Debug.Log("Player is Invincible");
            }
        }

        if (col.gameObject.tag == "Ammo")
        {

            if (Input.GetAxis("Interact") == 1)
            {

                if (col.gameObject.GetComponent<Ammo>().collectAmmo())
                {
                    ammo++;
                }
            }
        }
    }


    IEnumerator resetInvincibility()
    {
        canBeAttacked = false;
        playerLives--;
        yield return new WaitForSeconds(2.0f);
        canBeAttacked = true;
    }

    private bool canBeAttacked = true;

    void OnCollisionEnter(Collision col)
    {

    }

    IEnumerator playerDeath()
    {
        isDead = true;

        anim.SetBool("IsDead", true);

        //Play death anim
        GameManager.singleton.cavemanDeathSound.Play();
        yield return new WaitForSeconds(4);
        GameManager.singleton.endGame(GameManager.EndGameCondition.Loss);
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            if (!col.gameObject.GetComponent<Enemy>().isDead)
            {
                col.gameObject.GetComponent<Enemy>().shouldFollowPlayer = true;
            }

            //col.gameObject.GetComponent<Animator>().SetBool("Attack", false);

        }
    }







}



