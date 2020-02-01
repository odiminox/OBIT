using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public float shakeDuration = 0f;

    // Desired duration of the shake effect
    // A measure of magnitude for the shake. Tweak based on your preference
    private float shakeMagnitude = 0.05f;

    // A measure of how quickly the shake effect should evaporate
    private float dampingSpeed = 1.0f;

    // The initial position of the GameObject
    Vector3 initialPosition;

    public void OnScreenShake()
    {
        shakeDuration = 0.05f;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerControls.missed.AddListener(OnScreenShake);
        initialPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeDuration > 0)
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            transform.localPosition = initialPosition;
        }

    }
}
