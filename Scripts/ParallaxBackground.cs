using Godot;

namespace UnnamedProject.Scripts
{
    public class ParallaxBackground : Godot.ParallaxBackground
    {
        private float ScrollSpeed = 100;
        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(float delta)
        {
            this.ScrollOffset += new Vector2(ScrollSpeed *  delta, 0);
        }
    }
}