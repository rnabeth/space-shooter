using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{
	public float speed;

	//Executed on the very first frame the object is instantiated
	void Start ()
	{
		GetComponent<Rigidbody> ().velocity = transform.forward * speed; //z-axis
	}
}
