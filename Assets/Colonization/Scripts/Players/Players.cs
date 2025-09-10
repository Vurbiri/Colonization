using System;
using System.Runtime.CompilerServices;

namespace Vurbiri.Colonization
{
    public class Players : IDisposable
    {
        private readonly PlayersController _controller;
        private readonly Human[] _humans = new Human[PlayerId.HumansCount];
        private readonly Satan _satan;

        public Human this[int id]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _humans[id];
        }
        public Human[] Humans
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _humans;
        }
        public Human Person
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _humans[PlayerId.Person];
        }
        public Satan Satan
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _satan;
        }

        public Players(Player.Settings settings, GameLoop game)
        {
            _controller = new(game);

            AddHuman(PlayerId.Person, new PersonController(settings));
            for (int i = PlayerId.AI_01; i < PlayerId.HumansCount; i++)
                AddHuman(i, new AIController(i, settings));

            SatanController satan = new(settings);
            _controller.Add(PlayerId.Satan, satan);
            _satan = satan;

            Player.Init();

            settings.Dispose();

            void AddHuman(int id, AHumanController controller)
            {
                _controller.Add(id, controller);
                _humans[id] = controller;
            }
        }

        public void Dispose()
        {
            Player.Clear();
            _controller.Dispose();
        }
    }
}
