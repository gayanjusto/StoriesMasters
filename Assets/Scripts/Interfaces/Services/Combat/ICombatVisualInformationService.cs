using Assets.Scripts.Entities.ApplicationObjects;

namespace Assets.Scripts.Interfaces.Services
{
    public interface ICombatVisualInformationService
    {
        void HighlightAttackerForPlayer(BaseAppObject attacker);
        void RemoveHighlightAttackerInformation(BaseAppObject attacker);
    }
}
