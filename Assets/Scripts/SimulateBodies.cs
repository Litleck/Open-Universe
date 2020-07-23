using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulateBodies : MonoBehaviour
{
  public float G = 6.674f;

  Body[] bodies;

  private void Awake() {
    bodies = FindObjectsOfType<Body>();
  }

  private void FixedUpdate() {
    for (int i = 0; i < bodies.Length; i++) {
      bodies[i].UpdateVelocity(bodies, G);
    }
    for (int i = 0; i < bodies.Length; i++) {
      bodies[i].UpdatePosition();
    }
  }
}
