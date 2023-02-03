using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RootMovement : MonoBehaviour
{
    public LineRenderer line;

    private int count;

    private void Start()
    {
        count = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) Move(Vector3.left);
        else if (Input.GetKeyDown(KeyCode.D)) Move(Vector3.right);
        else if (Input.GetKeyDown(KeyCode.S)) Move(Vector3.down);
        else if (Input.GetKeyDown(KeyCode.W)) Move(Vector3.up);
    }

    private void Move(Vector3 direction)
    {

        count++;
        line.positionCount++;

        line.SetPosition(count, line.GetPosition(count - 1) + direction);
    }
}
