using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public GameObject loadingScreen;
    public GameObject dot1;
    public GameObject dot2;
    public GameObject dot3;

    private void Start()
    {
        StartCoroutine(AnimateDots());
    }

    IEnumerator AnimateDots()
    {
        while (true)
        {
            dot1.transform.localScale = new Vector3 (2f, 2f, 2f);
            yield return new WaitForSecondsRealtime(1f);
            dot1.transform .localScale = new Vector3 (1f, 1f,1f);

            dot2.transform.localScale = new Vector3(2f, 2f, 2f);
            yield return new WaitForSecondsRealtime(1f);
            dot2.transform.localScale = new Vector3(1f, 1f, 1f);

            dot3.transform.localScale = new Vector3(2f, 2f, 2f);
            yield return new WaitForSecondsRealtime(1f);
            dot3.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
