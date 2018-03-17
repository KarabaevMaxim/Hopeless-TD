/// <summary>
/// Catapult controller.
/// Steve Peters
/// 2/12/2013
/// 
/// This scripts provides basic firing and locomotion control for our catapult
/// We are using the animator from Unity 4.0
/// </summary>
using UnityEngine;
using System.Collections;

//Automatically assigns an animator, a rigidbody, and a capsule collider to the character
[RequireComponent(typeof(Animator))]  
[RequireComponent(typeof(BoxCollider))]  
[RequireComponent(typeof(Rigidbody))]  

public class CatapultController : MonoBehaviour
{
	
	public float moveSpeed = 10;
	public float turnSpeed = 25;
	public float animSpeed = 0.5f;
	
	public float launchAngle = 45.0f;
	public float launchForce = 35.0f;
	public Transform projectile;
	public Transform projectileSocket;
	public Transform launchTrigger;
	public Transform frontAxle;
	public Transform rearAxle;
	
	private Transform _myTransform;
	private float _wheelRotationSpeed;
	private Animator _animator;
	private AnimatorStateInfo _currentBaseState;			// a reference to the current state of the animator, used for base layer
	BoxCollider col;
	float h;
	float v;
	static int idleState = Animator.StringToHash ("Base Layer.Idle");
	static int fireState = Animator.StringToHash ("Base Layer.Fire");
	private int _maxLaunchAngle = 60;
	private int _minLaunchAngle = 30;
	private Transform _go;
	private bool _canLaunch;
	private Vector3 _launchVector;
	private bool _isStationary;
 
	void Awake ()
	{
		//asign the catapult as the player and cache the transform
		if (gameObject.tag != "Player")
			gameObject.tag = "Player";
		
		_myTransform = transform;
	}
	
	void Start ()
	{
		_animator = GetComponent<Animator> ();
		_animator.speed = animSpeed;
		
		if (_animator.layerCount > 1) {
			_animator.SetLayerWeight (1, 1.0f);	
		}
		
		//turn off the launch trigger so that the projectile won't launch upon instantiation
		launchTrigger.gameObject.SetActive(false);
		
		//multiply the move speed by a little magic to make the wheel rotation look right
		//Change this if the wheel appears to slide across the surface
		_wheelRotationSpeed = moveSpeed * 35;
	}
	
	
	// Update is called once per frame
	void Update ()
	{
		//prevent the catapult from firing while moving
		if((Input.GetButtonDown ("Fire1")) && _isStationary) {
			updateWeaponAttack ();
		}
		
		//stop the weapon from firing
		if (_currentBaseState.nameHash == fireState) {
			_animator.SetBool ("Load", false);
			_animator.SetBool ("Fire", false);
			launchTrigger.gameObject.SetActive(true);
		}
		
		//Keep the projectile rotation consistent for more accurate launches
		projectile.rotation = _myTransform.rotation;
	}
 
	void FixedUpdate ()
	{
 		// Grab Input each frame
		h = Input.GetAxis ("Horizontal");
		v = Input.GetAxis ("Vertical");	
		
		CheckIfStationary();
		
		_animator.SetFloat ("Speed", v);
		
		_currentBaseState = _animator.GetCurrentAnimatorStateInfo (0);	// set our currentState variable to the current state of the Base Layer (0) of animation
 
		ModifyMovementSpeed ();
	}
 
	
	/// <summary>
	/// Modifies the movement speed.
	/// </summary>
	public void ModifyMovementSpeed ()
	{
		//handles forward movement
		_myTransform.Translate (Vector3.forward * v * moveSpeed * Time.deltaTime);
		
		//handles rotation. Multiply be velocity so that the catapult can't turn while stationary
		//which is impossible with solid axles.
		_myTransform.Rotate (Vector3.up * v * h * turnSpeed * Time.deltaTime);
		
		frontAxle.Rotate (new Vector3 (v * _wheelRotationSpeed * Time.deltaTime, 0, 0));
		rearAxle.Rotate (new Vector3 (v * _wheelRotationSpeed * Time.deltaTime, 0, 0));
	}
	
	/// <summary>
	/// Updates the weapon attack.
	/// </summary>
	public void updateWeaponAttack ()
	{
		
		if (_currentBaseState.nameHash == idleState)
		{
			launchTrigger.gameObject.SetActive(false);
			_animator.SetBool ("Load", true);
			_go = Instantiate (projectile, projectileSocket.position, _myTransform.rotation) as Transform;
			_go.parent = projectileSocket;
			_go.GetComponent<Rigidbody>().isKinematic = true;
			AssignLaunchVector();
			_go.GetComponent<Projectile>().launchVector = _launchVector;
				
		}
		
		else if (_animator.GetBool ("Load") == true)
		{
			_animator.SetBool ("Fire", true);
		}
		
	}
	
	/// <summary>
	/// Assigns the launch vector, which is passed to the projectile when it is launched
	/// 
	/// </summary>
	public void AssignLaunchVector()
	{
			//Prevent improper inputs for force and angle
		if(launchForce < 1)
		{
			launchForce = 1;
		}
		
		if(launchAngle < _minLaunchAngle)
		{
			launchAngle = _minLaunchAngle;
		}
		
		if(launchAngle > _maxLaunchAngle)
		{
			launchAngle = _maxLaunchAngle;
		}
		
		
		float launchAngleInRadians = launchAngle * Mathf.Deg2Rad;
		float yforce = launchForce * Mathf.Sin(launchAngleInRadians);
		float zforce = launchForce * Mathf.Cos(launchAngleInRadians);
		
		_launchVector = new Vector3(0, yforce, zforce);
	}
	
	/// <summary>
	/// Checks if stationary.
	/// </summary>
	public void CheckIfStationary()
	{
	if((v < 0.05) && (v > -0.05))
		{
			_isStationary = true;
		}
		
		else
		{
			_isStationary = false;
		}
	}
}
