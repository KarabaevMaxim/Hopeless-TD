using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
public class Controller : MonoBehaviour {

    public float speed = 10.0f;
    GameMode gameMode;
	// Use this for initialization
	void Start ()
    {
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!gameMode.gameOver && !gameMode.victory && !gameMode.pause)
        {
            //if (Input.mousePosition.x <= 5.0f)
            //{
            //    transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
            //}
            //if (Input.mousePosition.x >= Screen.width - 5.0f)
            //{
            //    transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            //}
            //if (Input.mousePosition.y >= Screen.height - 5.0f)
            //{
            //    transform.position += new Vector3(0, 0, speed * Time.deltaTime);
            //}
            //if (Input.mousePosition.y <= 5.0f)
            //{
            //    transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
            //}
             
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.position += new Vector3(0, 0, speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
            }
            transform.position += new Vector3(  CnInputManager.GetAxis("Horizontal") * speed * Time.deltaTime, 
                                                0f , CnInputManager.GetAxis("Vertical") * speed * Time.deltaTime);
        }
    }
}
