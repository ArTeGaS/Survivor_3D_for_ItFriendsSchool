using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneTransparencyController : MonoBehaviour
{
    public Transform player;
    public float transparency;
    public float fadeDuration;

    private List<Renderer> structureRenderers = new List<Renderer>();
    private Dictionary<Renderer, Coroutine> fadingRenderers = new Dictionary<Renderer, Coroutine>();

    void Start()
    {
        // Знайти всі об'єкти з тегом "Structure" та отримати їхні рендерери
        GameObject[] structures = GameObject.FindGameObjectsWithTag("Structure");
        foreach (GameObject structure in structures)
        {
            Renderer renderer = structure.GetComponent<Renderer>();
            if (renderer != null)
            {
                structureRenderers.Add(renderer);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            foreach (Renderer renderer in structureRenderers)
            {
                if (!fadingRenderers.ContainsKey(renderer))
                {
                    Coroutine coroutine = StartCoroutine(FadeTo(renderer, transparency));
                    fadingRenderers.Add(renderer, coroutine);
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            foreach (Renderer renderer in structureRenderers)
            {
                if (fadingRenderers.ContainsKey(renderer))
                {
                    StopCoroutine(fadingRenderers[renderer]);
                    fadingRenderers.Remove(renderer);
                }
                StartCoroutine(FadeTo(renderer, 1f));
            }
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