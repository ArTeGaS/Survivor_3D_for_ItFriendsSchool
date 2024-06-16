using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    Animator animator;

    public float speed;
    public float dashSpeed; // �������� �� ��� "dash"
    public float dashDuration; // ��������� "dash"
    public static float hitPoints = 100;

    private Rigidbody rb;
    private bool isDashing = false; // ������

    public GameObject player;
    public GameObject mainCam;

    public Slider hpBar;
    public TextMeshProUGUI hpText;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Space) && !isDashing)
        {
            // ��������� ������� Dash
            Dash();
        }
        else
        {
            AxisMove(vertical, horizontal);
            //MoveToMouse(vertical, horizontal);
        }

        mainCam.transform.position = new Vector3(
            player.transform.position.x,
            player.transform.position.y + 10f,
            player.transform.position.z - 5f);

        OnDeath();
    }
    private void AxisMove(float vertical, float horizontal)
    {
        // ��������� ����
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y,
            Camera.main.transform.position.y - transform.position.y));
        Vector3 direction = mousePosition - transform.position;
        direction.y = 0;

        transform.LookAt(transform.position + direction);

        // ���
        Vector3 globalMove = new Vector3(horizontal, 0, vertical).normalized *
            speed * Time.fixedDeltaTime;
        Vector3 localMove = Quaternion.Inverse(
            Quaternion.LookRotation(direction)) * globalMove;
        rb.MovePosition(rb.position + globalMove);

        animator.SetFloat("Velocity Z", localMove.z);
        animator.SetFloat("Velocity X", localMove.x);

        Debug.Log(localMove.x + "---" + localMove.z);
    }
    private void MoveToMouse(float vertical, float horizontal)
    {
        // ��������� ����
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y,
            Camera.main.transform.position.y - transform.position.y));
        Vector3 direction = mousePosition - transform.position;
        direction.y = 0;

        float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);

        transform.Rotate(Vector3.up, angle);

        // ���
        Vector3 moveDirection = vertical * transform.forward + horizontal * transform.right;
        Vector3 move = moveDirection.normalized * speed * Time.fixedDeltaTime;

        rb.MovePosition(transform.position + move);

        animator.SetFloat("Velocity Z", vertical);
        animator.SetFloat("Velocity X", horizontal);
    }

    void Dash()
    {
        // ���������, �� �� � "dash"
        isDashing = true;

        // ������� �������� �� ��� "dash"
        speed = speed * 4;

        // ��������� ������ ��� ���������� "dash"
        StartCoroutine(EndDash());
    }

    IEnumerator EndDash()
    {
        // ������ ������� ���, ��� ��������� "dash"
        yield return new WaitForSecondsRealtime(dashDuration);

        speed = speed / 4;

        yield return new WaitForSecondsRealtime(3f);

        // �������� �������� ��� ��, �� "dash" ���������
        isDashing = false;
    }

    void OnDeath()
    {
        hpBar.value = Mathf.Clamp(hitPoints, 0, 100);
        hpText.text = $"HP:{hitPoints}";

        if (hpBar.value <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Score.score = 0;
        }
    }
}
