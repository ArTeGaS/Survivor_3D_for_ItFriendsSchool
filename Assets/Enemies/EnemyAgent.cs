using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAgent : MonoBehaviour
{
    private GameObject playerObj;
    private Transform player;
    private NavMeshAgent agent; // ����� ��� ���������� NavMeshAgent

    private void Start()
    {
        playerObj = GameObject.Find("Player");
        player = playerObj.GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>(); // ��������� ���������� NavMeshAgent
    }

    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position); // ������������ ������ ����� �������� ��� NavMeshAgent
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform == player)
        {
            PlayerMove.hitPoints -= 10;
            StartCoroutine(StopMoving());
            Debug.Log(PlayerMove.hitPoints);
        }
    }

    IEnumerator StopMoving()
    {
        agent.isStopped = true; // ������� NavMeshAgent
        yield return new WaitForSeconds(3f);
        agent.isStopped = false; // ³��������� ���� NavMeshAgent
    }
}