/// <summary>
/// Projectile.
/// Steve Peters
/// 2/15/2013
/// 
/// This class conotrols the lifetime and launching properties of our catapult projectile
/// </summary>
/// 
using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
	public float lifeTime; // The life of the projectile.	
	public Vector3 launchVector = new Vector3(0 , 0, 0);
	
	//launches the projectile when it hits the catapult launch trigger
	void OnTriggerEnter()
	{
		LaunchProjectile();
	}
	
	public void LaunchProjectile ()
	{
		GetComponent<Rigidbody>().isKinematic = false;
		
		if(transform.parent != null)
			transform.parent.DetachChildren();
		
		GetComponent<Rigidbody>().AddRelativeForce (launchVector,ForceMode.VelocityChange);
		Destroy(gameObject, lifeTime);
	}
	
	
		
	
}
