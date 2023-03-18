using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class EnemyController : MonoBehaviour {
    public List<Transform> points;

    public int nextID = 0; // if it's 0, enemy will go to 0, then it's 1 and so on
    private int idChangeValue = 1; // +1 or -1

    public float speed = 2f;

    private int decayAmountOfHealth = 25;

    // setup
    private void Reset() {
        GetComponent<BoxCollider2D>().isTrigger = true;

        GameObject root = new GameObject(name + "Root");
        root.transform.position = transform.position;
        transform.SetParent(root.transform);
        GameObject wayPoints = new GameObject("WayPoints");
        wayPoints.transform.SetParent(root.transform);
        wayPoints.transform.position = root.transform.position;

        GameObject p1 = new GameObject("Point1");
        p1.transform.SetParent(wayPoints.transform);
        p1.transform.position = root.transform.position;

        GameObject p2 = new GameObject("Point2");
        p2.transform.SetParent(wayPoints.transform);
        p2.transform.position = root.transform.position;

        points = new List<Transform>();
        points.Add(p1.transform);
        points.Add(p2.transform);
    }

    private void Update() {
        MoveToNextPoint();
    }

    public void MoveToNextPoint() {
        Transform goalPoint = points[nextID];

        if (goalPoint.transform.position.x > transform.position.x) {
            transform.localScale = new Vector3(-1, 1, 1);
        } else {
            transform.localScale = new Vector3(1, 1, 1);
        }

        transform.position = Vector2.MoveTowards(transform.position, goalPoint.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, goalPoint.position) < 0.5f) {
            // 2 points (0, 1) nextId == points.count(2) - 1 (bc index)
            // check if we're at the end of the line => -1
            if (nextID == points.Count - 1) { // means we're at the end
                idChangeValue = -1;
            }

            // at the start => +1
            if (nextID == 0) {
                idChangeValue = 1;
            }

            nextID += idChangeValue;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            // Debug.Log($"{name} Triggered");

            FindObjectOfType<HealthBar>().LoseHealth(decayAmountOfHealth);
        }
    }

    public void Hurt() {
        this.gameObject.SetActive(false);
    }
}
