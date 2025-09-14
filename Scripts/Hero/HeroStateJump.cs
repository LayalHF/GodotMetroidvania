using Godot;

namespace MetroidvaniaProject.Scripts.Hero
{
    public class HeroStateJump : IHeroState
    {
        private const float CutJumpThreshold = -200.0f; // The threshold for cutting a jump start
        private const float JumpForceAfterJumpCutShort = -320.0f; // the jump force after a jump has been cut short

        public IHeroState DoState(HeroStateMachine hero, float deltatime)
        {
            return Jump(hero, deltatime);
        }

        private IHeroState Jump(HeroStateMachine hero, float deltatime)
        {
            hero.HeroMoveLogic.DisableSnap();

            // Set the animation hero to run
            hero.HeroAnimations.Play("HeroJump");

            // Cute jump short if the jump button is released
            if (Input.IsActionJustReleased("Jump") && hero.HeroMoveLogic.Velocity.y < CutJumpThreshold)
            {
                // set the velocity to the new jump force for when a jump has been cut short
                hero.HeroMoveLogic.Velocity.y = JumpForceAfterJumpCutShort;
                // return the falling state
                return hero.StateFall;
            }

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
            }
            else if (hero.IsOnFloor())
            {
                return hero.StateIdle;
            }

            return hero.StateJump;
        }
    }
}