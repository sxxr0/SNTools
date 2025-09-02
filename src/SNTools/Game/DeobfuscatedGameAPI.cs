global using static SNTools.Game.DeobfuscatedTypes;
global using ActorType = EnumPublicSealedvaEXNECUSPITAN8vNEUnique;
//global using LobbyPlayerState = EnumPublicSealedvaNORELOLESEGASCMA9vUnique;
//global using WindowBreakMessage = Object2PublicVeVoVeObObVeObVeObVeUnique;
global using EncryptedString = ValueTypePublicSealedObBy_sVoStVoByVoUnique;
global using EndingType = EnumPublicSealedvaALBATIQU5vUnique;
global using EndMatchMessage = Object2Public47ObVo47Ob47Ob47Ob470;
//global using KeycardDoorUnlockedMessage = Object2PublicIn31Ob31InObVo31InObUnique;
//global using LockUnlockedMessage = Object2Public3150PlObVo50PlObPl50Unique;
global using GameContext = ObjectPublicObStObLi1ObMaLi1BoUnique;
global using GameMode = EnumPublicSealedvaNOGALOMEPRGAMASHCRUnique;
global using GameModeController = ObjectPublicDi2ObLiOb1AcBoAcObUnique;
global using GetLoadoutRequestResult = Object1Public99VoUnique;
global using HoloNetGlobalMessage = Object1PublicObVoObUnique;
global using HoloNetMessenger = ObjectPublicDoBoObBoUnique;
global using HoloNetObjectMessage = Object1PublicObBoObUnique;
global using Loadout = ObjectPublicAcLiAc1ObObObUnique;
global using LoadoutSerializer = h5ay7rju46La5doyt5jtndrdfjf;
global using Lobby = ObjectPublicStObInObBoBoInObBoObUnique;
global using LobbyPlayerDoEmotionMessage = Object2PublicStObVoStObStObStObSt0;
//global using DoorOpenMessage = Object2PublicSiObPlUnique;
global using OriginalMessageTarget = EnumPublicSealedva5v1;
global using PlayerApplyBuffMessage = Object2PublicSeUnique;
global using PlayerBuff = EnumPublicSealedvaSTCAGLCADITODIKNSLUnique;
global using PlayerController = ObjectPublicObLi1PlInPlInObLi1Unique;
global using PlayerDeactiveBuffMessage = Object2PublicInObUnique;
global using PlayerInfo = ObjectPublicObInObBoPlObBoAc1StUnique;
global using PlayfabBackendAdapter = ObjectPublicStObStDi2SiObDi2StUnique;
//global using PlayerPickUpResourceMessage = Object2Public31InObVoInObInObInOb0;
//global using SlingshotManager = Object2PublicObSiInSlBoScInBoSlUnique;
//global using SonarBeamMessage = Object2Public92SiObVo92SiOb92SiOb0;
//global using SonarIdleMessage = Object2Public92SiObVo92SiOb92SiOb1; // I'm not exactly sure what this message is for, but it's used between the sonar beams
//global using LobbyPlayerStateSyncMessage = Object2Public60ObVo60Ob60Ob60Ob60Unique;
global using TeleportPlayerMessage = Object2PublicVeObVoVeObVeObVeObVe4;
//global using ThrowableItemApplyForceMessage = Object2PublicVeDoVeUnique;
global using UpdateLoadoutRequestResult = Object1PublicVo12;
using AppControllers;
using GameModes.GameplayMode.ActorClassSystem.Classes;
using GameModes.GameplayMode.Actors;
using GameModes.GameplayMode.Interactables.InventoryItems;
using GameModes.GameplayMode.Players;
using GameModes.LobbyMode;
using GameModes.LobbyMode.LobbyCharacters;
using HoloNetwork.NetworkObjects;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Ui.Screens.CustomGame;
using UnityEngine;

namespace SNTools.Game;

