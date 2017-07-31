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
    public float rotSpeed = 2f;
    public float moveSpeed = 5f;

	void Update () {
        if (!World.instance.UIOpen)
        {
            if (Input.GetAxis("Vertical") > .2f && !moving && !rotatingLeft && !rotatingRight && !World.instance.dying)
            {
                int index = (int)((transform.rotation.eulerAngles.y - 30f) * (0 - 5) / (30f - 330f) + 0);
                Dictionary<int, Tile> neighbours = World.instance.TileNeighboursDict(currentTile);
                if (neighbours.ContainsKey(index))
                {
                    if (!neighbours[index].blocked)
                    {
                        AudioPlayer.instance.RobotMove();
                        targetTile = neighbours[index];
                        moving = true;
                        animations.StartMove(true);
                    }
                    else
                    {
                        World.instance.LightUpTile(neighbours[index]);
                    }
                }
            }
            //Debug.Log(Input.GetAxis("Horizontal"));
            if (Input.GetAxis("Horizontal") <= -.2f && !rotatingLeft && !rotatingRight && !moving && !World.instance.dying)
            {
                if (!rotatingLeft && !rotatingRight && !moving)
                {
                    AudioPlayer.instance.RobotTurn();
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
            else if (Input.GetAxis("Horizontal") >= .2f && !rotatingLeft && !rotatingRight && !moving && !World.instance.dying)
            {
                if (!rotatingLeft && !rotatingRight && !moving)
                {
                    AudioPlayer.instance.RobotTurn();
                    //RIGHT
                    rotatingLeft = false;
                    rotatingRight = true;
                    startRot = transform.rotation.eulerAngles.y;
                    if (startRot + 60f > 360f)
                    {
                        startRot -= 360f;
                    }

                    animations.StartRotate(true);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space) && !rotatingLeft && !rotatingRight && !moving && !World.instance.dying)
            {
                int index = (int)((transform.rotation.eulerAngles.y - 30f) * (0 - 5) / (30f - 330f) + 0);
                Dictionary<int, Tile> neighbours = World.instance.TileNeighboursDict(currentTile);
                Debug.Log("Beep" + neighbours.ContainsKey(index));
                if (neighbours.ContainsKey(index))
                {
                    Tile target = neighbours[index];
                    World.instance.LightUpTile(target);
                }
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

                World.instance.StepTile(currentTile);
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
}
