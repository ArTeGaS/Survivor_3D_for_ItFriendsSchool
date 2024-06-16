using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TransparencyController : MonoBehaviour
{
    public Transform player;
    public LayerMask obstacleLayer;
    public float transparency = 0.5f;
    public float fadeDuration = 0.5f;

    private Dictionary<Renderer, Coroutine> fadingRenderers = new Dictionary<Renderer, Coroutine>();

    void Update()
    {
        MakeObstaclesTransparent();
    }

    void MakeObstaclesTransparent()
    {
        Vector3 direction = player.position - Camera.main.transform.position;
        float distance = direction.magnitude;

        if (Physics.Raycast(Camera.main.transform.position, direction, out RaycastHit hit, distance, obstacleLayer))
        {
            Renderer renderer = hit.collider.GetComponent<Renderer>();

            if (renderer != null)
            {
                if (!fadingRenderers.ContainsKey(renderer))
                {
                    Coroutine coroutine = StartCoroutine(FadeTo(renderer, transparency));
                    fadingRenderers.Add(renderer, coroutine);
                }
            }

            List<Renderer> keysToRemove = new List<Renderer>();
            foreach (var pair in fadingRenderers)
            {
                if (pair.Key != renderer)
                {
                    StopCoroutine(pair.Value);
                    StartCoroutine(FadeTo(pair.Key, 1f));
                    keysToRemove.Add(pair.Key);
                }
            }

            foreach (Renderer key in keysToRemove)
            {
                fadingRenderers.Remove(key);
            }
        }
        else
        {
            foreach (var pair in fadingRenderers)
            {
                StopCoroutine(pair.Value);
                StartCoroutine(FadeTo(pair.Key, 1f));
            }
            fadingRenderers.Clear();
        }
    }

    IEnumerator FadeTo(Renderer renderer, float targetAlpha)
    {
        foreach (Material mat in renderer.materials)
        {
            Color color = mat.color;
            float startAlpha = color.a;
            float time = 0;

            while (time < fadeDuration)
            {
                time += Time.deltaTime;
                color.a = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
                mat.color = color;

                if (targetAlpha < 1f)
                {
                    mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    mat.SetInt("_ZWrite", 0);
                    mat.DisableKeyword("_ALPHATEST_ON");
                    mat.EnableKeyword("_ALPHABLEND_ON");
                    mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                }
                else
                {
                    mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    mat.SetInt("_ZWrite", 1);
                    mat.DisableKeyword("_ALPHABLEND_ON");
                    mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    mat.renderQueue = -1;
                }

                yield return null;
            }

            color.a = targetAlpha;
            mat.color = color;
        }
    }
}