using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebShot : MonoBehaviour
{
    [SerializeField] LayerMask ShootableMask;

    [SerializeField] LineRenderer webLine;
    [SerializeField] SpringJoint2D webJoints;

    [SerializeField] float maxShotDistance = 10f;
    [SerializeField] float movementSpeed = 10f;
    [SerializeField] float shootSpeed = 20f;

    private float webDistance;

    bool isShooting = false;
    bool shotIsConnected = false;
    bool shotIsRetracting = false;

    private Vector2 lookDirection;
    private Vector3 pointOfConnection;
    private Vector3 shotPosDistance;

    [Header("Unfinished:")]
    [SerializeField] float webLinePoints = 60;

    [Header("Animation Settings:")]
    public AnimationCurve animationCurve;
    [Range(0.01f, 4)] [SerializeField] private float StartWaveSize = 2;
    float waveSize = 0;

    [Header("Animation Progression:")]
    public AnimationCurve progressionCurve;
    [SerializeField] [Range(1, 50)] private float progressionSpeed = 1;

    float moveTime = 0;

    void Start()
    {
        webJoints.enabled = false;
        webLine.enabled = false;
    }

    void Update()
    {
        //Get the location of where the player clicked on the screen & subtract it by the player's position to get the distance between the two.
        lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        Debug.DrawLine(transform.position, lookDirection);

        if (Input.GetMouseButtonDown(0) && shotIsConnected)
        {
            StopShooting();
        }

        if (Input.GetMouseButtonDown(0) && !isShooting)
        {
            CheckForCollision();
        }

        if (isShooting && !shotIsConnected)
        {
            //moveTime += Time.deltaTime;
            ShootWeb();
        }

        if (shotIsRetracting)
        {
            RetractWeb();
        }
    }



    private void CheckForCollision()
    {
        //Raycast to check if we've hit anything between the click-point & the player
        RaycastHit2D hit = Physics2D.Raycast(transform.position, lookDirection, maxShotDistance, ShootableMask);

        if (hit.collider != null)
        {
            isShooting = true;

            webDistance = 0;
            webLine.SetPosition(0, transform.position);
            webLine.SetPosition(1, transform.position);
            webLine.enabled = true;
            pointOfConnection = hit.point;
        }
    }

    private void ShootWeb()
    {
        webDistance += shootSpeed * Time.deltaTime;
        Vector2 shotPointDistance = Vector2.Lerp(transform.position, pointOfConnection, webDistance / 10);

        if (Vector2.Distance(shotPointDistance, pointOfConnection) > .5f)
        {
            webLine.SetPosition(0, transform.position);
            webLine.SetPosition(1, shotPointDistance);
        }
        else
        {
            shotIsConnected = true;
            shotIsRetracting = true;

            webLine.SetPosition(1, pointOfConnection);
            webJoints.connectedAnchor = pointOfConnection;
            webJoints.enabled = true;
        }
    }

    private void RetractWeb()
    {
        float distance = Vector2.Distance(transform.position, pointOfConnection);

        //Distance where the web strops retracting
        if (distance >= 1.5f)
        {
            shotPosDistance = Vector2.Lerp(transform.position, pointOfConnection, movementSpeed * Time.deltaTime);
            transform.position = shotPosDistance;
        }

        webJoints.distance = distance;
        webLine.SetPosition(0, transform.position);
    }

    private void StopShooting()
    {
        isShooting = false;
        shotIsConnected = false;
        shotIsRetracting = false;

        webLine.enabled = false;
        webJoints.enabled = false;
        moveTime = 0;
    }

    private void DrawShotWeb()
    {
        for (int i = 0; i < webLinePoints; i++)
        {
            float delta = (float)i / ((float)webLinePoints - 1f);
            Vector2 offset = Vector2.Perpendicular(shotPosDistance).normalized * animationCurve.Evaluate(delta) * waveSize;
            Vector2 targetPosition = Vector2.Lerp(transform.position, pointOfConnection, delta) + offset;
            Vector2 currentPosition = Vector2.Lerp(transform.position, targetPosition, progressionCurve.Evaluate(moveTime) * progressionSpeed);

            webLine.SetPosition(i, currentPosition);
        }
    }
}