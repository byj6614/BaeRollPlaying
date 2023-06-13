using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HomeAttack : MonoBehaviour
{
    [SerializeField] bool debug;
    [SerializeField] int damage;
    [SerializeField] float range;
    [SerializeField, Range(0f, 360f)] float angle;

    private Animator ani;

    private void Awake()
    {
        ani = GetComponent<Animator>();
    }

    public void Attack()
    {
        ani.SetTrigger("HomeAttack");
    }

    private void OnAttack(InputValue value)
    {
        Attack();
    }

    public void HattackTiming()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);
        foreach (Collider collider in colliders)
        {
            Vector3 dirTarget = (collider.transform.position - transform.position).normalized;
            if (Vector3.Dot(transform.forward, dirTarget) < Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad))
                continue;

            IAttackAble hAttack=collider.GetComponent<IAttackAble>();
            hAttack?.AttackReceive(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!debug) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);

        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + angle * 0.5f);
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - angle * 0.5f);
        Debug.DrawRay(transform.position, rightDir * range, Color.yellow); ;
        Debug.DrawRay(transform.position, leftDir * range, Color.yellow);
    }

    private Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
    }
}
