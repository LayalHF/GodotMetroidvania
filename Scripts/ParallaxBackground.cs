using Godot;

namespace MetroidvaniaProject.Scripts
{
    public class ParallaxBackground : Godot.ParallaxBackground
    {
        private readonly float ScrollSpeed = 100;

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(float delta)
        {
            ScrollOffset += new Vector2(ScrollSpeed * delta, 0);
        }
    }
}