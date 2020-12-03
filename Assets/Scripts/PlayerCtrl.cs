using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour {

    public float speed = 5f;
    public Rigidbody2D rb;
    public Camera camera;

    Vector2 mouse;
    Vector2 movement;
    
    void Update() {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mouse = camera.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate() {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

        Vector2 lookDirection = mouse - rb.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        rb.rotation = angle;
    }
}
