using UnityEngine;
using System.Collections;

public class Crawler : Agent {

    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform bulletPosition;
    private bool PlayerNearby;
    [SerializeField]
    private float HideTime;
    private float TimeHidden;
    private bool hiding;

    // Update is called once per frame
    void Update()
    {
        if (Alive())
        {
            UpdateHide();
            if (!(activeState is RangedAttackState))
                AgentAnimator.SetBool("Shooting", false);
            PlayerNearby = LookForPlayer();

            if (!hiding)
            {
                if (!Patrol && !PlayerNearby && !(activeState is IdleState))
                    SetState(new IdleState());
                else if (Patrol && !PlayerNearby && !(activeState is PatrolState))
                    SetState(new PatrolState());
            }

            if (PlayerNearby && !IsPlayerReachable() && !(activeState is HideState))
            {
                SetState(new HideState());
                hiding = true;
            }
            else if (IsPlayerReachable() && !(activeState is RangedAttackState))
            {
                SetState(new RangedAttackState());
                hiding = false;
            }
            activeState.Act();
        }
    }

    public void ShootBullet()
    {
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, bulletPosition.position, Quaternion.identity);
        if (this.isFacingRight())
            bullet.GetComponent<Bullet>().Initialize(Vector2.right);
        else
            bullet.GetComponent<Bullet>().Initialize(Vector2.left);
    }

    private void UpdateHide()
    {
        if (hiding)
        {
            TimeHidden += Time.deltaTime;
            if (TimeHidden > HideTime)
            {
                hiding = false;
                TimeHidden = 0;
            }
        }
    }

    protected override bool LookForPlayer()
    {
        if (Mathf.Abs(player.transform.position.x - AgentRigidbody.transform.position.x) < 15)
        {
            if (Mathf.Abs(player.transform.position.y - AgentRigidbody.transform.position.y) < 3)
                return true;
        }
        return false;
    }

}
