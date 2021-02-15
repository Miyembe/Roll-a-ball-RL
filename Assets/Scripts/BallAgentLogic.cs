using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class BallAgentLogic : Agent
{

    Rigidbody rBody;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public Transform target;
    public override void OnEpisodeBegin()
    {
        // Reset agent
        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
        this.transform.localPosition = new Vector3(0.0f, 0.5f, 0.0f);

        // Move target to a new spot
        target.localPosition = new Vector3(Random.value * 16 - 8, 0.5f, Random.value * 16 - 8);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions & Agent velocity
        sensor.AddObservation(target.localPosition);
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(rBody.velocity);

        // Calculate the distance between the agent and the target
        float atdistance = Vector3.Distance(target.localPosition, this.transform.localPosition);
        sensor.AddObservation(atdistance);
    }


    public float speed = 20;
    public override void OnActionReceived(float[] vectorAction)
    {
        Vector3 controlSignal = Vector3.zero;

        if (vectorAction[0] == 1)
        {
            controlSignal.x = 1;
        }
        else
        {
            controlSignal.x = -1;
        }

        if (vectorAction[1] == 1)
        {
            controlSignal.z = 1;
        }
        else
        {
            controlSignal.z = -1;
        }

        // Prevent adding forces after jumping
        if (this.transform.localPosition.x < 20.0)
        {
            rBody.AddForce(controlSignal * speed);
        }

        float distanceToTarget = Vector3.Distance(this.transform.localPosition, target.localPosition);
        // Reached target
        if (distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        // Fell of platform
        if (this.transform.localPosition.x < -8.1 || this.transform.localPosition.x > 8.1 || this.transform.localPosition.z > 8.1 || this.transform.localPosition.z < -8.1)
        {
            SetReward(-1.0f);
            EndEpisode();
        }

        float atdistance = Vector3.Distance(target.localPosition, this.transform.localPosition);
        SetReward(-atdistance*0.05f);
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Vertical");
        actionsOut[1] = Input.GetAxis("Horizontal");
    }

}

