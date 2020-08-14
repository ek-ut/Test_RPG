using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Hero;

    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset.x = transform.position.x - Hero.transform.position.x;
        offset.y = transform.position.y - Hero.transform.position.y;
        offset.z = transform.position.z - Hero.transform.position.z;
    }

    // LateUpdate is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3( Hero.transform.position.x + offset.x, Hero.transform.position.y + offset.y, Hero.transform.position.z + offset.z);
    }
}
