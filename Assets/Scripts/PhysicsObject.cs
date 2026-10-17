using UnityEngine;
using System.Collections.Generic;

public class PhysicsObject : MonoBehaviour
{
    public int ppu = 8; // Pixels per unit (block)

    [Space()]
    public float pX; // X position
    public float pY; // Y position

    [Space()]
    public float vX; // X velocity
    public float vY; // Y velocity

    [Space()]
    public float tv; // Terminal velocity

    [Space()]
    public float gravity; // Negative is down

    [Space()]
    public bool up; // Colliding up?
    public bool right; // Colliding right?
    public bool down; // Colliding down?
    public bool left; // Colliding left?

    [Space()]
    public Vector2 bcl; // Big hitbox lower bound
    public Vector2 bch; // Big hitbox upper bound
    public Vector2 lcl; // Small hitbox lower bound
    public Vector2 lch; // Small hitbox upper bound
    public bool disableCollision;

    [Space()]
    public Collider2D[] rup; // List of up collisions
    public Collider2D[] rright; // List of right collisions
    public Collider2D[] rdown; // List of down collisions
    public Collider2D[] rleft; // List of left collisions

    [Space()]
    public int maxShoveDepth; // Number of iterations to try to expel the physics object from inside a block
    public bool autoSimulate = true;
    public float motionMultiplier;
    public Vector2 Pos => new Vector2(Round(pX), Round(pY)) / ppu;

