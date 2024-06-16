using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMov : MonoBehaviour
{
    private GameObject playerObj;
    private Transform player; // ��������� �� ��'��� ������
    public float moveSpeed = 5f; // �������� ���� ������

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
        // ��������, �� ���� ��������� �� ������
        if (player != null && isMoveing)
        {
            // ���������� ������� �������� �� ������
            Vector3 direction = player.position - transform.position;
            direction.Normalize(); // ����������� ��� ��������� ���������� �������

            // ��������� ������� � �������� �� ������
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            lookRotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
            transform.rotation = lookRotation;

            // ��� ������ � �������� ������
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
