using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer : MonoBehaviour
{
    public bool canScale;
    public float scaleSpeed;
    public int layerIndex;

    public Vector2 targetScale = new Vector2();

    public void ScaleUp()
    {
        if (Mathf.Abs(Vector2.Distance(transform.localScale, targetScale)) <= 0.1f)
        {
            canScale = false;
        }

        transform.localScale = Vector2.Lerp(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);
    }

    public void UpdateTargetScale()
    {
        switch (layerIndex)
        {
            case 0:
                targetScale = new Vector2(20f, 20f);
                break;
            case 1:
                targetScale = new Vector2(50f, 50f);
                break;
            case 2:
                targetScale = new Vector2(80f, 80f);
                break;
            default:
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateTargetScale();
    }

    // Update is called once per frame
    void Update()
    {
        if (canScale)
        {
            ScaleUp();
        }
    }
}
