using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Body : MonoBehaviour
{
  public Vector3 initialVelocity;
  public float radius = 1;
  public float mass = 1;
  public bool stationary = false;

  Rigidbody rb;
  Vector3 currentVelocity;

  private void Awake() {
    currentVelocity = initialVelocity;
    rb = this.GetComponent<Rigidbody>();
    mass = rb.mass;
  }

  public void UpdateVelocity (Body[] bodies, float G) {
    foreach (var otherBody in bodies) {
      if (otherBody != this && !stationary) {
        Rigidbody otherRB = otherBody.GetComponent<Rigidbody>();
        float sqrDist = (otherRB.position - rb.position).sqrMagnitude;

        Vector3 forceDir = (otherRB.position - rb.position).normalized;
        Vector3 force = (forceDir * G * mass * otherBody.mass) / sqrDist;
        Vector3 acceleration = force / mass;

        currentVelocity += acceleration * Time.deltaTime;
      }
    }
  }

  public void UpdatePosition () {
    rb.position += currentVelocity * Time.deltaTime;
  }

  private void OnDrawGizmos() {
    Gizmos.color = Color.green;
    Gizmos.DrawSphere(transform.position, 1 / Mathf.Pow(radius, 2));
  }

  private void OnValidate() {
    transform.localScale = radius * Vector3.one;
  }
}