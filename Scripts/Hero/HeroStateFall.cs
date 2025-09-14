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
         hero.EnableSnap();
      
         hero.HeroAnimations.Play("HeroFall");

         if (hero.IsOnFloor())
         {
            if (hero.IsMoving)
            {
               return hero.StateRun;
            }
            return hero.StateIdle;
         }

         return hero.StateFall;
      }
   }
}
