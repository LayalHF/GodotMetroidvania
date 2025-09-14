namespace MetroidvaniaProject.Scripts.Hero
{
    public class HeroStateInitJump : IHeroState
    {
        private float JumpForce = -700f; // the strength of the jump

        public IHeroState DoState(HeroStateMachine hero, float deltatime)
        {
            return InitiateJump(hero, deltatime);
        }

        private IHeroState InitiateJump(HeroStateMachine hero, float deltatime)
        {
            // disable snap so the hero can jump off from slopes
            hero.HeroMoveLogic.DisableSnap();
            // apply the jump force to the hero velocity
            hero.HeroMoveLogic.Velocity.y = JumpForce;

            hero.HeroAnimations.Play("HeroInitJump");

            return hero.StateJump;
        }
    }
}