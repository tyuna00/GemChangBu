using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectMove : MonoBehaviour
{
    public GameManager gameManager;

    [SerializeField] float Hs;
    [SerializeField] float Vs;



    public string dingMsg;

    Vector3 target;
    Vector3 PrevPosition;

    void Awake()
    {
        PrevPosition = transform.position;
        target = PrevPosition + new Vector3(Vs, Hs, 0);
    }

    void Update()
    {
        if (!gameManager.isDie)
            transform.position = Vector3.MoveTowards(transform.position, target, 0.06f);
        if (transform.position == target)
            transform.position = PrevPosition;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player") {
            gameManager.GameOver(dingMsg);
        }
    }
}
