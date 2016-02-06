using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour {

    public float speed;
	public GameObject wayPoint;
	public Text mainText;
	
	private float wayPointTimer = 0.5f;
	private int breadCount = 0;
	private Vector3 movingDir;
	public int ducklingCount = 0;
	public int totalDucklings;
	private Rigidbody rb;

	void Start ()
    {
		setMainText();
		rb = this.gameObject.GetComponent<Rigidbody> ();
		rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }
	
	void Update ()
	{
		this.transform.position = Vector3.Scale (this.transform.position, new Vector3 (1, 0, 1));
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		// I don't want it moving both directions at once, so just move horizontally if both are pressed.
		if (moveHorizontal != 0) {
			this.movingDir = new Vector3 (Mathf.Sign (moveHorizontal), 0, 0);
		} else if (moveVertical != 0) {
			this.movingDir = new Vector3 (0, 0, Mathf.Sign (moveVertical));
		}

		if (this.wayPointTimer > 0) {
			this.wayPointTimer -= Time.deltaTime;
		} else {
			UpdateWaypoint();
			this.wayPointTimer = 0.5f;
		}
	}

	void FixedUpdate (){
		this.rb.MovePosition (transform.position + this.movingDir * speed * Time.deltaTime);
	}

	void UpdateWaypoint ()
	{
		this.wayPoint.transform.position = this.transform.position;
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Breadcrumb")) {
			this.breadCount += 1;
			other.gameObject.SetActive (false);
			setMainText ();
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag ("Wall")) {
			this.movingDir = Vector3.zero;
		}
	}

	string DucklingText(){
		if (this.ducklingCount == 0) {
			return "No ducklings found!";
		} else if (this.ducklingCount == this.totalDucklings) {
			return "You've collected all the ducklings in this level!";
		} else {
			return "You've collected " + this.ducklingCount.ToString() + " ducklings so far!";
		}
	}

	public void setMainText(){
		this.mainText.text = DucklingText() + "\nBreadcrumbs: " + this.breadCount.ToString ();
	}

	public void MurderDuckling(string cause){
		this.mainText.text = "You just lost a duckling to a " + cause + "! You monster!";
	}
	

}

