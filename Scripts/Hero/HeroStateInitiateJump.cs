namespace MetroidvaniaProject.Scripts.Hero
{
    public class HeroStateInitJump : IHeroState
    {
        private float JumpForce = -700f;        // the strength of the jump
        private bool JumpInitiated = false;     // if the jump has been initiated

        public IHeroState DoState(HeroStateMachine hero, float deltatime)
        {
            return InitiateJump(hero,deltatime);
        }

        private IHeroState InitiateJump(HeroStateMachine hero, float deltatime)
        {
            // disable snap so the hero can jump off from slopes
            hero.DisableSnap();

            // if jump has not been initiated 
            if (!JumpInitiated)
            {
                JumpInitiated = true;
                // apply the jump force to the hero velocity
                hero.HeroMoveLogic.Velocity.y = JumpForce;
            }

            hero.HeroAnimations.Play("HeroInitJump");

            if (hero.LastPlayedHeroAnimation.Equals("HeroInitJump"))
            {
                // reset the jump initiated flag
                this.JumpInitiated = false;

                return hero.StateJump;
            }

            return hero.StateInitJump;
        }
    }
}
