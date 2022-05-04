using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAction : MonoBehaviour
{
    public float Spd = 5f; //��ĭ ���ǵ�
    public GameManager manager;

    public Transform target;
    public Vector3 moveVec;
    public Vector3 dirVec;

    Animator anim;
    Rigidbody2D rigid;
    float h;
    float v;
    bool isHorizonMove;
    bool moveOk;



    GameObject scanObject;
    GameObject determent;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target.parent = null;
        target.position = new Vector3(0.5f, 0.5f, 0);
    }

    void Update()
    {   
        //////////�̵� ����
        //manager.isAction ? 0 : ��ȭ �� �̵� ���� / ���� ������ �̵� ����
        moveOk = manager.isAction | manager.isDie;

        h = moveOk ? 0 : Input.GetAxisRaw("Horizontal");
        v = moveOk ? 0 : Input.GetAxisRaw("Vertical");

        bool hDown = moveOk ? false : Input.GetButtonDown("Horizontal");
        bool vDown = moveOk ? false : Input.GetButtonDown("Vertical");
        bool hUp = moveOk ? false : Input.GetButtonUp("Horizontal");
        bool vUp = moveOk ? false : Input.GetButtonUp("Vertical");

        if (hDown)
            isHorizonMove = true;
        else if (vDown)
            isHorizonMove = false;
        else if (hUp || vUp)
            isHorizonMove = h != 0;

        if (anim.GetInteger("hAxisRaw") != h) {
            anim.SetBool("isChange", true);
            anim.SetInteger("hAxisRaw", (int)h);
        } else if (anim.GetInteger("vAxisRaw") != v) {
            anim.SetBool("isChange", true);
            anim.SetInteger("vAxisRaw", (int)v);
        } else
            anim.SetBool("isChange", false);

        moveVec = isHorizonMove ? new Vector3(h, 0, 0) : new Vector3(0, v, 0);

        //Direction
        if (moveVec != new Vector3(0, 0, 0))
            dirVec = moveVec;

        //Ray  ///////////////�̺κ� �ϳ��� ��ġ��
        //��ȣ�ۿ� ������Ʈ
        Debug.DrawRay(rigid.position, dirVec * 1, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, 1, LayerMask.GetMask("Object"));

        if (rayHit.collider != null)
            scanObject = rayHit.collider.gameObject;
        else
            scanObject = null;

        //���ع�
        rayHit = Physics2D.Raycast(rigid.position, dirVec, 1, LayerMask.GetMask("Determent"));

        if (rayHit.collider != null)
            determent = rayHit.collider.gameObject;
        else
            determent = null;

        ////Scan Object
        if (Input.GetButtonDown("Jump") && scanObject != null) {
            manager.Action(scanObject);
        }



        //--��ĭ ������
        transform.position = Vector3.MoveTowards(transform.position, target.position, Spd * Time.deltaTime);

        if (determent == null && scanObject == null) {
            if (Vector3.Distance(transform.position, target.position) <= .05f)
            {
                if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
                    target.position += moveVec;
            }
        }
    }
}

