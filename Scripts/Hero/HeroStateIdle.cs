using Godot;

namespace MetroidvaniaProject.Scripts.Hero
{
    public class HeroStateIdle : IHeroState
    {
        public IHeroState DoState(HeroStateMachine hero, float delta)
        {
            return Idle(hero, delta);
        }

        private IHeroState Idle(HeroStateMachine hero, float delta)
        {
            if (hero.IsOnFloor())
            {
                // enable snap so the hero will snap to the slopes
                hero.EnableSnap();
                
                if (Input.IsActionJustPressed("Jump"))
                {
                    return hero.StateInitJump;
                }
                
                hero.HeroAnimations.Play("HeroIdle");

                if (hero.IsMoving)
                {
                    return hero.StateRun;
                }
            }
            else if (!hero.IsOnFloor())
            {
                return hero.StateFall;
            }

            return hero.StateIdle;
        }
    }
}