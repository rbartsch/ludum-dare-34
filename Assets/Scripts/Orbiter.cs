using UnityEngine;
using System.Collections;

public class Orbiter : MonoBehaviour
{
  public float rotateSpeed;

  void FixedUpdate ()
  {
    transform.Rotate( 0, 0, rotateSpeed * Time.deltaTime );
  }
}