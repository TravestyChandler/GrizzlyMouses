using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public static PlayerController Instance;

    public enum PlayerStatus
    {
        Normal,
        SuperJump,
        Invincibility,
        SuperSpeed
    }

	public KeyCode JumpButton;
    public KeyCode TravelButton;
    public float jumpDelay = 0.25f;
    public float travelDelay = 0.25f;
    public float groundedDist = 0.5f;
    public float distToOtherTime = 10f;
	public bool canJump = true;
	public bool isGrounded = true;
    public bool inPresent = true;
    public bool canTravel = true;
    public Rigidbody2D rb;
    public float jumpVelocity;
    public float superJumpVelocity;
	public float gravityScale;
    public LayerMask groundLayers;
    public PhotonView photonView;
    public bool isDamaged = false;
    public float DamageTimer = 1f;
    public SpriteRenderer sp;
	public Transform circleCastLocation;
	public float circleCastRadius = 0.5f;
    public PlayerStatus state = PlayerStatus.Normal;

	// Use this for initialization
	void Start () {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
        rb = this.GetComponent<Rigidbody2D>();
        sp = this.GetComponent<SpriteRenderer>();
		gravityScale = rb.gravityScale;
        photonView = PhotonView.Get(this);
	}
	
	// Update is called once per frame
	void Update () {
		//circleoverlap for canJump boolean
		isGrounded = IsGrounded();
		if (GameManager.instance.phase == GameManager.GamePhase.Running && isGrounded && canJump) {
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
        if(PhotonNetwork.playerName == "2")
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if(GameManager.instance.resourcesCollected >= 5)
                {
                    GameManager.instance.photView.RPC("UsePowerUp", PhotonTargets.All, 1);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if(GameManager.instance.resourcesCollected >= 10)
                {
                    GameManager.instance.photView.RPC("UsePowerUp", PhotonTargets.All, 2);

                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if(GameManager.instance.resourcesCollected >= 15)
                {
                    GameManager.instance.photView.RPC("UsePowerUp", PhotonTargets.All, 3);

                }
            }
        }
    }

    
    public void UsePowerUp(int type)
    {
        Debug.Log("Use Power Up val " + type);
        if(state != PlayerStatus.Normal)
        {
            TurnOffPowerUps();
        }
        if (type == 1)
        {
            //Super jump
            Debug.Log("activating power up Jump");
            state = PlayerController.PlayerStatus.SuperJump;
            GameManager.instance.resourcesCollected -= 5;
            sp.color = Color.red;
        }
        if (type == 2)
        {
            //Invincibility
            Debug.Log("activating power up invincibility");
            state = PlayerController.PlayerStatus.Invincibility;
            rb.gravityScale = 0;
            GameManager.instance.resourcesCollected -= 10;
            sp.color = Color.yellow;
        }
        if (type == 3)
        {
            //Super Speed
            Debug.Log("activating power up superspeed");
            state = PlayerController.PlayerStatus.SuperSpeed;
            GameManager.instance.CurrentSpeed = GameManager.instance.SuperSpeed;
            GameManager.instance.resourcesCollected -= 15;
            sp.color = Color.blue;
        }
        if (PhotonNetwork.isMasterClient)
        {
            GameManager.instance.PowerUpTimer();
        }
        else
        {
            UIController.Instance.ResourcesText.text = "x" + GameManager.instance.resourcesCollected;
        }
    }

    [PunRPC]
    public void TurnOffPowerUps()
    {
        sp.color = Color.white;
        if(state == PlayerStatus.SuperJump)
        {

        }
        if(state == PlayerStatus.Invincibility)
        {
            rb.gravityScale = gravityScale;
        }
        if(state == PlayerStatus.SuperSpeed)
        {
            GameManager.instance.CurrentSpeed = GameManager.instance.maxSpeed;
        }
        state = PlayerStatus.Normal;
    }

    [PunRPC]
	public void Jump(){
        if(state == PlayerStatus.SuperJump)
        {
            rb.velocity = new Vector2(0f, superJumpVelocity);
        }
        else
        {
            rb.velocity = new Vector2(0f, jumpVelocity);
        }
        canJump = false;
        //Debug.Log("jumping");
        StartCoroutine(JumpRoutine());
        SoundManager.Instance.PlaySFX("Jump", 100);
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
		Collider2D col = Physics2D.OverlapCircle(circleCastLocation.position, circleCastRadius, groundLayers);
        //Ray2D ray2D = new Ray2D(transform.position, Vector2.down);
        //RaycastHit2D hit2D = Physics2D.Raycast(transform.position, Vector2.down, groundedDist,groundLayers);
		if (col != null)
        {
            return true;
        }
        else
        {
            return false;
        }
	}


    public void OnTriggerEnter2D(Collider2D col)
    {
        
         if (col.tag.Equals("deathbarrier"))
        {
            if (GameManager.instance.networkedGame)
            {
                photonView.RPC("Death", PhotonTargets.All);
            }
            else {
                Death();
            }
        }
		if (col.tag.Equals ("resource")) {
			//Debug.Log ("Hit Resource");
			if (PhotonNetwork.isMasterClient) {
				Collectible collect = col.GetComponent<Collectible> ();
                if (collect != null)
                {
                    if (collect.canCollect)
                    {
                        collect.phot.RPC("Collected", PhotonTargets.All);
                        Debug.Log("collecting resource");
                        GameManager.instance.photView.RPC("IncreaseResources", PhotonTargets.All);
                    }
                }
			}
		}
    }
		

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag.Equals("obstacle"))
        {
			//Compare y values, if hit point more than 75% height of other object
			//Use grounded position to compare with player's 'feet'
			float obstacleCompareValue = col.collider.bounds.center.y + (col.collider.bounds.extents.y * 0.5f);
			float playerY = circleCastLocation.position.y;
			//Debug.Log ("Player Y = " + playerY + "   Obstacle Y = " + obstacleCompareValue);
			if (playerY < obstacleCompareValue) {
				if (GameManager.instance.networkedGame) {
					if (PhotonNetwork.isMasterClient) {
						col.collider.GetComponent<PhotonView> ().RPC ("DestroyObstacle", PhotonTargets.All);
						photonView.RPC ("HitObstacle", PhotonTargets.All);
					}
				} else {
					NonNetworkHitObstacle ();
				}
			} else {
				if (PhotonNetwork.isMasterClient) {
					//Manually place player above obstacle
					float obstacleMaxY = col.collider.bounds.center.y + col.collider.bounds.extents.y;
					photonView.RPC ("SetOntoObstacle", PhotonTargets.All, obstacleMaxY);
				}
			}
        }
    }

	[PunRPC]
	public void SetOntoObstacle(float obstacleMaxY){
		float moveDistance = GetComponent<BoxCollider2D>().bounds.size.y/2f;
		float newY = obstacleMaxY + moveDistance;
		//Debug.Log ("ObstacleMaxY = " + obstacleMaxY + "  OldY = " + this.transform.position.y + " New Y = " + newY + "    moveDistance = " + moveDistance);
		this.transform.position = new Vector2 (this.transform.position.x, newY);
	}

    [PunRPC]
    public void Death()
    {
        rb.gravityScale = 0f;
        rb.velocity = Vector3.zero;
        GameManager.instance.PlayerDeath();
        
    }

	public void NonNetworkHitObstacle()
	{
		StartCoroutine(DamageRoutine());
	}
    [PunRPC]
	public void HitObstacle()
    {
		
        StartCoroutine(DamageRoutine());
    }

    public IEnumerator DamageRoutine()
    {
        Debug.Log("Character Damaged");
        GameManager.instance.photView.RPC("ShiftPlayerBack", PhotonTargets.All);
        float totalTimer = 0f;
        while (totalTimer < DamageTimer)
        {
            float timer = 0f;
            float flashTime = 0.5f;
            while (timer < flashTime)
            {
                timer += Time.deltaTime;
                totalTimer += Time.deltaTime;
                float colorVal = Mathf.Lerp(1f, 0.25f, timer / flashTime);
                sp.color = new Color(1f, colorVal, colorVal);
                yield return null;
            }
            timer = 0f;
            while(timer < flashTime)
            {
                timer += Time.deltaTime;
                totalTimer += Time.deltaTime;
                float colorVal = Mathf.Lerp(0.25f, 1f, timer / flashTime);
                sp.color = new Color(1f, colorVal, colorVal);
                yield return null;
            }
        }
        sp.color = Color.white;
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
				if (PhotonNetwork.isMasterClient) {
					CameraController.Instance.SwapCameras (inPresent);
				}
                SoundManager.Instance.PlaySFX("TimeTravel", 100);
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
				if (PhotonNetwork.isMasterClient) {
					CameraController.Instance.SwapCameras (inPresent);
				}
                SoundManager.Instance.PlaySFX("TimeTravel", 100);
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
