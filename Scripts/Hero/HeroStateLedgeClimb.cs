using Godot;
using System;

namespace MetroidvaniaProject.Scripts.Hero
{
    public class HeroStateLedgeClimb : Timer, IHeroState
    {
        private bool Initialized = false; // bool to keep track if the class has been initialized

        private bool LedgeClimbTimerHasTimedOut = false; // if the timer has timed out (climb is finished)

        private HeroStateMachine Hero;

        private bool LedgeClimbInitiated = false; // if the hero has started to perform the ledge-climb

        public IHeroState DoState(HeroStateMachine hero, float deltatime)
        {
            InitState(hero);
            return LedgeClimb(deltatime);
        }

        private IHeroState LedgeClimb(float deltatime)
        {
            CheckLedgeClimbStateInitiated();        // check if ledge climb state has been initiated or not
            PerformClimb(deltatime);

            if (LedgeClimbTimerHasTimedOut)
            {
                LedgeClimbInitiated = false;
                Hero.HeroCollisionShapes.ChangeCollisionShapesToStanding();
                Hero.HeroMoveLogic.GravityDisabled = false;
                Hero.HeroMoveLogic.MovementDisabled = false;
                return Hero.StateIdle;
            }
            
            Hero.HeroAnimations.Play("HeroLedgeClimb");
            return Hero.StateLedgeClimb;
        }

        private void CheckLedgeClimbStateInitiated()
        {
            if (!LedgeClimbInitiated)
            {
                LedgeClimbTimerHasTimedOut = false;
                Hero.HeroCollisionShapes.DisableAllCollisionShapes();
                Hero.HeroTimers.LedgeClimbTimer.Start();
                LedgeClimbInitiated = true;
            }
        }
        
        public void InitState(HeroStateMachine hero)
        {
            if (!Initialized)
            {
                Hero = hero; // set hero reference
                ConnectLedgeClimbTimerSignal();
                Initialized = true; // set the class as initialized
            }
        }

        private void ConnectLedgeClimbTimerSignal()
        {
            // whenever the slide timer times out, run the OnSlideTimerTimeout() method
            Hero.HeroTimers.LedgeClimbTimer.Connect("timeout", this, nameof(OnLedgeClimbTimerTimeout));
        }

        private void OnLedgeClimbTimerTimeout()
        {
            Hero.HeroTimers.LedgeClimbTimer.Stop(); // stop the ledge climb timer
            LedgeClimbTimerHasTimedOut = true; // flag that the ledge climb timer has timed out
        }

        private void PerformClimb(float deltatime)
        {
            // if hero is facing left (Animations are flipped)
            if (Hero.HeroAnimations.FlipH)
            {
                Hero.HeroMoveLogic.Velocity.x = -25; // push the hero left onto the platform
            }
            // if hero facing right (Animations are not flipped)
            else if (!Hero.HeroAnimations.FlipH)
            {
                Hero.HeroMoveLogic.Velocity.x = 25; // push the hero right onto the platform
            }

            // push the hero upwards
            Hero.HeroMoveLogic.Velocity.y = -4300 * deltatime;
        }
    }
}