using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour
{

	public Rigidbody projectile;

	public float fireRate = 1.0f;
	private float nextFire = 0.0f;

	private bool movingRight;

	private Vector3 originalPosition;

	public float moveSpeed;


	// Use this for initialization
	void Start ()
	{
		originalPosition = transform.position;
	}	

	void Update ()
	{

		if ((transform.position.x - originalPosition.x) > 5f) {
			movingRight = false;
		} else if ((originalPosition.x - transform.position.x) > 5f) {
			movingRight = true;
		}

		if (movingRight) {
			transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
		} else {
			transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
		}

		if (Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			Rigidbody clone = Object.Instantiate(projectile, transform.position + new Vector3(0.0f, 0.0f, 1.0f), transform.rotation) as Rigidbody;
			clone.velocity = transform.TransformDirection(Vector3.forward * 10);
		}

	}

}
