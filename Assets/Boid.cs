using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public float visionRadius;
    public float separationDistance;
    public float maxSpeed;
    public float adjustment;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(
            Random.Range(-0.4f * maxSpeed, 0.4f * maxSpeed),
            Random.Range(-0.4f * maxSpeed, 0.4f * maxSpeed)
        );
    }

    void Update()
    {
        FlyToCenterOfMass();
        MatchVelocity();
        MaintainSeparation();
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        StayInbounds();
        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.velocity += rb.velocity * adjustment * Time.deltaTime * 0.2f;
        } 
    }

    private void FlyToCenterOfMass()
    {
        Vector3 center = Vector2.zero;
        int numBoidsInVision = 0;
        for (int i = 0; i < BoidManager.Instance.allBoids.Count; i++)
        {
            GameObject otherBoid = BoidManager.Instance.allBoids[i];
            if (otherBoid != gameObject)
            {
                float distance = Vector2.Distance(transform.position, otherBoid.transform.position);
                if (distance < visionRadius)
                {
                    center += otherBoid.transform.position;
                    numBoidsInVision++;
                }
            }
        }
        if (numBoidsInVision > 0)
        {
            center /= numBoidsInVision;
            Vector2 directionToCenter = (center - transform.position).normalized;
            rb.velocity = rb.velocity + adjustment * Time.deltaTime * directionToCenter;
        }
    }
    private void MatchVelocity()
    {
        Vector2 avgVelocity = Vector2.zero;
        int numBoidsInVision = 0;
        Vector2 myVelocity = rb.velocity;
        for (int i = 0; i < BoidManager.Instance.allBoids.Count; i++)
        {
            GameObject otherBoid = BoidManager.Instance.allBoids[i];
            if (otherBoid != gameObject)
            {
                float distance = Vector2.Distance(transform.position, otherBoid.transform.position);
                if (distance < visionRadius)
                {
                    avgVelocity += otherBoid.GetComponent<Rigidbody2D>().velocity;
                    numBoidsInVision++;
                }
            }
        }
        if (numBoidsInVision > 0)
        {
            avgVelocity /= numBoidsInVision;
            Vector2 velocityDiff = avgVelocity - myVelocity;
            Vector2 velocityDiffDirection = velocityDiff.normalized;
            rb.velocity += adjustment * Time.deltaTime * velocityDiffDirection;
        }
    }

    private void MaintainSeparation()
    {
        Vector2 separationForce = Vector2.zero;
        for (int i = 0; i < BoidManager.Instance.allBoids.Count; i++)
        {
            GameObject otherBoid = BoidManager.Instance.allBoids[i];
            if (otherBoid != gameObject)
            {
                float distance = Vector2.Distance(transform.position, otherBoid.transform.position);
                if (distance < separationDistance)
                {
                    Vector2 awayFromBoid = transform.position - otherBoid.transform.position;
                    separationForce += awayFromBoid.normalized / distance;
                }
            }
        }
        rb.velocity += adjustment * Time.deltaTime * separationForce;
    }

    private void StayInbounds()
    {
        if (transform.position.x > 8.0f)
        {
            rb.velocity += new Vector2(-adjustment * Time.deltaTime * 10, 0);
        }
        else if (transform.position.x < -8.0f)
        {
            rb.velocity += new Vector2(adjustment * Time.deltaTime * 10, 0);
        }

        if (transform.position.y > 4.0f)
        {
            rb.velocity += new Vector2(0, -adjustment * Time.deltaTime * 10);
        }
        else if (transform.position.y < -4.0f)
        {
            rb.velocity += new Vector2(0, adjustment * Time.deltaTime * 10);
        }
    }
}
