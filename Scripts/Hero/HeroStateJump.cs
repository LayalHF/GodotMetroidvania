namespace MetroidvaniaProject.Scripts.Hero
{
   public class HeroStateJump : IHeroState
   {
      public IHeroState DoState(HeroStateMachine hero, float deltatime)
      {
         return Jump(hero, deltatime);
      }

      private IHeroState Jump(HeroStateMachine hero, float deltatime)
      {
         hero.DisableSnap();
      
         // Set the animation hero to run
         hero.HeroAnimations.Play("HeroJump");
      
         if (!hero.IsOnFloor())
         {
            if (hero.HeroMoveLogic.Velocity.y < 0)
            {
               return hero.StateJump;
            }

            if (hero.HeroMoveLogic.Velocity.y > 0)
            {
               return hero.StateFall;
            }
         }else if (hero.IsOnFloor())
         {
            return hero.StateIdle;
         }
      
         return hero.StateJump;
      }
   
   }
}
