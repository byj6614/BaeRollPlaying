using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacker : MonoBehaviour
{

    [SerializeField] bool debug;
    [SerializeField] int damage;
    [SerializeField] float range;
    [SerializeField,Range(0,360)] float angle;

    private Animator ani;
    private float cosResult;
    private void Awake()
    {
        cosResult = Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad);
        ani = GetComponent<Animator>();
    }

    public void Attack()
    {
        ani.SetTrigger("Attack");
    }

    private void OnAttack(InputValue value)
    {
        Attack();
    }

    public void AttackTiming()
    {
        //1.범위 안에 있는지
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);
        foreach(Collider collider in colliders)
        {
            //2.내적을 이용한 각도 안에 있는지
            Vector3 dirTarget = (collider.transform.position - transform.position).normalized;
            if (Vector3.Dot(transform.forward, dirTarget) < cosResult)
                continue;

            IHittable hittable = collider.GetComponent<IHittable>();
            hittable?.TakeHit(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(!debug) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);

        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + angle * 0.5f);
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - angle * 0.5f);
        Debug.DrawRay(transform.position, rightDir*range, Color.yellow); ;
        Debug.DrawRay(transform.position,leftDir*range,Color.yellow);
    }

    private Vector3 AngleToDir(float angle)
    {
        float radian = angle*Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian),0,Mathf.Cos(radian));
    }
}
