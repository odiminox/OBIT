using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipes : MonoBehaviour
{
    public Sprite pipesA;
    public Sprite pipesB;
    public Sprite pipesC;

    public void SetRandomPipes()
    {
        int selection = Random.Range(1, 3);

        switch (selection)
        {
            case 1:
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = pipesA;

                    break;
                }
            case 2:
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = pipesB;

                    break;
                }
            case 3:
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = pipesC;

                    break;
                }
            default:
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
