using Godot;

namespace UnnamedProject.Scripts
{
    public class Main : Node2D
    {

        private Sprite _icon;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _icon = GetNode<Sprite>("Sprite");
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(float delta)
        {
      
            _icon.Position += new Vector2(3, 0);
            if (_icon.Position.x > 800)
            {
                _icon.Position = new Vector2(-50, 300);
            }
        }
    }
}