internal static class DeobfuscatedExtensions
{
    public static void SendMessage(this HoloNetObject obj, HoloNetObjectMessage message, MessageTarget target = MessageTarget.All, bool reliable = true)
    {
        message.field_Public_ValueTypePublicSealedObauUIobBoObBoObBoUnique_0 = obj.oid;
        ObjectPublicOb8423Ob617057203317Unique.prop_ObjectPublicOb8423Ob617057203317Unique_0.field_Public_ObjectPublicBoLi1ObHaTy1ObUnique_0.Method_Public_Void_ObjectPublicAbstractDoBoObBoSiObBoObBoObUnique_EnumPublicSealedva5v1_Boolean_0(message, (OriginalMessageTarget)target, reliable);
    }

    public static void SetResultReceived<T>(this T requestResult, Il2CppSystem.Action<T> callback) where T : ObjectPublicBoObStAc1ObBoUnique<T>
    {
        requestResult.field_Public_Action_1_TC_0 = callback;
        requestResult.field_Public_Boolean_0 = true;
        callback?.Invoke(requestResult);
    }

    public static void SetLoadoutResult(this GetLoadoutRequestResult requestResult, Loadout loadout, Il2CppSystem.Action<GetLoadoutRequestResult> callback)
    {
        requestResult.field_Public_ObjectPublicAcLiAc1ObObObUnique_0 = loadout;
        requestResult.SetResultReceived(callback);
    }

    public static Loadout Unbox(this LoadoutSerializer serializer)
    {
        return serializer.Method_Public_ObjectPublicAcLiAc1ObObObUnique_PDM_0();
    }

    public static LoadoutSerializer Box(this Loadout loadout)
    {
        return LoadoutSerializer.Method_Public_Static_h5ay7rju46La5doyt5jtndrdfjf_ObjectPublicAcLiAc1ObObObUnique_0(loadout);
    }

    public static Lobby GetSelectedLobby(this CustomGameScreen cgs)
    {
        return cgs.field_Private_ObjectPublicStObInObBoBoInObBoObUnique_0;
    }

    public static Il2CppSystem.Collections.Generic.List<Actor> GetActors(this Player player)
    {
        return player.prop_List_1_Actor_0;
    }

    public static Actor? GetCurrentActor(this Player player)
    {
        return player?.prop_Actor_0;
    }

    //public static void Reload(this RifleInventoryItem rifle)
    //{
    //    rifle.field_Protected_ObscuredBool_0 = true;
    //}

    //public static void SetResourceAmount(this PlayerResourceManager prm, int amount)
    //{
    //    prm.prop_ObscuredInt_1 = amount;
    //}

    public static bool IsDead(this Player player)
    {
        return player.prop_Boolean_1;
    }

    public static bool IsNeighbor(this Player player)
    {
        return player.prop_Boolean_3;
    }

    public static PlayerInfo GetPlayerInfo(this Player player)
    {
        return player.prop_ObjectPublicObInObBoPlObBoAc1StUnique_0;
    }

    public static string Decrypt(this EncryptedString str)
    {
        return ObjectPublicAbstractSealedStObStOb0.Method_Public_Static_String_ValueTypePublicSealedObBy_sVoStVoByVoUnique_PDM_0(str);
    }

    public static EncryptedString Encrypt(this string str)
    {
        return Il2CppSystem.Activator.CreateInstance(Il2CppType.Of<EncryptedString>(), new Il2CppReferenceArray<Il2CppSystem.Object>(
        [
            new Il2CppSystem.String(IL2CPP.ManagedStringToIl2Cpp(str))
        ])).Cast<EncryptedString>();
    }

    public static string GetName(this PlayerInfo player)
    {
        return player.field_Public_ValueTypePublicSealedObBy_sVoStVoByVoUnique_2.Decrypt();
    }

    public static string GetID(this PlayerInfo player)
    {
        return player.field_Public_ValueTypePublicSealedObBy_sVoStVoByVoUnique_0.Decrypt();
    }

