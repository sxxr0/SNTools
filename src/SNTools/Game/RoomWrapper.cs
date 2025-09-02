namespace SNTools.Game;

public class RoomWrapper(HoloNetRoom holoNetRoom)
{
    public HoloNetRoom HoloNetRoom { get; set; } = holoNetRoom;

    public string Id => HoloNetRoom.field_Public_String_0;

    public string Name => HoloNetRoom.field_Public_ValueTypePublicSealedSt_sUnique_0.Method_Public_String_PDM_0();

    public string Type => HoloNetRoom.field_Public_ValueTypePublicSealedSt_sUnique_1.Method_Public_String_PDM_0();

    public string Locale => HoloNetRoom.field_Public_ValueTypePublicSealedSt_sUnique_2.Method_Public_String_PDM_0();

    public string Password => HoloNetRoom.field_Public_ValueTypePublicSealedSt_sUnique_3.Method_Public_String_PDM_0();

    public int PlayerCount => HoloNetRoom.field_Public_Int32_1;

    public string PasswordWithIcon => string.IsNullOrEmpty(Password) ? string.Empty : ("🔒" + Password);

    public void Join()
    {
        LobbyControllerAPI.JoinLobby(HoloNetRoom);
    }

    public override string ToString()
        => string.IsNullOrEmpty(Name) ? Id : Name;
}
