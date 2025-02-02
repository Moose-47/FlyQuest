using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{   
    public GameObject Player; 
    public float timeOffset;
    public Vector2 posOffset;

    public float leftLimit;
    public float rightLimit;
    public float bottomLimit;
    public float topLimit;

    private Vector3 velocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       //Camera current position
        Vector3 startPos = transform.position;

      //Player current position
        Vector3 endPos = Player.transform.position;

        endPos.x += posOffset.x;
        endPos.y += posOffset.y;
        endPos.z = -10;

        //Smoothly move the camera towards player position
        transform.position = Vector3.Lerp(startPos, endPos, timeOffset * Time.deltaTime);

        transform.position = new Vector3
            (
                Mathf.Clamp(transform.position.x, leftLimit, rightLimit),
                Mathf.Clamp(transform.position.y, bottomLimit, topLimit),
                transform.position.z
            );
    }
}
