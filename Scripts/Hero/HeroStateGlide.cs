using Godot;

namespace MetroidvaniaProject.Scripts.Hero
{
    public class HeroStateGlide : Node2D , IHeroState
    {
        public float GliderGravity = 100;
        public IHeroState DoState(HeroStateMachine hero, float deltatime)
        {
            return Glide(hero);
        }

        private IHeroState Glide(HeroStateMachine hero)
        {
            StopUpwardGliding(hero);        // stop gliding upwards
            CatchTheWind(hero);
            
            // flip the glider in the same direction as the hero
            hero.HeroEquipment.Glider.FlipHorizontal = hero.HeroAnimations.FlipH;
            
            if (hero.IsOnFloor())
            {
                hero.HeroEquipment.Glider.CloseGlider();
                return hero.StateIdle;
            }
            
            // if hero is next to a ledge
            if (hero.StateLedgeGrab.CanHeroLedgeGrab(hero))
            {
                hero.HeroEquipment.Glider.CloseGlider();        // close the glider
                return hero.StateLedgeGrab;
            }
            
            return hero.StateGlide;
        }

        private void StopUpwardGliding(HeroStateMachine hero)
        {
            // make sure hero does not glide upwards
            if (hero.HeroMoveLogic.Velocity.y <= 40)
            {
                // liniary interpolate down towards zero
                hero.HeroMoveLogic.Velocity.y = Mathf.Lerp(hero.HeroMoveLogic.Velocity.y, 40, 0.1f);
            }
        }

        private void CatchTheWind(HeroStateMachine hero)
        {
            // if the current gravity is greater than the glider gravity
            if (hero.HeroMoveLogic.Velocity.y > GliderGravity)
            {
                // lineary interpolate the falling velocity to math the hero glider gravity
                hero.HeroMoveLogic.Velocity.y = Mathf.Lerp(hero.HeroMoveLogic.Velocity.y, GliderGravity, 0.15f);
            }
        }
    }
}
