using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour {

    public float speed;
	public float gridSize;
	public GameObject wayPoint;
	public Text countText;
	
	private float wayPointTimer = 0.5f;
	private int breadCount = 0;
	private bool isMoving = false;
	private Rigidbody rb;

	void Start ()
    {
        this.rb = GetComponent<Rigidbody>();
		setCountText();

    }

    /*void FixedUpdate ()
    {
        float moveHorizontal = Input.GetAxis ("Horizontal");
        float moveVertical = Input.GetAxis ("Vertical");

        Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		//transform.position += new Vector3(moveHorizontal,0,moveVertical);

        rb.AddForce (movement * this.speed);
    }*/

	void Update ()
	{
		if (!isMoving) {
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");
			// I don't want it moving both directions at once, so just move horizontally if both are pressed.
			if (moveHorizontal != 0){
				StartCoroutine(move(this.transform, new Vector3(moveHorizontal, 0, 0)));
			}
			else if (moveVertical != 0) {
				StartCoroutine(move(this.transform, new Vector3(0, 0, moveVertical)));
			}
		}

		if (this.wayPointTimer > 0) {
			this.wayPointTimer -= Time.deltaTime;
		} else {
			UpdateWaypoint();
			this.wayPointTimer = 0.5f;
		}
	}

	void UpdateWaypoint ()
	{
		this.wayPoint.transform.position = this.transform.position;
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Breadcrumb"))
		{
			this.breadCount += 1;
			other.gameObject.SetActive (false);
			setCountText();
		}
	}

	void setCountText(){
		this.countText.text = "Count: " + this.breadCount.ToString ();
	}

	public IEnumerator move(Transform transform, Vector3 input) {
		this.isMoving = true;
		Vector3 startPosition = this.transform.position;
		float d = 0;

		Vector3 endPosition = new Vector3(startPosition.x + System.Math.Sign(input.x) * gridSize,
			                              startPosition.y, startPosition.z + System.Math.Sign(input.z) * gridSize);
		
		while (d < this.gridSize) {
			d += Time.deltaTime * this.speed;
			this.transform.position = Vector3.Lerp(startPosition, endPosition, d);
			yield return null;
		}
		
		this.isMoving = false;
		yield return 0;
	}

}

