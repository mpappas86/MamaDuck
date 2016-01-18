using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    public float speed;
	public GameObject wayPoint;
	private float wayPointTimer = 0.5f;

/*    void Start ()
    {
        rb = GetComponent<Rigidbody>();
    }
*/

    void FixedUpdate ()
    {
        float moveHorizontal = Input.GetAxis ("Horizontal");
        float moveVertical = Input.GetAxis ("Vertical");

        //Vector3 movement = new Vector3 (moveHorizontal != 0 ? 1 : 0, 0.0f, moveVertical != 0 ? 1 : 0);

		transform.position += new Vector3(moveHorizontal,0,moveVertical);

        //rb.AddForce (movement * speed);
    }

	void Update ()
	{
		if (wayPointTimer > 0) {
			wayPointTimer -= Time.deltaTime;
		} else {
			UpdateWaypoint();
			wayPointTimer = 0.5f;
		}
	}

	void UpdateWaypoint ()
	{
		wayPoint.transform.position = transform.position;
	}
}
