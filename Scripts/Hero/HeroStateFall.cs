using Godot;

namespace MetroidvaniaProject.Scripts.Hero
{
   public class HeroStateFall : Timer, IHeroState
   {
      private bool Initialized = false;
      private bool CoyoteTimeTimerHasTimedOut = false;      // if the coyote timer has timedout
      public bool CanCoyoteTimeJump = false;    // if the hero can perform a coyote time jump
      private HeroStateMachine Hero;      // the hero state machine
      
      public IHeroState DoState(HeroStateMachine hero, float deltatime)
      {
         InitState(hero);     // initialize the state
         return Fall(hero, deltatime);
      }

      private void InitState(HeroStateMachine hero)
      {
         if (!Initialized)
         {
            Initialized = true;
            Hero = hero;

            Hero.HeroTimers.CoyoteTimeTimer.Connect("timeout", this, nameof(OnCoyoteTimeTimerTimeout));
         }
      }

      private void OnCoyoteTimeTimerTimeout()
      {
         CanCoyoteTimeJump =  false;
      }
      private IHeroState Fall(HeroStateMachine hero, float deltatime)
      {
         // Enable Snapping so the Hero will be able to walk on slopes
         Hero.HeroMoveLogic.EnableSnap();
      
         Hero.HeroAnimations.Play("HeroFall");
         
         // if the attack action is pressed
         if (Input.IsActionJustPressed("Attack"))
         {
            return Hero.StateAttack;
         }
         
         // if hero is falling next to a ledge
         if (Hero.StateLedgeGrab.CanHeroLedgeGrab(Hero))
         {
            return Hero.StateLedgeGrab;
         }
         // if the glide action is pressed
         if (Input.IsActionJustPressed("Glide"))
         {
            Hero.HeroEquipment.Glider.OpenGlider();
             return Hero.StateGlide;
         }
         
         // if the hero is landing on the ground/floor
         if(Hero.IsOnFloor())
         {
            Hero.StateJump.ResetJumpCounter();
            
            // if the hero is moving
            if (Hero.HeroMoveLogic.IsMoving)
            {
               return Hero.StateRun;
            }
            // if not, return the idle state
            return Hero.StateIdle;
         }
         
         if (Input.IsActionJustPressed("Jump"))
         {
            if (CanCoyoteTimeJump 
                || Hero.StateJump.CanHeroJumpBufferJump(Hero)
                || Hero.StateJump.CanWallJump(Hero) 
                || Hero.StateJump.CanJumpAgainInAir())
            {
               CanCoyoteTimeJump = false;
               return Hero.StateInitJump;
            }
         }
         // if no other state was triggered, continue the falling
         return Hero.StateFall;
      }

      public void HeroPassedOverAnEdgeStartCoyoteTimeTimer(HeroStateMachine hero)
      {
         hero.StateFall.CanCoyoteTimeJump = true;
         hero.HeroTimers.CoyoteTimeTimer.Start();
      }
   }
}
