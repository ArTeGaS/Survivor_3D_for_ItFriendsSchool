using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAgent : MonoBehaviour
{
    private GameObject playerObj;
    private Transform player;
    private NavMeshAgent agent; // Змінна для компонента NavMeshAgent

    private void Start()
    {
        playerObj = GameObject.Find("Player");
        player = playerObj.GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>(); // Отримання компонента NavMeshAgent
    }

    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position); // Встановлення кінцевої точки маршруту для NavMeshAgent
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
        agent.isStopped = true; // Зупинка NavMeshAgent
        yield return new WaitForSeconds(3f);
        agent.isStopped = false; // Відновлення руху NavMeshAgent
    }
}