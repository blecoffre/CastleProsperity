using UnityEngine;
using UnityEngine.AI;

namespace CastleProsperity.Utils
{
    class NavMeshUtils
    {
        public static bool DestinationReached(NavMeshAgent agent, Vector3 actualPosition)
        {
            //  because takes some time to update the remainingDistance and will return a wrong value
            if (agent.pathPending)
            {
                return Vector3.Distance(actualPosition, agent.pathEndPosition) <= agent.stoppingDistance;
            }
            else
            {
                return (agent.remainingDistance <= agent.stoppingDistance);
            }
        }
    }
}
