using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject bar;
    public GameObject outer;
    public GameObject title;
    public GameObject button;

    public bool canDecrease;

    Coroutine route;

    IEnumerator FadeTo(float aValue, float aTime)
    {
        float alpha = bar.GetComponent<Image>().material.color.a;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            float scaleX = bar.transform.localScale.x;

            float newScaleX = Mathf.Lerp(scaleX, aValue, t);

            bar.transform.localScale = new Vector2(newScaleX, 0.1f);

            yield return null;
        }
    }

    public void OnPlayPressed()
    {
        bar.SetActive(true);
        outer.SetActive(true);

        title.SetActive(false);
        button.gameObject.SetActive(false);

        GameplayManager.play.Invoke();
    }

    public void OnLayerComplete()
    {
        StopCoroutine(route);

        bar.transform.localScale = new Vector3(0.43f, 0.1f);

        route = StartCoroutine(FadeTo(0.0f, 1000.0f));
    }

    public void OnCorrectMatch()
    {
        StopCoroutine(route);

        if (bar.transform.localScale.x < 0.43f)
        {
            bar.transform.localScale = new Vector2(bar.transform.localScale.x + 0.1f, 0.1f);
        }

        route = StartCoroutine(FadeTo(0.0f, 1000.0f));
    }

    // Start is called before the first frame update
    void Start()
    {

        route = StartCoroutine(FadeTo(0.0f, 1000.0f));

        GameplayManager.correctMatch.AddListener(OnCorrectMatch);

        GameplayManager.finishedLevelTransition.AddListener(OnLayerComplete);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
