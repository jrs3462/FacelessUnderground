using UnityEngine;
using System;
using System.Collections;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public float rate = 2f;

    private Seeker seeker;
    private Rigidbody2D rb;

    public Path path;

    public float speed = 300f;
    public ForceMode2D fMode;

    public bool pathIsEnded = false;

    public float nextWaypointDistance = 3f;

    private int currentWaypoint = 0;

    private bool m_FacingLeft = true;  // For determining which way the player is currently facing.

    Transform enemyGraphics;

    public float health;

    public GameObject light;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        seeker.StartPath(transform.position, target.position, OnPathComplete);

        StartCoroutine(UpdatePath());

        enemyGraphics = transform.Find("ghost");

        health = 10f;
    }

    IEnumerator UpdatePath()
    {
        if (target == null)
        {
            yield return false;
        }

        seeker.StartPath(transform.position, target.position, OnPathComplete);

        yield return new WaitForSeconds(1f / rate);

        StartCoroutine(UpdatePath());
    }

    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void FixedUpdate()
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }

        if (target == null)
        {
            return;
        }

        if (path==null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            if (pathIsEnded)
            {
                return;
            }

            pathIsEnded = true;
            return;
        }

        pathIsEnded = false;

        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;

        rb.AddForce(dir, fMode);

        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);

        if (dist < nextWaypointDistance)
        {
            currentWaypoint++;
            return;
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingLeft = !m_FacingLeft;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = enemyGraphics.localScale;
        theScale.x *= -1;
        enemyGraphics.localScale = theScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == light)
        {
            health -= 10f;
            Debug.Log("hit");
        }

        else if (collision.gameObject == player)
        {
            player.GetComponent<PlayerHealth>().health -= 1f;
            Debug.Log(player.GetComponent<PlayerHealth>().health);
        }
    }
}
