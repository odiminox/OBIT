using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public GameObject pivot;

    public float speed;

    enum DIRECTION { LEFT, RIGHT };
    DIRECTION currentDirection = DIRECTION.LEFT;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentDirection == DIRECTION.LEFT)
            {
                currentDirection = DIRECTION.RIGHT;
            }
            else if (currentDirection == DIRECTION.RIGHT)
            {
                currentDirection = DIRECTION.LEFT;
            }
        }

        switch (currentDirection)
        {
            case DIRECTION.LEFT:
                {
                    break;
                }
            case DIRECTION.RIGHT:
                {
                    break;
                }
            default:
                {
                    break;
                }
        }

        transform.RotateAround(pivot.transform.position, Vector3.forward, speed * Time.deltaTime);
    }
}
