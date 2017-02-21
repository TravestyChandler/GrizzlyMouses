using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {


	public KeyCode JumpButton;
    public KeyCode TravelButton;
    public float jumpDelay = 0.25f;
    public float travelDelay = 0.25f;
    public float groundedDist = 0.5f;
    public float distToOtherTime = 10f;
    public bool canJump;
    public bool inPresent = true;
    public bool canTravel = true;
    public Rigidbody2D rb;
    public float jumpVelocity;
    public LayerMask groundLayers;
    public PhotonView photonView;
	// Use this for initialization
	void Start () {
        rb = this.GetComponent<Rigidbody2D>();
        photonView = PhotonView.Get(this);
	}
	
	// Update is called once per frame
	void Update () {
        if (GameManager.instance.phase == GameManager.GamePhase.Running && IsGrounded() && canJump) {
            if (GameManager.instance.networkedGame && PhotonNetwork.playerName == "1")
            {
                if (Input.GetKeyDown(JumpButton))
                {
                    photonView.RPC("Jump", PhotonTargets.All);
                }
            }
            else if(!GameManager.instance.networkedGame){
                if (Input.GetKeyDown(JumpButton))
                {
                    Jump();
                }
            }
		}
        if(GameManager.instance.phase == GameManager.GamePhase.Running && canTravel)
        {
            if (GameManager.instance.networkedGame && PhotonNetwork.playerName == "2")
            {

                if (Input.GetKeyDown(TravelButton))
                {
                    photonView.RPC("TimeTravel", PhotonTargets.All);
                }
            }
            else if (!GameManager.instance.networkedGame)
            {
                if (Input.GetKeyDown(TravelButton))
                {
                    TimeTravel();
                }
            }
        }

    }

    [PunRPC]
	public void Jump(){
        rb.velocity += new Vector2(0f, jumpVelocity);
        canJump = false;
        Debug.Log("jumping");
        StartCoroutine(JumpRoutine());
	}

    public IEnumerator JumpRoutine()
    {
        float timer = 0f;
        while(timer < jumpDelay)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        canJump = true;
    }

	public bool IsGrounded(){
        Ray2D ray2D = new Ray2D(transform.position, Vector2.down);
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, Vector2.down, groundedDist,groundLayers);
        if (hit2D.collider != null && hit2D.collider.name != this.name)
        {
            return true;
        }
        else
        {
            return false;
        }
		//Ray ray = new Ray (transform.position, Vector2.down);
  //      Debug.DrawRay(transform.position, Vector2.down);
		//RaycastHit hit;
		//if (Physics.Raycast (ray, out hit, groundedDist) ){
  //          Debug.Log("grounded");
		//	return true;
		//} else {
		//	return false;
		//}
	}


    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("obstacle"))
        {
            if (GameManager.instance.networkedGame)
            {
                photonView.RPC("HitObstacle", PhotonTargets.All);
            }
            else {
                HitObstacle();
            }
        }
        else if (col.tag.Equals("deathbarrier"))
        {
            if (GameManager.instance.networkedGame)
            {
                photonView.RPC("Death", PhotonTargets.All);
            }
            else {
                Death();
            }
        }
    }

    [PunRPC]
    public void Death()
    {
        rb.gravityScale = 0f;
        rb.velocity = Vector3.zero;
        GameManager.instance.PlayerDeath();
        
    }

    [PunRPC]
    public void HitObstacle()
    {
        
    }

    [PunRPC]
    public void TimeTravel()
    {
        if (inPresent)
        {
            Vector3 newPos = this.transform.position;
            newPos.y -= distToOtherTime;
            Collider[] cols = Physics.OverlapSphere(newPos, 1f);
            if (cols.Length > 0)
            {
                Debug.Log("Can't Travel with an obstacle in the way");
            }
            else
            {
                inPresent = !inPresent;
                canTravel = false;
                Debug.Log("Traveling to other time");

                this.transform.position = newPos;
            }
        }
        else
        {
            Vector3 newPos = this.transform.position;
            newPos.y += distToOtherTime;
            Collider[] cols = Physics.OverlapSphere(newPos, 1f);
            if (cols.Length > 0)
            {
                Debug.Log("Can't Travel with an obstacle in the way");
            }
            else
            {
                Debug.Log("Traveling to other time");
                inPresent = !inPresent;
                canTravel = false;
                this.transform.position = newPos;
            }
        }
        if (!canTravel)
        {
            StartCoroutine(TravelRoutine());
        }
    }

    public IEnumerator TravelRoutine()
    {
        float timer = 0f;
        while (timer < travelDelay)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        canTravel = true;
    }
}