    public static void SetName(this PlayerInfo player, string name)
    {
        player.field_Public_ValueTypePublicSealedObBy_sVoStVoByVoUnique_2 = name.Encrypt();
    }

    public static void SetID(this PlayerInfo player, string id)
    {
        player.field_Public_ValueTypePublicSealedObBy_sVoStVoByVoUnique_0 = id.Encrypt();
    }

    public static ActorClassInfo? GetActorClassInfo(this Actor actor)
    {
        return actor?.prop_ObjectPublicISerializableObLoObAcLoUnique_0?.prop_ActorClassInfo_0;
    }

    public static void Buff(this Player player, PlayerBuff buff)
    {
        player.prop_HoloNetObject_0.SendMessage(HoloNetMessages.CreatePlayerApplyBuffMessage(buff));
    }

    public static void Debuff(this Player player, PlayerBuff buff)
    {
        player.prop_HoloNetObject_0.SendMessage(HoloNetMessages.CreatePlayerDeactiveBuffMessage(buff));
    }

    public static HoloNetObject GetNetObject(this Player player)
    {
        return player.prop_HoloNetObject_0;
    }

    public static HoloNetObject GetNetObject(this LobbyPlayer player)
    {
        return player.prop_HoloNetObject_0;
    }

    public static void RefreshName(this LobbyCharacter character)
    {
        character.Method_Public_Void_PDM_4();
    }

    public static bool IsLocal(this LobbyPlayer player)
    {
        return player.GetNetObject().IsLocal;
    }

    public static string GetEmotionID(this LobbyPlayerDoEmotionMessage message)
    {
        return message.field_Public_String_0;
    }
}

internal static class DeobfuscatedTypes
{
    public static class HoloNetMessengerAPI
    {
        public static void SendMessage(HoloNetGlobalMessage message, MessageTarget target = MessageTarget.All)
        {
            HoloNetMessenger.Method_Public_Static_Void_Object1PublicObVoObUnique_EnumPublicSealedva5v1_0(message, (OriginalMessageTarget)target);
        }
    }

    public static class PlayfabBackendAdapterAPI
    {
        public const string InitializeMethod = nameof(PlayfabBackendAdapter.Method_Public_Virtual_Final_New_Void_2);
        public const string UpdateLoadoutRequestMethod = nameof(PlayfabBackendAdapter.Method_Public_Virtual_Final_New_Object1PublicVo12_ObjectPublicAcLiAc1ObObObUnique_Action_1_Object1PublicVo12_0);
        public const string GetLoadoutRequestMethod = nameof(PlayfabBackendAdapter.Method_Public_Virtual_Final_New_Object1Public99VoUnique_Action_1_Object1Public99VoUnique_0);
    }

    public static class GameModeControllerAPI
    {
        public const string SetGameModeMethod = nameof(GameModeController.Method_Public_Void_EnumPublicSealedvaNOGALOMEPRGAMASHCRUnique_0);
    }

    public static class LobbyControllerAPI
    {
        public static void JoinLobby(Lobby lobby)
        {
            LobbyController.prop_LobbyController_0.Method_Public_Void_ObjectPublicStObInObBoBoInObBoObUnique_0(lobby);
        }
    }

    public static class CustomGameScreenAPI
    {
        public const string OnJoinAttemptMethod = nameof(CustomGameScreen.Method_Private_Void_List_1_InterfacePublicAbstractTyBoVoTyBoObVoUnique_0);
    }

    public static class PlayerControllerAPI
    {
        public static LobbyPlayer? GetLocalLobbyPlayer()
        {
            return LobbyController.prop_LobbyController_0?.players.prop_LobbyPlayer_0;
        }

        public static Il2CppSystem.Collections.Generic.List<LobbyPlayer>? GetAllLobbyPlayers()
        {
            return LobbyController.prop_LobbyController_0?.players.prop_List_1_LobbyPlayer_0;
        }

