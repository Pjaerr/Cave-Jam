using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    private Rigidbody rockRigidbody;

    private Vector3 m_whereToApplyForce;

    // Use this for initialization
    void Start()
    {
        rockRigidbody = GetComponent<Rigidbody>();
        GetComponent<MeshRenderer>().enabled = false;
        rockRigidbody.useGravity = false;


    }

    public void throwSelf(Vector3 whereToApplyForce)
    {

        m_whereToApplyForce = whereToApplyForce;

        GameManager.singleton.throwGruntSound.Play();

        StartCoroutine(throwAnimation());
    }

    IEnumerator throwAnimation()
    {
        GameManager.singleton.player.GetComponent<Player>().playRockAnim();
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(move());
    }

    IEnumerator move()
    {
        GetComponent<MeshRenderer>().enabled = true;
        rockRigidbody.useGravity = true;
        rockRigidbody.AddForce(m_whereToApplyForce, ForceMode.Impulse);
        yield return new WaitForSeconds(0.8f);
        Destroy(this.gameObject);
    }


    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<Enemy>().removeLife();
            Destroy(this.gameObject);
        }

        // if (col.gameObject.tag == "LevelColliders")
        // {
        //     Destroy(this.gameObject);
        // }
    }
}
