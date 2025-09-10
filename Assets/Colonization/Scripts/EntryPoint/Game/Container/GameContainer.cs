using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Actors.UI;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Colonization.EntryPoint;
using Vurbiri.Colonization.Storage;
using Vurbiri.UI;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	public class GameContainer : ProjectContainer
    {
        private static GameContent s_content;

        public static GameStorage Storage               { [Impl(256)] get => s_content.storage; }
        public static GameLoop GameLoop                 { [Impl(256)] get => s_content.gameLoop; }
        public static GameEvents GameEvents             { [Impl(256)] get => s_content.gameLoop; }
        public static GameTriggerBus TriggerBus         { [Impl(256)] get => s_content.triggerBus; }
        public static GameEventBus EventBus             { [Impl(256)] get => s_content.triggerBus; }

        public static InputController InputController   { [Impl(256)] get => s_content.inputController; }
        public static CameraController CameraController { [Impl(256)] get => s_content.cameraController; }
        public static CameraTransform CameraTransform   { [Impl(256)] get => s_content.cameraTransform; }
        
        public static Hexagons Hexagons                 { [Impl(256)] get => s_content.hexagons; }
        public static Crossroads Crossroads             { [Impl(256)] get => s_content.crossroads; }

        public static ActorsFactory Actors              { [Impl(256)] get => s_content.actorsFactory; }

        public static Players Players                   { [Impl(256)] get => s_content.players; }
        public static Balance Balance                   { [Impl(256)] get => s_content.balance; }
        public static Score Score                       { [Impl(256)] get => s_content.score; }
        public static Diplomacy Diplomacy               { [Impl(256)] get => s_content.diplomacy; }

        public static Transform SharedContainer         { [Impl(256)] get => s_content.sharedContainer; }
        public static AudioSource SharedAudioSource     { [Impl(256)] get => s_content.sharedAudioSource; }

        public static SFXStorage HitSFX                 { [Impl(256)] get => s_content.actorSFXs; }

        public new class UI : ProjectContainer.UI
        {
            public static WorldHint WorldHint           { [Impl(256)] get => s_content.worldHint; }
            public static CanvasHint CanvasHint         { [Impl(256)] get => s_content.canvasHint; }

            public static ReadOnlyIdArray<ActorAbilityId, Sprite> SpritesOfAbilities    { [Impl(256)] get => s_content.abilities; }
 
            public static Pool<EffectsBar> EffectsBar   { [Impl(256)] get => s_content.poolEffectsBar; }
         }

        public GameContainer(GameContent content) => s_content ??= content;
        public override void Dispose()
        {
            s_content.Dispose();
            s_content = null;
        }
    }
}
