using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public GameObject gameOver;

    public Animator anim;

    private string currentAnimation;

    private string previousAnimation;

    private SpriteRenderer playerSprite;

    private int vertical, horizontal;

    private int animationNumber;

    private enum lastState
    {
        UP, DOWN, HORIZONTAL
    }

    lastState state;

    public void Start()
    {
        currentAnimation = "idle_down";
        previousAnimation = null;
        state = lastState.DOWN;
        playerSprite = GetComponent<SpriteRenderer>();
    }

    private void StopAnimating()
    {
        anim.speed = 0f;
    }

    private bool shouldStopAnimating = false;

    public void Update()
    {
        PlayerStates.returnAxes(ref vertical, ref horizontal);

        if(!PlayerStates.hadDied)
        {
            if (!GameStates.isGamePaused)
            {
                HandleAnimation();
                ChangeAnimation();
            }
        }
        else
        {
            if(!shouldStopAnimating)
            {
                currentAnimation = "dying";
                ChangeAnimation();
            }
             

            Invoke(nameof(InitiateDyingProtocol), 0.8f);
        }
        

    }

    private void InitiateDyingProtocol()
    {
        shouldStopAnimating = true;
        StopAnimating();
        gameOver.SetActive(true);
    }

    void ChangeAnimation()
    {

        if (currentAnimation != previousAnimation)
            anim.Play(currentAnimation);
    }

    

    void HandleAnimation()
    {

        previousAnimation = currentAnimation;

        if (!PlayerStates.getAttack())
        {
            if (horizontal != 0)
            {
                state = lastState.HORIZONTAL;

                if (horizontal > 0)
                    playerSprite.flipX = false;
                else playerSprite.flipX = true;

                currentAnimation = "walk_right";
            }
            else if (vertical != 0)
            {
                if (vertical > 0)
                {
                    state = lastState.UP;
                    currentAnimation = "walk_up";
                }
                else
                {
                    state = lastState.DOWN;
                    currentAnimation = "walk_down";
                }

            }
            else
            {
                switch (state)
                {
                    case lastState.HORIZONTAL:
                        currentAnimation = "idle_right";
                        break;
                    case lastState.UP:
                        currentAnimation = "idle_up";
                        break;
                    case lastState.DOWN:
                        currentAnimation = "idle_down";
                        break;
                }
            }


        }else
        {
            //0 - up, 1 - right , 2 - left, 3 - down.
            animationNumber = PlayerStates.getWatchDirection();

            if(animationNumber == 1 || animationNumber == 2)
            {
                switch (animationNumber)
                {
                    case 2:
                        playerSprite.flipX = true;
                        break;
                    case 1:
                        playerSprite.flipX = false;
                        break;
                }
                currentAnimation = "attack_right";

                state = lastState.HORIZONTAL;
            }else
            {
                if(animationNumber == 0)
                {
                    currentAnimation = "attack_up";
                    state = lastState.UP;
                }else
                {
                    currentAnimation = "attack_down";
                    state = lastState.DOWN;
                }
            }
        }


    }

    public static PlayerAnimations instance;

    private void Awake()
    {
        instance = this;
    }

    public void SetAllBackToDefaults()
    {
        currentAnimation = "idle_down";
    }
}

