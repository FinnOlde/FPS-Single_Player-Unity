using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform PlayerPos;
    NavMeshPath path;

    // Start is called before the first frame update
    void Start()
    {
        path = new NavMeshPath();
    }

    // Update is called once per frame
    void Update()
    {
        agent.CalculatePath(PlayerPos.position, path);
        Debug.Log(path.status);
        agent.SetPath(path);
    }
}
