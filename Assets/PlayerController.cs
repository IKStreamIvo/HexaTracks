using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Tile currentTile;

    RobotAnimator animations;

	// Use this for initialization
	void Start () {
        animations = GetComponent<RobotAnimator>();
	}

    bool moving;
    Tile targetTile;
    bool rotatingRight;
    bool rotatingLeft;
    float startRot;
    public float moveSpeed = 5f;

	void Update () {
        if (Input.GetKey(KeyCode.W) && !moving && !rotatingLeft && !rotatingRight)
        {
            int index = (int)((transform.rotation.eulerAngles.y - 30f) * (0 - 5) / (30f - 330f) + 0);
            Dictionary<int, Tile> neighbours = World.instance.TileNeighboursDict(currentTile);
            if (neighbours.ContainsKey(index)) {
                targetTile = neighbours[index];
                moving = true;
                animations.StartMove(true);
            }
        }
        
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.A))
        {
            if (!rotatingLeft && !rotatingRight && !moving)
            {
                //LEFT
                rotatingLeft = true;
                rotatingRight = false;
                startRot = transform.rotation.eulerAngles.y;
                if (startRot - 60f <= 0f)
                {
                    startRot += 360f;
                }
                animations.StartRotate(false);
            }
        }
        else if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.D) && !rotatingLeft && !rotatingRight && !moving)
        {
            if (!rotatingLeft && !rotatingRight && !moving)
            {
                //RIGHT
                rotatingLeft = false;
                rotatingRight = true;
                startRot = transform.rotation.eulerAngles.y;
                if (startRot + 60f > 360f)
                {
                    startRot -= 360f;
                }
                Debug.Log(startRot + " to " + (startRot + 60f) + "\n" + transform.rotation.eulerAngles.y);

                animations.StartRotate(true);
            }
        }

        if (moving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetTile.transform.position, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetTile.transform.position) <= .1f)
            {
                currentTile = targetTile;
                transform.position = currentTile.transform.position;
                animations.Stop();
                moving = false;

                //false = dead
                if (World.instance.ShowTile(currentTile))
                {

                }
                else
                {
                    Debug.LogError("Game over");
                }
            }
        }

        if (rotatingRight && !moving)
        {
            transform.Rotate(new Vector3(0, rotSpeed * Time.deltaTime, 0));
            float compVal = transform.rotation.eulerAngles.y;
            if (compVal + 60f > 360f && (startRot + 60f) != 330f)
            {
                compVal -= 360f;
            }
            if (startRot + 60f <= compVal)
            {
                Vector3 rot = transform.rotation.eulerAngles;
                rot.y = startRot + 60f;
                transform.rotation = Quaternion.Euler(rot);
                rotatingRight = false;
                animations.Stop();
            }
        }
        else if (rotatingLeft && !moving)
        {
            transform.Rotate(new Vector3(0, -rotSpeed * Time.deltaTime, 0));
            float compVal = transform.rotation.eulerAngles.y;
            if (compVal - 60f <= 0f && (startRot - 60f) != 30f)
            {
                compVal += 360f;
            }

            if (startRot - 60f >= compVal)
            {
                Vector3 rot = transform.rotation.eulerAngles;
                rot.y = startRot - 60f;
                transform.rotation = Quaternion.Euler(rot);
                rotatingLeft = false;
                animations.Stop();
            }
        }
    }

    public float rotSpeed = 2f;
    private void LateUpdate()
    {
        
    }
}
