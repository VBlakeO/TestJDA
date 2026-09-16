using UnityEngine.AI;
using UnityEngine;

public class Pedestrians : MonoBehaviour
{
    public Transform[] targets = new Transform[0];
    public int currentTarget = -1;
    public NavMeshAgent agent = null;
    public bool going = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(targets[0].position);

        GetComponent<Animator>().SetBool("Walk", true);
    }

    private void FixedUpdate()
    {
        if (agent != null && agent.remainingDistance < 1)
        {
            if(going)
            {
                if (currentTarget < targets.Length - 1)
                    currentTarget++;
                else
                    going = false;
            }
            else
            {
                 if (currentTarget > 0)
                    currentTarget--;
                else
                    going = true;
            }

            agent.SetDestination(targets[currentTarget].position);  
        }
    }
}
