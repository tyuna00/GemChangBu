using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPush : MonoBehaviour
{
    public GameObject player;

    public bool isMove;

    Vector3 position;

    Vector3 target;
    PlayerAction playerAction;
    Transform trans;
    int numObjectHit;


    void Awake()
    {
        playerAction = player.GetComponent<PlayerAction>();
        trans = GetComponent<Transform>();

        position = transform.position;
        target = transform.position;
    }

   IEnumerator Move()
    {
        while (Vector3.Distance(transform.position, target) == 0f)
        {
            Vector3 velo = Vector3.zero;
            if (Vector3.Distance(transform.position, target) > 0.5f)
            {
                transform.position = Vector3.SmoothDamp(transform.position, target, ref velo, 0.03f);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, target, 0.07f);
            }
        }
        yield return null;
    }

    public void Push()
    {
        if (numObjectHit < 2)
        {
            target += playerAction.moveVec;
            StartCoroutine(Move());
        }
        else
            ResetObject();
    }

    public void ResetObject()
    {
        transform.position = position;
        target = position;
    }

    void FixedUpdate()
    {
        Debug.DrawRay(trans.position, playerAction.dirVec, new Color(0, 1, 0));

        int mask = (1 << 8) + (1 << 9);

        RaycastHit2D[] rayHit = Physics2D.RaycastAll(trans.position, playerAction.dirVec, 1, mask);
        numObjectHit = rayHit.Length;
    }
}
