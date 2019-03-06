using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 10f;
    public float health = 100f;
    private Transform target;
    private int wavePointIndex = 0;


    // Start is called before the first frame update
    //starting target on AI path is point[0]
    void Start()
    {
        target = PathPoint.points[0];
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dire = target.position - transform.position;
        transform.Translate(dire.normalized * speed * Time.deltaTime , Space.World);
        if (Vector3.Distance(transform.position , target.position) <= 0.4f)
        {
            GetNextWayPoint();
        }

    }

    private void GetNextWayPoint()
    {
        if (wavePointIndex >= PathPoint.points.Length -1 ) {
            Destroy(gameObject);
            return;
        }
        wavePointIndex++;
        target = PathPoint.points[wavePointIndex];
    }
}
