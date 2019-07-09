using UnityEngine;

public class PlayerControl : PlayerBehaviour
{

    void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        //Rotina de verificação de inputs. Se estiver seguindo em um sentido, não permite andar para o sentido contrário, evitando o auto devoramento da cobra. Passa o sentido para o PlayerBehaviour
        if (inputY > 0 && op != Direction.DOWN)
        {
            op = Direction.UP;
        }
        else
        {
            if (inputY < 0 && op != Direction.UP)
            {
                op = Direction.DOWN;
            }
            else
            {
                if (inputX < 0 && op != Direction.RIGHT)
                {
                    op = Direction.LEFT;
                }
                else
                {
                    if (inputX > 0 && op != Direction.LEFT)
                    {
                        op = Direction.RIGHT;
                    }
                }
            }
        }
        SwitchDirection();
    }
}
