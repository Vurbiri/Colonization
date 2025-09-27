using System;
using System.Runtime.CompilerServices;
using Vurbiri.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public class Players : IDisposable
    {
        private readonly PlayersController _controller;
        private readonly Array<HumanController> _humans = new(PlayerId.HumansCount);
        private readonly SatanController _satan;

        public Human this[int id] { [Impl(256)] get => _humans[id]; }
        public HumanController Person { [Impl(256)] get => _humans[PlayerId.Person]; }
        public ReadOnlyArray<HumanController> Humans { [Impl(256)] get => _humans;  }
        public SatanController Satan { [Impl(256)] get => _satan; }

        public Players(Player.Settings settings, GameLoop game)
        {
            _controller = new(game);

            AddHuman(PlayerId.Person, new PersonController(settings));
            for (int i = PlayerId.AI_01; i < PlayerId.HumansCount; i++)
                AddHuman(i, new AIController(i, settings));

            _controller.Add(PlayerId.Satan, _satan = new(settings, _humans));

            settings.Dispose();

            // Local
            //=======================================================
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            void AddHuman(int id, HumanController controller)
            {
                _controller.Add(id, controller);
                _humans[id] = controller;
            }
            //=======================================================
        }

        public void Dispose()
        {
            _satan.Dispose();
            for (int i = 0; i < PlayerId.HumansCount; i++)
                _humans[i].Dispose();
        }
    }
}
