using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMov : MonoBehaviour
{
    private GameObject playerObj;
    private Transform player; // посилання на об'єкт гравця
    public float moveSpeed = 5f; // швидкість руху ворога

    private Rigidbody rb;
    private bool isMoveing = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerObj = GameObject.Find("Player");
        player = playerObj.GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        // Перевірка, чи існує посилання на гравця
        if (player != null && isMoveing)
        {
            // Визначення вектора напрямку до гравця
            Vector3 direction = player.position - transform.position;
            direction.Normalize(); // Нормалізація для отримання одиничного вектора

            // Створюємо поворот в напрямку до гравця
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            lookRotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
            transform.rotation = lookRotation;

            // Рух ворога в напрямку гравця
            rb.velocity = direction * moveSpeed;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    IEnumerator StopMoving()
    {
        isMoveing = false;
        yield return new WaitForSeconds(3f);
        isMoveing = true;
    }
}
