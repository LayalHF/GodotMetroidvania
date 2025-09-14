using Godot;

namespace MetroidvaniaProject.Scripts
{
    [Tool]
    public class AutoTile : TileSet
    {
        public override bool _IsTileBound(int drawnId, int neighborId)
        {
            return GetTilesIds().Contains(neighborId);
        }
    }
}