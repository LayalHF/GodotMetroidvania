using Godot;
using System;

namespace MetroidvaniaProject.Scripts.Hero
{
    public class HeroStateLedgeGrab : Timer, IHeroState
    {
        private bool Initialized = false;
        private HeroStateMachine Hero;
        private bool FallAfterLedgeGrab = false;

        public IHeroState DoState(HeroStateMachine hero, float deltatime)
        {
            InitState(hero); // initialize the class
            return LedgeGrab(); // run ledgeGrab method, and return the state outcome
        }

        private void InitState(HeroStateMachine hero)
        {
            // if class has not been initialized
            if (!Initialized)
            {
                Hero = hero;
                ConnectLedgeFallTimerSignal();
                Initialized = true;
            }
        }

        private void ConnectLedgeFallTimerSignal()
        {
            // whenever the ledge fall timer times out, run the OnLedgeFallTimerTimeout() method
            Hero.HeroTimers.LedgeFallTimer.Connect("timeout", this, nameof(OnLedgeFallTimerTimeout));
        }

        private void OnLedgeFallTimerTimeout()
        {
            FallAfterLedgeGrab = false;
        }

        public IHeroState LedgeGrab()
        {
            Hero.HeroMoveLogic.GravityDisabled = true; // disable gravity
            Hero.HeroMoveLogic.MovementDisabled = true; // disable movement
            Hero.HeroMoveLogic.Velocity.y = 0; // Reset gravity velocity to 0, so the hero won't fall at the previous accumulated gravity

            if (Input.IsActionJustPressed("Down"))
            {
                FallAfterLedgeGrab = true;      // flag that hero just released the ledge
                Hero.HeroTimers.LedgeFallTimer.Start();
            }
            
            Hero.HeroAnimations.Play("HeroLedgeGrab");
            return Hero.StateLedgeGrab;
        }


        // this method can be called by other states. The ledge-grab state might not be initialized when this method is called
        // therefore, we pass in the hero as reference, to not get a null-exception
        public bool CanHeroLedgeGrab(HeroStateMachine hero)
        {
            // if the ray above the hero's head is not colliding and the ray next to the hero is colliding
            if (!hero.HeroRaycasts.LedgeGrabRayCastTileAbove.IsColliding() &&
                hero.HeroRaycasts.LedgeGrabRayCastTileHead.IsColliding())
            {
                return true; // the hero can ledge grab
            }

            return false;
        }
    }
}