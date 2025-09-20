using Godot;
using System;
using MetroidvaniaProject.Scripts.Hero;

public class HeroStateAttack : Timer, IHeroState
{
    private bool Initialized = false;
    
    private bool AttackInProgress =false;
    
    private bool AttackTimerHasTimedOut = false;        // if the timer has timed out (attack finished)

    private HeroStateMachine Hero;
    public IHeroState DoState(HeroStateMachine hero, float deltatime)
    {
        InitState(hero);
        return Attack(deltatime);       // run the attack method, and return the state outcome
    }

    public void InitState(HeroStateMachine hero)
    {
        if (!Initialized)
        {
            Hero = hero;
            ConnectAttackTimerSignal();     // connect the time to signal
            Initialized = true;     // set the class as initialized
        }
    }
    private void ConnectAttackTimerSignal()
    {
        // whenever the attack timer times out, run the OnAttackTimerTimeout() method
        Hero.HeroTimers.AttackTimer.Connect("timeout", this, nameof(OnAttackTimerTimeout));
    }

    private void OnAttackTimerTimeout()
    {
        Hero.HeroTimers.AttackTimer.Stop();
        AttackTimerHasTimedOut = true;
    }

    private IHeroState Attack(float deltatime)
    {
        if (Hero.IsOnFloor())
        {
            PlayAttackAnimation("HeroAttack");
        }
        // hero is in the air
        else if (!Hero.IsOnFloor())
        {
            PlayAttackAnimation("HeroAttackInAir");

        }
        
        // if attack timer has timed out
        if (AttackTimerHasTimedOut)
        {
            AttackInProgress = false;

            if (Hero.IsOnFloor())
            {
                return Hero.StateIdle;
            }
            // hero is in the air
            else if (!Hero.IsOnFloor())
            {
                return Hero.StateFall;
            }
        }
        // if no other state was triggered, continue in the attack state
        return Hero.StateAttack;
    }

    private void PlayAttackAnimation(string attackAnimation)
    {
        // if an attack is not in progress
        if (!AttackInProgress)
        {
            AttackTimerHasTimedOut = false;     // reset attack timer timeout state
            Hero.HeroTimers.AttackTimer.Start();        // start the attack timer
            AttackInProgress =  true;       // flag that an attack is in progress
            Hero.HeroAnimations.Play(attackAnimation);      
        }
    }
}
