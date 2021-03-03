using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace RVO
{
    public class CircleMonobehavior : MonoBehaviour
    {
        private IList<Vector2> goals;
        private GameObject[] prefabAgents;
        public GameObject prefab;

        // Start is called before the first frame update
        void Start()
        {
            /* Store the goals of the agents. */
            goals = new List<Vector2>();
            setupScenario();
        }

        void setupScenario()
        {
            /*
             * Specify the default parameters for agents that are subsequently
             * added.
             */
            Simulator.Instance.setAgentDefaults(15.0f, 10, 10.0f, 10.0f, 1.5f, 1.4f, new Vector2(0.0f, 0.0f));

            /*
             * Add agents, specifying their start position, and store their
             * goals on the opposite side of the environment.
             */
            prefabAgents = new GameObject[250];
            for (int i = 0; i < 250; ++i)
            {
                Simulator.Instance.addAgent(200.0f *
                    new Vector2((float)Mathf.Cos(i * 2.0f * Mathf.PI / 250.0f),
                        (float)Mathf.Sin(i * 2.0f * Mathf.PI / 250.0f)));
                goals.Add(-Simulator.Instance.getAgentPosition(i));
                
                Vector3 pos = new Vector3(Simulator.Instance.getAgentPosition(i).x(), 0,
                    Simulator.Instance.getAgentPosition(i).y());
                
                prefabAgents[i] = Instantiate(prefab, pos, Quaternion.identity);
            }
        }

        bool reachedGoal()
        {
            /* Check if all agents have reached their goals. */
            for (int i = 0; i < Simulator.Instance.getNumAgents(); ++i)
            {
                if (RVOMath.absSq(Simulator.Instance.getAgentPosition(i) - goals[i]) > Simulator.Instance.getAgentRadius(i) * Simulator.Instance.getAgentRadius(i))
                {
                    return false;
                }
            }

            return true;
        }

        void setPreferredVelocities()
        {
            /*
             * Set the preferred velocity to be a vector of unit magnitude
             * (speed) in the direction of the goal.
             */
            for (int i = 0; i < Simulator.Instance.getNumAgents(); ++i)
            {
                Vector2 goalVector = goals[i] - Simulator.Instance.getAgentPosition(i);

                if (RVOMath.absSq(goalVector) > 1.0f)
                {
                    goalVector = RVOMath.normalize(goalVector);
                }

                Simulator.Instance.setAgentPrefVelocity(i, goalVector);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!reachedGoal())
            {
                setPreferredVelocities();
                Simulator.Instance.setTimeStep(Time.deltaTime);
                Simulator.Instance.doStep();

                for (int i = 0; i < Simulator.Instance.getNumAgents(); ++i)
                {
                    prefabAgents[i].transform.position = new Vector3(
                        Simulator.Instance.getAgentPosition(i).x(), 0,
                        Simulator.Instance.getAgentPosition(i).y());

                    prefabAgents[i].transform.rotation = quaternion.LookRotationSafe(
                        new Vector3(Simulator.Instance.getAgentVelocity(i).x(), 0.0f,
                            Simulator.Instance.getAgentVelocity(i).y()),
                        math.up());
                }
            }
        }
    }
}