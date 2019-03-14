using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public Transform target;
    public Transform[] targets;
    public float speed = 1;
    
    int nextIndex;
    bool isMoving = false;

    void Start() {
        transform.position = targets[0].position;
        nextIndex = 1;
    }
    void Update() {
        HandleInput();
        HandleMovement();
    }
    
    void HandleInput() {
        if(Input.GetButtonDown("Fire1")) isMoving = !isMoving;
    }
    
    void HandleMovement() {
        if(!isMoving) return;
        float distance = Vector3.Distance(transform.position, targets[nextIndex].position);
        if(distance>0) {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targets[nextIndex].position, step);
        } else {
            nextIndex++;
            nextIndex = nextIndex % targets.Length;
            isMoving = false;
        }
    }
}