        public static Player GetLocalPlayer()
        {
            return PlayerController.prop_Player_0;
        }

        public static Il2CppSystem.Collections.Generic.List<Player> GetAllPlayers()
        {
            return PlayerController.prop_List_1_Player_0;
        }
    }

    public static class RifleInventoryItemAPI
    {
        public const string ShootRifleMethod = nameof(RifleInventoryItem.Method_Protected_Void_Object2PublicVeObVeVoObVeObVeObVe2_PDM_2);
        //public const string PickUpRifleMethod = nameof(RifleInventoryItem.Method_Public_Virtual_Final_New_Void_Action_0);
    }

    public static class SlingshotAPI
    {
        public const string InitializeMethod = nameof(SlingshotInventoryItem.Method_Public_Virtual_Void_1);
        //public const string HasEnoughResourcesMethod = nameof(SlingshotManager.Method_Private_Boolean_PDM_0);
    }

    public static class LobbyPlayerAPI
    {
        public const string OnKickMessageMethod = nameof(LobbyPlayer.Method_Public_Void_Object2PublicObVoObObObObObObObOb0_PDM_0);
        public const string OnEmotionMessageMethod = nameof(LobbyPlayer.Method_Private_Void_Object2PublicStObVoStObStObStObSt0_PDM_0);
    }

    public static class GameContextAPI
    {
        public static PlayerInfo GetLocalPlayerInfo()
        {
            return GameContext.prop_ObjectPublicObStObLi1ObMaLi1BoUnique_0.field_Public_ObjectPublicObInObBoPlObBoAc1StUnique_0;
        }
    }

    public static class PlatformAPI
    {
        public static string GetUserName()
            => AppController.prop_AppController_0.field_Public_ObjectPublicBoObBoAc2BoPlObStBoUnique_0.Method_Public_String_0();
    }

    public static class HoloNetMessages
    {
        public static EndMatchMessage CreateEndMatchMessage(EndingType type)
        {
            return new EndMatchMessage()
            {
                field_Public_EnumPublicSealedvaALBATIQU5vUnique_0 = type
            };
        }

        public static PlayerApplyBuffMessage CreatePlayerApplyBuffMessage(PlayerBuff buff)
        {
            return new PlayerApplyBuffMessage()
            {
                field_Public_SerializableBuff_0 = new(buff)
            };
        }

        public static PlayerDeactiveBuffMessage CreatePlayerDeactiveBuffMessage(PlayerBuff buff)
        {
            return new PlayerDeactiveBuffMessage()
            {
                field_Public_Int32_0 = (int)buff
            };
        }

        public static TeleportPlayerMessage CreateTeleportPlayerMessage(Vector3 position)
        {
            return new TeleportPlayerMessage()
            {
                field_Public_Vector3_0 = position
            };
        }

        //public static LobbyPlayerSyncInfoMessage CreateLobbyPlayerSyncInfoMessage(PlayerInfo playerInfo, ActorInfo childInfo, ActorInfo neighborInfo)
        //{
        //    return LobbyPlayerSyncInfoMessage.Method_Public_Static_Object2PublicObBoObObObObUnique_ObjectPublicObBoObAcBo1ObStAc1Unique_ObjectPublicISerializableObLoObAcLoUnique_ObjectPublicISerializableObLoObAcLoUnique_ObjectPublicISerializableObLoObAcLoUnique_EnumPublicSealedvaWISPSUAU5vUnique_Boolean_0(playerInfo, childInfo, neighborInfo);
        //}

        public static LobbyPlayerDoEmotionMessage CreateLobbyPlayerDoEmotionMessage(string emotionID)
        {
            return new LobbyPlayerDoEmotionMessage()
            {
                field_Public_String_0 = emotionID
            };
        }
    }

    public enum MessageTarget
    {
        Others,
        All,
        Server,
        Self
    }
}