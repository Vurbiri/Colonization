using System.Runtime.CompilerServices;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Actors.UI;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Colonization.EntryPoint;
using Vurbiri.Colonization.Storage;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
	public class GameContainer : ProjectContainer
    {
        private static GameContent s_content;

        public static GameStorage Storage               { [MethodImpl(Inline)] get => s_content.storage; }
        public static GameLoop GameLoop                 { [MethodImpl(Inline)] get => s_content.gameLoop; }
        public static GameEvents GameEvents             { [MethodImpl(Inline)] get => s_content.gameLoop; }
        public static GameTriggerBus TriggerBus         { [MethodImpl(Inline)] get => s_content.triggerBus; }
        public static GameEventBus EventBus             { [MethodImpl(Inline)] get => s_content.triggerBus; }

        public static InputController InputController   { [MethodImpl(Inline)] get => s_content.inputController; }
        public static CameraController CameraController { [MethodImpl(Inline)] get => s_content.cameraController; }
        public static CameraTransform CameraTransform   { [MethodImpl(Inline)] get => s_content.cameraTransform; }
        
        public static Hexagons Hexagons                 { [MethodImpl(Inline)] get => s_content.hexagons; }
        public static Crossroads Crossroads             { [MethodImpl(Inline)] get => s_content.crossroads; }

        public static ActorsFactory ActorsFactory       { [MethodImpl(Inline)] get => s_content.actorsFactory; }

        public static Players Players                   { [MethodImpl(Inline)] get => s_content.players; }
        public static Balance Balance                   { [MethodImpl(Inline)] get => s_content.balance; }
        public static Score Score                       { [MethodImpl(Inline)] get => s_content.score; }
        public static Diplomacy Diplomacy               { [MethodImpl(Inline)] get => s_content.diplomacy; }

        public static Transform SharedContainer         { [MethodImpl(Inline)] get => s_content.sharedContainer; }
        public static AudioSource SharedAudioSource     { [MethodImpl(Inline)] get => s_content.sharedAudioSource; }

        public static SFXStorage HitSFX                 { [MethodImpl(Inline)] get => s_content.actorSFXs; }

        public new class UI : ProjectContainer.UI
        {
            public static WorldHint WorldHint                                           { [MethodImpl(Inline)] get => s_content.worldHint; }
            public static CanvasHint CanvasHint                                         { [MethodImpl(Inline)] get => s_content.canvasHint; }

            public static ReadOnlyIdArray<ActorAbilityId, Sprite> SpritesOfAbilities    { [MethodImpl(Inline)] get => s_content.abilities; }
 
            public static Pool<EffectsBar> EffectsBar                                   { [MethodImpl(Inline)] get => s_content.poolEffectsBar; }
         }

        public GameContainer(GameContent content) => s_content ??= content;
        public override void Dispose()
        {
            s_content.Dispose();
            s_content = null;
        }
    }
}
