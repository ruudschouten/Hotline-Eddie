using UnityEngine;

namespace Characters
{
    public class MOONMOON : Boss
    {
        protected override void DetermineAction()
        {
            // Get the distance between the player and the boss.
            Distance = Vector3.Distance(transform.position, player.transform.position);
            // Check if player is outside of melee distance.
            if (Distance >= meleeDistance)
            {
                // Check if the ranged attack is ready.
                if (!IsRangeRecharging)
                {
                    // Check if the player is between the min and max range distances
                    if (Distance >= minRangedDistance && Distance <= maxRangedDistance)
                    {
                        Shoot();

                        IsRangeRecharging = true;
                    }
                    else
                    {
                        Move();
                    }
                }
                else
                {
                    // If the player is still recharging, move towards the player, hoping to get into melee range.
                    Move();
                }
            }
            else
            {
                if (!IsMeleeRecharging)
                {
                    MeleeAttack();
                    IsMeleeRecharging = true;
                }
            }
        }
    }
}