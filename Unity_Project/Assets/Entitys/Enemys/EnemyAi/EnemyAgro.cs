using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine; 

public class EnemyAgro
{

    LayerMask solid;

    LayerMask player;

    Vector2 origin;

    Vector2 direction;

    public void setOrigin(Vector2 origin)
    {
        this.origin = origin;
    }

    public void setDirection(Vector2 direction)
    {
        this.direction = direction.normalized;
    }

    float distance;

    public EnemyAgro(float distance, LayerMask solid, LayerMask player)
    {
        this.distance = distance;
        this.solid = solid;
        this.player = player;
    }

    /*  public bool canSeePlayer()
      {
          RaycastHit2D hitSolid = Physics2D.Raycast(origin, direction, distance, solid);

          RaycastHit2D hitPlayer = Physics2D.Raycast(origin, direction, distance, player);

          Debug.DrawRay(origin, direction * distance, Color.green);

          return (!hitSolid && hitPlayer);
      }*/

    /*  public bool CanSeePlayer()
      {
          int rayCount = 10; // Number of rays to cast
          float parabolaHeight = 1f; // How "tall" the parabola is

          for (int i = 0; i < rayCount; i++)
          {
              // t goes from -1 to 1 across the rayCount
              float t = (i / (float)(rayCount - 1)) * 2f - 1f;

              // Calculate parabolic offset: y = -a*x^2 + c (c = parabolaHeight)
              float offsetY = -parabolaHeight * t * t + parabolaHeight;

              // Construct the ray direction
              Vector2 rayDir = direction + new Vector2(0, offsetY);
              rayDir.Normalize();

              RaycastHit2D hitSolid = Physics2D.Raycast(origin, rayDir, distance, solid);
              RaycastHit2D hitPlayer = Physics2D.Raycast(origin, rayDir, distance, player);

              Debug.DrawRay(origin, rayDir * distance, Color.green);

              if (!hitSolid && hitPlayer)
              {
                  return true;
              }
          }

          return false;
      }*/

    public bool CanSeePlayer()
    {
        int rayCount = 10; 
        float spreadAngle = 30f;

        float halfSpread = spreadAngle / 2f;
        Vector2 forwardDir = direction.normalized;

        for (int i = 0; i < rayCount; i++)
        {
            
            float t = i / (float)(rayCount - 1); 
            float angle = Mathf.Lerp(-halfSpread, halfSpread, t);

            Vector2 rayDir = Quaternion.Euler(0, 0, angle) * forwardDir;

            RaycastHit2D hitSolid = Physics2D.Raycast(origin, rayDir, distance, solid);
            RaycastHit2D hitPlayer = Physics2D.Raycast(origin, rayDir, distance, player);

            Debug.DrawRay(origin, rayDir * distance, Color.green);

            if (!hitSolid && hitPlayer)
            {
                return true;
            }
        }

        return false;
    }







}
