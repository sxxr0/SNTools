namespace SNTools.Game.Tools.Identity;

public class IdSpoofer() : ToggleTool(GameMode.PRELOAD)
{
    protected override void Disable()
    {
        if (Enabled)
        {
            GameContextAPI.GetLocalPlayerInfo().SetID("FF45EB4E3A698D3F");
        }
    }

    protected override void Enable()
    {

    }
}
