using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class Trigonometry
{
    public float normalizeAngle(float angle)
    {
        if (angle < 0)
            return 360 + angle;

        return angle;
    }
    public float Atan2(float x, float y)
    {
        return Mathf.Atan2(y, x);
    }

    public float getAngle(float x, float y)
    {
        float angle = Atan2(x, y) * Mathf.Rad2Deg;

        return normalizeAngle(angle);
    }

    public float getRawAngle(float x, float y)
    {
        return Atan2(x, y) * Mathf.Rad2Deg;
    }

    //You can send any vector
    public Vector2 rotateVector(Vector2 vector, float angle)
    {
        Vector2 direction = vector.normalized;

        float magnitude = vector.magnitude;

        //cos(a + b) = cos(a)cos(b) - sin(a)sin(b)
        //sin(a + b) = sin(a)cos(b) + cos(a)sin(b)

        angle = angle * Mathf.Deg2Rad;

        float X = Mathf.Cos(angle);
        float Y = Mathf.Sin(angle);

        float DirX = direction.x;
        float DirY = direction.y;

        float RotX = magnitude * (X * DirX - Y * DirY);
        float RotY = magnitude * (Y * DirX + X * DirY);

        return new Vector2(RotX,RotY);
    }

    public Vector2 rotateToClosestPerpendicular(Vector2 vector)
    {
        float angle = getAngle(vector.x, vector.y);

        float perpendicularAngle = 90 * Mathf.Round(angle / 90);

        int direction = (perpendicularAngle > angle) ? 1 : -1;

        vector = rotateVector(vector, Mathf.Abs(angle - perpendicularAngle) * direction);

        return vector;
    }

    public int getClosesstPerpendicular(Vector2 vector)
    {
        float angle = getAngle(vector.x, vector.y);

        return 90 * (int)Mathf.Round(angle / 90);
    }

}

