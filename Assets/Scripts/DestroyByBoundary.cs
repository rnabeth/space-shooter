using UnityEngine;
using System.Collections;

public class DestroyByBoundary : MonoBehaviour
{
	// Called when the Collider other has stopped touching the trigger
	void OnTriggerExit (Collider other)
	{
		// Destroy everything that leaves the trigger
		Destroy (other.gameObject);
	}
}
