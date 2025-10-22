#if UNITY_EDITOR

using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    public partial class PlayerPanels
    {
        [StartEditor]
        [SerializeField] private Vector2 _paddingIn = new(6f, 8f);
        [SerializeField, Range(1f, 10f)] private float _spaceIn = 4f;
        [Header("Between")]
        [SerializeField, Range(1f, 20f)] private float _spaceOut = 6.7f;
        [SerializeField, Range(0.1f, 1f)] private float _advRatioSpaceOut = 0.6f;
        [EndEditor] public bool endEditor;

        public void UpdateVisuals_Ed(float pixelsPerUnit, ProjectColors colors, Vector3 position)
        {
            RectTransform rectWarriors   = _warriors.UpdateVisuals_Editor(pixelsPerUnit, _paddingIn, colors);
            RectTransform rectColonies   = _colonies.UpdateVisuals_Editor(pixelsPerUnit, _paddingIn, colors);
            RectTransform rectPorts      = _ports.UpdateVisuals_Editor(pixelsPerUnit, _paddingIn, colors);
            RectTransform rectShrines    = _shrines.UpdateVisuals_Editor(pixelsPerUnit, _paddingIn, colors);
            RectTransform rectRoads      = _roads.UpdateVisuals_Editor(pixelsPerUnit, _paddingIn, colors);
            RectTransform rectCurrencies = _currencies.UpdateVisuals_Editor(pixelsPerUnit, _paddingIn, _spaceIn, colors);
            RectTransform rectBlood      = _blood.UpdateVisuals_Editor(pixelsPerUnit, _paddingIn, colors);
            RectTransform rectScore      = _score.UpdateVisuals_Editor(pixelsPerUnit, _paddingIn, colors);
            RectTransform rectArtefact   = _artefactPanel.UpdateVisuals_Editor(rectScore.rect.height, colors);

            //======= Positions
            var advPadding = _spaceOut * _advRatioSpaceOut;

            rectWarriors.localPosition   = position;
            rectColonies.localPosition   = position = NextPosition(position, rectWarriors, advPadding);
            rectPorts.localPosition      = position = NextPosition(position, rectColonies);
            rectShrines.localPosition    = position = NextPosition(position, rectPorts);
            rectRoads.localPosition      = position = NextPosition(position, rectShrines, advPadding * _advRatioSpaceOut);
            rectCurrencies.localPosition = position = NextPosition(position, rectRoads, advPadding);
            rectBlood.localPosition      = position = NextPosition(position, rectCurrencies);
            rectScore.localPosition      = position = NextPosition(position, rectBlood, advPadding);
            rectArtefact.localPosition   = position = NextPosition(position, rectScore, advPadding);

            // Local function
            Vector3 NextPosition(Vector3 current, RectTransform prevPanel, float advOffset = 0f)
            {
                current.x += prevPanel.rect.width + _spaceOut + advOffset;
                return current;
            }
        }

        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                EUtility.SetObject(ref _warriors);
                EUtility.SetObject(ref _colonies);
                EUtility.SetObject(ref _ports);
                EUtility.SetObject(ref _shrines);
                EUtility.SetObject(ref _roads);
                EUtility.SetObject(ref _currencies);
                EUtility.SetObject(ref _blood);
                EUtility.SetObject(ref _score);
                EUtility.SetObject(ref _artefactPanel); 

                for (int i = 0; i < EdificeId.Count; i++)
                {
                    if (i > 0 && _sprites[i] == null)
                        _sprites[i] = EUtility.FindAnyAsset<Sprite>($"SP_Icon{EdificeId.Names_Ed[i]}");
                }
            }
        }
    }
}

#endif
