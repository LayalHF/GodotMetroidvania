using Godot;

namespace MetroidvaniaProject.Scripts.Hero
{
    public class HeroStateRun : IHeroState
    {
        public IHeroState DoState(HeroStateMachine hero, float delta)
        {
            return Run(hero, delta);
        }

        private IHeroState Run(HeroStateMachine hero, float delta)
        {
            // Set the animation hero to run
            hero.HeroAnimations.Play("HeroRun");

            if (Input.IsActionJustPressed("Jump"))
            {
                return hero.StateInitJump;
            }
            
            // if the hero is not on the ground or floor
            if (!hero.IsOnFloor())
            {
                // set state to falling
                return hero.StateFall;
            }
            else if (hero.IsOnFloor())
            {
                // if the hero is not moving
                if (!hero.IsMoving)
                {
                    // set the state to idle
                    return hero.StateIdle;
                }
            }

            // if no other state is triggered, continue running state
            return hero.StateRun;
        }
    }
}