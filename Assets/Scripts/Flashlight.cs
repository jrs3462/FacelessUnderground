using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public float rate = 0;
    public float damage = 10;
    public LayerMask hits;

    float timeToFire = 0;
    Transform firePoint;
    public GameObject light;

    private float lightCD;
    private bool attack;

    // Start is called before the first frame update
    void Awake()
    {
        firePoint = transform.Find("Light");
        light = GameObject.Find("On");
        light.active = false;
        attack = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (rate == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //TurnOn();
                light.active = true;
                float cd = 1f;
                lightCD = Time.time + cd;
            }
        }

        else
        {
            if (Input.GetMouseButtonDown(0) && Time.time > timeToFire)
            {
                timeToFire = Time.time + 2f;
                light.active = true;
                float cd = 1f;
                lightCD = Time.time + cd;
                //TurnOn();
            }
        }

        if (Time.time > lightCD)
        {

            light.active = false;
        }
    }

    void TurnOn()
    {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, 2, hits);
    }
}
