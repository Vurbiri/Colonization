using System.Runtime.CompilerServices;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Colonization.EntryPoint;
using Vurbiri.Colonization.Storage;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
	public class GameContainer : ProjectContainer
    {
        private static GameContent s_content;

        public static GameStorage Storage
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_content.storage;
        }

        public static GameLoop GameLoop
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_content.gameLoop;
        }
        public static GameEvents GameEvents
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_content.gameLoop;
        }

        public static GameTriggerBus TriggerBus
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_content.triggerBus;
        }
        public static GameEventBus EventBus
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_content.triggerBus;
        }

        public static InputController InputController
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_content.inputController;
        }
        public static CameraController CameraController
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_content.cameraController;
        }
        public static CameraTransform CameraTransform
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_content.cameraTransform;
        }
        
        public static Hexagons Hexagons
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_content.hexagons;
        }
        public static Crossroads Crossroads
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_content.crossroads;
        }

        public static Players Players
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_content.players;
        }
        public static Balance Balance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_content.balance;
        }
        public static Score Score
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_content.score;
        }
        public static Diplomacy Diplomacy
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_content.diplomacy;
        }

        public static Transform Repository
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_content.sharedRepository;
        }
        public static AudioSource AudioSource
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_content.sharedAudioSource;
        }

        public new class UI : ProjectContainer.UI
        {
            public static WorldHint WorldHint
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => s_content.worldHint;
            }
            public static CanvasHint CanvasHint
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => s_content.canvasHint;
            }

            public static Pool<EffectsBar> EffectsBar
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => s_content.poolEffectsBar;
            }
        }


        public GameContainer(GameContent content)
        {
            s_content ??= content;
        }

        public override void Dispose()
        {
            s_content.Dispose();
            s_content = null;
        }
    }
}
