using Godot;

namespace MetroidvaniaProject.Scripts.Hero
{
   public class HeroStateFall : IHeroState
   {
      public IHeroState DoState(HeroStateMachine hero, float deltatime)
      {
         return Fall(hero, deltatime);
      }

      private IHeroState Fall(HeroStateMachine hero, float deltatime)
      {
         // Enable Snapping so the Hero will be able to walk on slopes
         hero.HeroMoveLogic.EnableSnap();
      
         hero.HeroAnimations.Play("HeroFall");

         // if hero is falling next to a ledge
         if (hero.StateLedgeGrab.CanHeroLedgeGrab(hero))
         {
            return hero.StateLedgeGrab;
         }
         // if the glide action is pressed
         if (Input.IsActionJustPressed("Glide"))
         {
             hero.HeroEquipment.Glider.OpenGlider();
             return hero.StateGlide;
         }
         
         // if the hero is landing on the ground/floor
         if(hero.IsOnFloor())
         {
            hero.StateJump.ResetJumpCounter();
            
            // if the hero is moving
            if (hero.HeroMoveLogic.IsMoving)
            {
               return hero.StateRun;
            }
            // if not, return the idle state
            return hero.StateIdle;
         }
         
         if (Input.IsActionJustPressed("Jump"))
         {
            if (hero.StateJump.CanWallJump(hero))
            {
               return hero.StateInitJump;
            }
            if (hero.StateJump.CanJumpAgainInAir())
            {
               return hero.StateInitJump;   
            }
         }
         
         // if no other state was triggered, continue the falling
         return hero.StateFall;
      }
   }
}
