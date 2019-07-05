using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : PlayerBehaviour
{

    void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        if (inputY > 0)
        {
            op = Direction.UP;
        }
        else
        {
            if (inputY < 0)
            {
                op = Direction.DOWN;
            }
            else
            {
                if (inputX < 0)
                {
                    op = Direction.LEFT;
                }
                else
                {
                    if (inputX > 0)
                    {
                        op = Direction.RIGHT;
                    }
                }
            }
        }
        SwitchDirection();
    }
}
