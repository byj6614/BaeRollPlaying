using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class HomeMover : MonoBehaviour
{
    [SerializeField] bool debug;
    [SerializeField] float runSpeed;//최대속도
    [SerializeField] float walkSpeed;//걸을때 속도
    [SerializeField] float jumpSpeed;//점프 위력
    [SerializeField] float walkStepRange;
    [SerializeField] float runStepRange;

    private CharacterController control;
    private Animator ani;
    private Vector3 moveDir;//움직이는 방향
    private float curSpeed;//현재 움직이는 속도
    private float ySpeed;//y축방향에서 내려오는 속도
    private bool walk;//현재 걷고 있는가 아닌가를 bool로 표현

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

    float lastStepTime = 0.5f;
    private void Move()
    {
        if(moveDir.magnitude==0)//움직임이 멈췄을 때
        {
            curSpeed = Mathf.Lerp(curSpeed, 0, 0.1f);//현재 속도를 절대값으로 0에 다다르게 한다.
            ani.SetFloat("HomeMove", curSpeed);//HomeMove를 현재속도에 맞춰준다.
            return;//return을 바로 해주어서 바라보는 위치가 초기화가 안되게 해준다.
        }

        //카메라가 보는 방향이 앞이 되도록 해주는 방법
        Vector3 forwardVec=new Vector3(Camera.main.transform.forward.x,0,Camera.main.transform.forward.z).normalized;
        Vector3 rigthVec = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;

        if(walk)
        {
            curSpeed = Mathf.Lerp(curSpeed, walkSpeed, 0.1f);//현재속도를 걷는 스피드까지 Lerp식으로 맞춰간다.
        }
        else
        {
            curSpeed = Mathf.Lerp(curSpeed, runSpeed, 0.1f);//현재속도를 뛰는 스피드까지 Lerp식으로 맞춰간다.
        }
        control.Move(forwardVec * moveDir.z * runSpeed * Time.deltaTime);
        control.Move(rigthVec*moveDir.x*runSpeed * Time.deltaTime);//여기까지가 카메라가 앞을 바라보게 해주는 방법이다.
        ani.SetFloat("HomeMove", curSpeed);//걷거나 뛸때의 속도가 HomeMove에 적용되도록 해준다.
        Quaternion lookRotation = Quaternion.LookRotation(forwardVec * moveDir.z + rigthVec * moveDir.x);
        //캐릭터가 바라보는 위치를 구하기위해서 Quaternion으로 교정
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.2f);
        //Lerp를 통해서 부자연스럽게 돌리는 것보단 자연스럽게 돌아가도록 설정


        lastStepTime -= Time.deltaTime;
        if (lastStepTime < 0)
        {
            lastStepTime = 0.5f;
            GenerateFootStepSound();
        }
    }
    private void GenerateFootStepSound()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, walk ? walkStepRange : runStepRange);
        foreach (Collider collider in colliders)
        {
            IHlisten listenable = collider.GetComponent<IHlisten>();
            listenable?.HListen(transform);
        }
    }
    private void OnMove(InputValue value)
    {
        moveDir.x = value.Get<Vector2>().x;
        moveDir.z = value.Get<Vector2>().y;//y방향은 3D에서는 점프방향과 같기에 moveDir.z방향으로 주어야 한다.
    }
    private void Fall()
    {
        ySpeed += Physics.gravity.y * Time.deltaTime;//y축 방향으로 중력값을 ySpeed에 넣어준다.

        if(control.isGrounded&&ySpeed<0)//바닥에있거나 yspeed가 0아래로 떨어졌을 경우 내려갈 공간이 없다는 뜻
            ySpeed = 0;//0으로 고정해준다.
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
    private void OnWalk(InputValue value)//걷기키가 눌렸을경우 true 아닐경우 flase
    {
        walk = value.isPressed;
    }
    private void OnDrawGizmosSelected()
    {

        if (!debug)
            return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, walkStepRange);
        Gizmos.DrawWireSphere(transform.position, runStepRange);
    }
}
