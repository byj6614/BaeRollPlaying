using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class HomeMover : MonoBehaviour
{
    [SerializeField] float runSpeed;//�ִ�ӵ�
    [SerializeField] float walkSpeed;//������ �ӵ�
    [SerializeField] float jumpSpeed;//���� ����

    private CharacterController control;
    private Animator ani;
    private Vector3 moveDir;//�����̴� ����
    private float curSpeed;//���� �����̴� �ӵ�
    private float ySpeed;//y����⿡�� �������� �ӵ�
    private bool walk;//���� �Ȱ� �ִ°� �ƴѰ��� bool�� ǥ��

    private void Awake()
    {
        control = GetComponent<CharacterController>();
        ani = GetComponent<Animator>();
    }
    private void Update()
    {
        Move();
        Fall();
    }
    private void Move()
    {
        if(moveDir.magnitude==0)//�������� ������ ��
        {
            curSpeed = Mathf.Lerp(curSpeed, 0, 0.1f);//���� �ӵ��� ���밪���� 0�� �ٴٸ��� �Ѵ�.
            ani.SetFloat("HomeMove", curSpeed);//HomeMove�� ����ӵ��� �����ش�.
            return;//return�� �ٷ� ���־ �ٶ󺸴� ��ġ�� �ʱ�ȭ�� �ȵǰ� ���ش�.
        }

        //ī�޶� ���� ������ ���� �ǵ��� ���ִ� ���
        Vector3 forwardVec=new Vector3(Camera.main.transform.forward.x,0,Camera.main.transform.forward.z).normalized;
        Vector3 rigthVec = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;

        if(walk)
        {
            curSpeed = Mathf.Lerp(curSpeed, walkSpeed, 0.1f);//����ӵ��� �ȴ� ���ǵ���� Lerp������ ���簣��.
        }
        else
        {
            curSpeed = Mathf.Lerp(curSpeed, runSpeed, 0.1f);//����ӵ��� �ٴ� ���ǵ���� Lerp������ ���簣��.
        }
        control.Move(forwardVec * moveDir.z * runSpeed * Time.deltaTime);
        control.Move(rigthVec*moveDir.x*runSpeed * Time.deltaTime);//��������� ī�޶� ���� �ٶ󺸰� ���ִ� ����̴�.
        ani.SetFloat("HomeMove", curSpeed);//�Ȱų� �۶��� �ӵ��� HomeMove�� ����ǵ��� ���ش�.
        Quaternion lookRotation = Quaternion.LookRotation(forwardVec * moveDir.z + rigthVec * moveDir.x);
        //ĳ���Ͱ� �ٶ󺸴� ��ġ�� ���ϱ����ؼ� Quaternion���� ����
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.2f);
        //Lerp�� ���ؼ� ���ڿ������� ������ �ͺ��� �ڿ������� ���ư����� ����
    }
    private void OnMove(InputValue value)
    {
        moveDir.x = value.Get<Vector2>().x;
        moveDir.z = value.Get<Vector2>().y;//y������ 3D������ ��������� ���⿡ moveDir.z�������� �־�� �Ѵ�.
    }
    private void Fall()
    {
        ySpeed += Physics.gravity.y * Time.deltaTime;//y�� �������� �߷°��� ySpeed�� �־��ش�.

        if(control.isGrounded&&ySpeed<0)//�ٴڿ��ְų� yspeed�� 0�Ʒ��� �������� ��� ������ ������ ���ٴ� ��
            ySpeed = 0;//0���� �������ش�.
        control.Move(Vector3.up*ySpeed*Time.deltaTime);
    }
    private void Jump()
    {
        ySpeed = jumpSpeed;
    }
    private void OnJump(InputValue value)
    {
        Jump();
    }
    private void OnWalk(InputValue value)//�ȱ�Ű�� ��������� true �ƴҰ�� flase
    {
        walk = value.isPressed;
    }
}