    void Awake()
    {
        pX = (long)(ppu * transform.position.x);
        pY = (long)(ppu * transform.position.y);
        motionMultiplier = 1f;
    }
    public void Simulate()
    {
        vY += gravity * motionMultiplier;
        if (Mathf.Abs(vY) > tv)
        {
            vY = tv * Mathf.Sign(vY);
        }
        Move();
        SetPos();
    }
    void FixedUpdate()
    {
        if (autoSimulate)
        {
            Simulate();
        }
    }
    void Move()
    {
        rup = GetCollision(new Vector2(lcl.x, lch.y) + Pos, new Vector2(lch.x, bch.y) + Pos, 2);
        rright = GetCollision(new Vector2(lch.x, lcl.y) + Pos, new Vector2(bch.x, lch.y) + Pos, 0);
        rdown = GetCollision(new Vector2(lcl.x, bcl.y) + Pos, new Vector2(lch.x, lcl.y) + Pos, 3);
        rleft = GetCollision(new Vector2(bcl.x, lcl.y) + Pos, new Vector2(lcl.x, lch.y) + Pos, 1);
        up = rup.Length > 0;
        right = rright.Length > 0;
        down = rdown.Length > 0;
        left = rleft.Length > 0;
        if (vX > 0)
        {
            float move = (vX * motionMultiplier) + pX - Round(pX);
            bool a = true;
            int mvc = 0;
            for (int i = 0; i <= move - 1; i += 1)
            {
                rright = GetCollision(new Vector2(lch.x, lcl.y) + Pos, new Vector2(bch.x, lch.y) + Pos, 0);
                if (rright.Length == 0 || disableCollision)
                {
                    pX += 1;
                    mvc += 1;
                }
                else
                {
                    a = false;
                    pX = 0.5f + Mathf.Round(pX - 0.5f);
                    vX = 0;
                    right = true;
                    i += 314159265;
                }
            }
            if (a)
            {
                pX += (vX * motionMultiplier) - mvc;
            }
        }
        if (vX < 0)
        {
            float move = (-vX * motionMultiplier) + pX - Round(pX);
            bool a = true;
            int mvc = 0;
            for (int i = 0; i <= move - 1; i += 1)
            {
                rleft = GetCollision(new Vector2(bcl.x, lcl.y) + Pos, new Vector2(lcl.x, lch.y) + Pos, 1);
                if (rleft.Length == 0 || disableCollision)
                {
                    pX -= 1;
                    mvc += 1;
                }
                else
                {
                    a = false;
                    pX = 0.5f + Mathf.Round(pX - 0.5f);
                    vX = 0;
                    left = true;
                    i += 314159265;
                }
            }
            if (a)
            {
                rleft = GetCollision(new Vector2(bcl.x, lcl.y) + Pos, new Vector2(lcl.x, lch.y) + Pos, 1);
                if (rleft.Length == 0)
                {
                    pX += (vX * motionMultiplier) + mvc;
                }
            }
        }
        if (vY > 0)
        {
            float move = (vY * motionMultiplier) + 0.5f + pY - Round(pY);
            bool a = true;
            int mvc = 0;
            for (int i = 0; i <= move - 1; i += 1)
            {
                rup = GetCollision(new Vector2(lcl.x, lch.y) + Pos, new Vector2(lch.x, bch.y) + Pos, 2);
                if (rup.Length == 0 || disableCollision)
                {
                    pY += 1;
                    mvc += 1;
                }
                else
                {
                    a = false;
                    pY = 0.5f + Mathf.Round(pY - 0.5f);
                    vY = 0;
                    up = true;
                    i += 314159265;
                }
            }
            if (a)
            {
                pY += (vY * motionMultiplier) - mvc;
            }
        }
        if (vY < 0)
        {
            float move = (-vY * motionMultiplier) - 0.5f + pY - Round(pY);
            bool a = true;
            int mvc = 0;
            for (int i = 0; i <= move - 1; i += 1)
            {
                rdown = GetCollision(new Vector2(lcl.x, bcl.y) + Pos, new Vector2(lch.x, lcl.y) + Pos, 3);
                if (rdown.Length == 0 || disableCollision)
                {
                    pY -= 1;
                    mvc += 1;
                }
                else
                {
                    a = false;
                    pY = 0.5f + Mathf.Round(pY - 0.5f);
                    vY = 0;
                    down = true;
                    i += 314159265;
                }
            }
            if (a)
            {
                rdown = GetCollision(new Vector2(lcl.x, bcl.y) + Pos, new Vector2(lch.x, lcl.y) + Pos, 3);
                if (rdown.Length == 0)
                {
                    pY += (vY * motionMultiplier) + mvc;
                }
            }
        }
        for (int i = 0; i < maxShoveDepth && !disableCollision; i++)
        {
            rup = GetCollision(new Vector2(lcl.x, lch.y) + Pos, new Vector2(lch.x, bch.y) + Pos, 2);
            rdown = GetCollision(new Vector2(lcl.x, bcl.y) + Pos, new Vector2(lch.x, lcl.y) + Pos, 3);
            rright = GetCollision(new Vector2(lch.x, lcl.y) + Pos, new Vector2(bch.x, lch.y) + Pos, 3);
            rleft = GetCollision(new Vector2(bcl.x, lcl.y) + Pos, new Vector2(lcl.x, lch.y) + Pos, 3);
            up = rup.Length > 0;
            down = rdown.Length > 0;
            right = rright.Length > 0;
            left = rleft.Length > 0;
            if (up && right && down)
            {
                pX -= 1;
            }
            if (right && down && left)
            {
                pY += 1;
            }
            if (down && left && up)
            {
                pX += 1;
            }
            if (left && up && right)
            {
                pY -= 1;
            }
        }
    }
    public int Round(float integer, bool roundDown = true)
    {
        if (roundDown)
        {
            return Mathf.FloorToInt(integer);
        }
        else
        {
            return Mathf.CeilToInt(integer);
        }
    }
    public void SetPos()
    {
        transform.position = new Vector3(Round(pX) / (float)ppu, Round(pY) / (float)ppu, transform.position.z);
    }
    public Collider2D[] GetCollision(Vector2 low, Vector2 high, int dir, string tag = "Untagged") // 0 right 1 left 2 up 3 down
    {
        Collider2D[] ret = Physics2D.OverlapBoxAll((low + high) / 2f, high - low, 0);
        List<Collider2D> cnd = new();
        for (int i = 0; i < ret.Length; i += 1)
        {
            if (ret[i].gameObject.CompareTag("Up") && dir == 3)
            {
                cnd.Add(ret[i]);
            }
            else if (ret[i].gameObject.CompareTag(tag))
            {
                cnd.Add(ret[i]);
            }
        }
        ret = new Collider2D[cnd.Count];
        for (int i = 0; i < ret.Length; i += 1)
        {
            ret[i] = cnd[i];
        }
        return ret;
    }
}