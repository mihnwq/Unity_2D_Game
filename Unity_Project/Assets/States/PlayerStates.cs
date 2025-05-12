using UnityEngine;

public static class PlayerStates
{
    public static bool IsAttacking;

    public static bool aquiredSword;

    public static bool hadDied;

    public static int playerAmmo;

    public static int playerMaxAmmo;

    /// DIRECTION VECTOR IS FOR TESTING PURPOSES ONLY

    public static Vector2 direction = Vector2.zero;

    public static void setDir(Vector2 dir)
    {
        direction = dir;
    }

    public static void setAttack(bool state)
    {
        IsAttacking = state;
    }

    public static bool getAttack()
    {
        return IsAttacking;
    }

    public enum wacthDirection
    {
        up, right, left, down
    }

    public static wacthDirection dir;

    public static void setWatchDirection(string direction)
    {
       switch (direction)
        {
            case "up":
                dir = wacthDirection.up;
                break;
            case "down":
                dir = wacthDirection.down;
                break;
            case "left":
                dir = wacthDirection.left;
                break;
            case "right":
                dir = wacthDirection.right;
                break;
        }
    }

    public static int getWatchDirection()
    {
        return (int)dir;
    }

    public static int vertical, horizontal;

    public static void getAxes(int v, int h)
    {
        vertical = v;
        horizontal = h;
    }

    public static void returnAxes(ref int v,ref int h)
    {
        v = vertical;

        h = horizontal;
    }

    public static int getHorizontal()
    {
        return horizontal;
    }

    public static int getVertical()
    {
        return vertical;
    }

    public static bool isMoving()
    {
        return horizontal != 0 || vertical != 0;
    }

    public static void GetLivelihood(bool alive)
    {
        hadDied = !alive;
    }

    public static bool CanUseSword()
    {
        return aquiredSword;
    }

    public static void SetAmmountAmunition(int amount)
    {
        playerAmmo = amount;
    }

    public static void SetMaxAmunition(int amount)
    {
        playerMaxAmmo = amount;
    }
    
    public static bool HasMaxAmmo()
    {
        return playerAmmo == playerMaxAmmo;
    }

    public static int GetPlayerAmmo()
    {
        return playerAmmo;
    }

    public static bool HasAmmoLeft()
    {
        return playerAmmo > 0;
    }
}
