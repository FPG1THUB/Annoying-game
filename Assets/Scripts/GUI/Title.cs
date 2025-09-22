using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour
{
    [Header("Rotation Settings")]
    [Tooltip("Minimum rotation speed in degrees per second")]
    public float minSpeed = 80f;
    [Tooltip("Maximum rotation speed in degrees per second")]
    public float maxSpeed = 120f;
    [Tooltip("Maximum angle before swapping direction (degrees)")]
    public float maxAngle = 180f;
    [Tooltip("Chance to swap direction randomly (0-1)")]
    public float directionChangeProbability = 0.2f;
    [Tooltip("Time between random direction checks")]
    public float checkInterval = 0.5f;

    private float currentSpeed;
    private float accumulatedRotation;

    void Start()
    {
        InitializeRotation();
        StartCoroutine(RandomDirectionCheck());
    }

    void InitializeRotation()
    {
        float randomSpeed = Random.Range(minSpeed, maxSpeed);
        int direction = (Random.Range(0, 2) == 0) ? -1 : 1;
        currentSpeed = randomSpeed * direction;
        accumulatedRotation = 0f;
    }

    IEnumerator RandomDirectionCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);

            if (Random.value < directionChangeProbability)
            {
                currentSpeed *= -1;
                accumulatedRotation = 0f;
            }
        }
    }

    void Update()
    {
        float rotationThisFrame = currentSpeed * Time.deltaTime;
        transform.Rotate(0, 0, rotationThisFrame);

        accumulatedRotation += Mathf.Abs(rotationThisFrame);

        if (accumulatedRotation >= maxAngle)
        {
            currentSpeed *= -1;
            accumulatedRotation = 0f;
        }
    }
}