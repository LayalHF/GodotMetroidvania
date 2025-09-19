using Godot;

namespace MetroidvaniaProject.Scripts.Hero
{
    public class HeroEquipment
    {
        private HeroStateMachine Hero;

        private bool HeroEquipementInitialized = false; // flag to keep track if the hero equipment is initialized

        public Glider Glider;

        public HeroEquipment(HeroStateMachine hero, ref bool initOk)
        {
            Hero = hero;
            initOk = InitHeroEquopment();
        }

        private bool InitHeroEquopment()
        {
            InitGlider();

            if (!HeroEquipementInitialized)
            {
                return false;
            }
            return true;
        }

        private void InitGlider()
        {
            Glider = Hero.GetNode<Glider>("./Equipment/Glider");

            if (Glider == null)
            {
                GD.PrintErr("[HeroEquipment] - InitGlider() - Glider node was not found!");
                
                HeroEquipementInitialized = false;
                
                return;
            }

            HeroEquipementInitialized = true;
        }
    }
}