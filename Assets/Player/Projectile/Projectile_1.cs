using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile_1 : MonoBehaviour
{
    public float projectileSpeed;  // �������� ��������

    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        // ������������ ��������� ������� ���� � ��������� �������� � ������ ���
        Vector3 mouseScreenPosition = Input.mousePosition;
        // ������ �������, �� ��� ����������� ���� �� ������
        mouseScreenPosition.z = Camera.main.transform.position.y;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        // ��������� �� ������� ������, ��� ����������� ���� ���������� ����
        StartCoroutine(ProjectileDownForce());

        Vector3 direction = (mouseWorldPosition - transform.position).normalized;

        // ��������� �������� � �������� ����
        transform.rotation = Quaternion.LookRotation(direction);

        rb.velocity = direction * projectileSpeed;
        StartCoroutine(LifeTime());
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Score.score++;
            Debug.Log("������ ������!");
            Destroy(collision.gameObject);  // �������� ������
            Destroy(gameObject);  // �������� ��������
        }
    }
    IEnumerator ProjectileDownForce()
    {
        float currentY = transform.position.y;
        float targetY = 1.0f;
        float currentTime = 0.0f;
        float duration = 2.0f;
        while (currentTime < duration)
        {
            transform.position = new Vector3(
                transform.position.x,
                Mathf.Lerp(currentY, targetY, currentTime / duration),
                transform.position.z);
            currentTime += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}