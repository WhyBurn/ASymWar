using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(0, transform.position.z * Time.deltaTime * -1, 0);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(transform.position.z * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(0, transform.position.z * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(transform.position.z * Time.deltaTime * -1, 0, 0);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            GetComponent<Camera>().orthographicSize += Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.E))
        {
            GetComponent<Camera>().orthographicSize = Mathf.Max(1, GetComponent<Camera>().orthographicSize - Time.deltaTime);
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = new Vector3(0, 0, Mathf.Min(-10, Mathf.Max(-500, transform.position.z)));
        }
    }
}
