//Assets\Colonization\Scripts\Controllers\Interface\ISelectable.cs
namespace Vurbiri.Colonization
{
    public interface ISelectable
    {
        public void Select();
        public void Unselect(ISelectable newSelectable);
    }
}
