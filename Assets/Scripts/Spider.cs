using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    [Header("References:")]
    [SerializeField] LayerMask ShootableMask;

    [SerializeField] GameObject Crosshair;
    Vector3 mousePos;

    private List<Vector2> lineColliderPoints;
    [SerializeField] PolygonCollider2D webLineCollider;
    [SerializeField] LineRenderer webLine;
    [SerializeField] SpringJoint2D webJoints;
    [SerializeField] Rigidbody2D spiderRB;

    [Header("Webshot Settings:")]
    [SerializeField] float maxShotDistance = 10f;
    [SerializeField] float shootSpeed = 20f;
    [SerializeField] float retractionSpeed = 2f;
    [SerializeField] float swingForce = 5f;

    [SerializeField] int shotsLeft = 5;

    float timer = 1.5f;
    float timerStart = 1.5f;


    private float webDistance;
    private Platform target;

    bool isShooting = false;
    bool isShotConnected = false;
    bool isInvincible = false;
    bool autoRetracting = true;

    private float horizontalInputs;
    private float verticalInputs;


    float shotDistance = 0f;

    private Vector2 lookDirection;
    private Vector3 pointOfConnection;
    private Vector3 shotPosDistance;

    void Start()
    {
        webJoints.enabled = false;
        webLine.enabled = false;
        webLineCollider.enabled = false;
    }

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Get the location of where the player clicked on the screen & subtract it by the player's position to get the distance between the two.
        lookDirection = mousePos - transform.position;
        Debug.DrawLine(transform.position, lookDirection);

        if(isInvincible)
        {
            timer -= Time.deltaTime;

            if(timer < 0 || isShooting)
            {
                isInvincible = false;
                timer = timerStart;
            }
        }

        if (shotsLeft > 0)
        {
            if (Vector2.Distance(transform.position, mousePos) < maxShotDistance)
            {
                if (!Crosshair.activeInHierarchy)
                {
                    Crosshair.SetActive(true);
                    Cursor.visible = false;
                }
                mousePos.z += Camera.main.nearClipPlane;
                Crosshair.transform.position = mousePos;
            }
            else if (Crosshair.activeInHierarchy)
            {
                Cursor.visible = true;
                Crosshair.SetActive(false);
            }
        }

        horizontalInputs = Input.GetAxisRaw("Horizontal");
        verticalInputs = Input.GetAxisRaw("Vertical");

        if (isShotConnected && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
        {
            StopShooting();
        }

        if (Input.GetMouseButtonDown(0) && !isShooting && shotsLeft > 0)
        {
            CheckForCollision();
        }

        if (isShooting)
        {
            if (!isShotConnected)
            {
                ShootWeb();
            }

            if (isShotConnected)
            {
                shotDistance = Vector2.Distance(transform.position, pointOfConnection);

                if (verticalInputs > 0.1f || verticalInputs < -0.1f)
                {
                    if(autoRetracting)
                    {
                        autoRetracting = false;
                    }

                    if(verticalInputs > 0.1f)
                    {
                        RetractWeb();
                    }
                    else
                    {
                        DescendWeb();
                    }
                }
                else if(autoRetracting)
                {
                    RetractWeb();
                }

                webJoints.distance = shotDistance;
                webLine.SetPosition(0, transform.position);
            }

            lineColliderPoints = CalculateWebLinePoints();
            webLineCollider.SetPath(0, lineColliderPoints.ConvertAll(p => (Vector2)transform.InverseTransformPoint(p)));
        }
    }

    private void FixedUpdate()
    {
        if(isShotConnected)
        {
            Vector2 swingInputs = new Vector2(horizontalInputs * swingForce * Time.fixedDeltaTime, 0f);
            spiderRB.AddForce(swingInputs);
        }
    }

    private void CheckForCollision()
    {
        //Raycast to check if we've hit anything between the click-point & the player
        RaycastHit2D hit = Physics2D.Raycast(transform.position, lookDirection, maxShotDistance, ShootableMask);

        if (hit.collider != null)
        {
            isShooting = true;
            shotsLeft--;

            webDistance = 0;
            webLine.SetPosition(0, transform.position);
            webLine.SetPosition(1, transform.position);
            webLine.enabled = true;

            webLineCollider.enabled = true;

            pointOfConnection = hit.point;

            Platform platform = hit.collider.gameObject.GetComponentInParent<Platform>();

            if (platform)
            {
                target = platform;
                target.SetIsGrappled(true);
            }
        }
    }

    private void ShootWeb()
    {
        webDistance += shootSpeed * Time.deltaTime;
        Vector2 shotPointDistance = Vector2.Lerp(transform.position, pointOfConnection, webDistance / 10);

        if (Vector2.Distance(shotPointDistance, pointOfConnection) > 1f)
        {
            webLine.SetPosition(0, transform.position);
            webLine.SetPosition(1, shotPointDistance);
        }
        else
        {
            isShotConnected = true;
            autoRetracting = true;

            webLine.SetPosition(1, pointOfConnection);
            webJoints.connectedAnchor = pointOfConnection;
            webJoints.enabled = true;
        }
    }

    private Vector3[] GetWebLinePositions()
    {
        Vector3[] webLinePositions = new Vector3[webLine.positionCount];
        webLine.GetPositions(webLinePositions);
        return webLinePositions;
    }

    private List<Vector2> CalculateWebLinePoints()
    {
        Vector3[] linePositions = GetWebLinePositions();
        float width = webLine.startWidth;

        float m = ((linePositions[1].y - linePositions[0].y) / (linePositions[1].x - linePositions[0].x));
        float deltaX = (width / 2f) * (m / Mathf.Pow(m * m + 1, 0.5f));
        float deltaY = (width / 2f) * (1 / Mathf.Pow(m * m + 1, 0.5f));

        Vector3[] offsets = new Vector3[2];
        offsets[0] = new Vector3(-deltaX, deltaY);
        offsets[1] = new Vector3(deltaX, -deltaY);


        List<Vector2> colliderPoints = new List<Vector2>
        {
            linePositions[0] + offsets[0],
            linePositions[1] + offsets[0],
            linePositions[1] + offsets[1],
            linePositions[0] + offsets[1],
        };

        return colliderPoints;
    }


    private void RetractWeb()
    {
        //Distance where the web stops retracting
        if (shotDistance >= 1.5f)
        {
            shotPosDistance = Vector2.Lerp(transform.position, pointOfConnection, retractionSpeed * Time.fixedDeltaTime);
            transform.position = shotPosDistance;
        }
    }

    private void DescendWeb()
    {
        //Distance where the web stops descending
        if (shotDistance < maxShotDistance / 2)
        {
            shotPosDistance = new Vector2(transform.position.x, transform.position.y - (retractionSpeed + retractionSpeed) * Time.fixedDeltaTime);
            transform.position = shotPosDistance;
        }
    }

    private void StopShooting()
    {
        isShooting = false;
        isShotConnected = false;
        autoRetracting = true;

        webLine.enabled = false;
        webJoints.enabled = false;
        webLineCollider.enabled = false;

        if(target != null)
        {
            target.SetIsGrappled(false);
            target = null;
        }
    }

    public Vector2 GetPlayerPos()
    {
        return transform.position;
    }

    public float GetPlayerPosY()
    {
        return transform.position.y;
    }

    public bool IsShooting()
    {
        return isShooting;
    }

    public bool IsShotConnected()
    {
        return isShotConnected;
    }

    public void CutWeb()
    {
        StopShooting();
    }

    public void AddNewShot(int shotCount = 1)
    {
        shotsLeft += shotCount;
    }

    public void SetShotsLeft(int shotCount = 3)
    {
        shotsLeft = shotCount;
    }

    public int GetShotsLeft()
    {
        return shotsLeft;
    }

    public void SetInvincible()
    {
        isInvincible = true;
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }
}