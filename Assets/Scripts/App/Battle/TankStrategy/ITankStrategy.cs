namespace App.Battle.TankStrategy
{
    public interface ITankStrategy
    {
        ICombatant ChooseTank(ICombatTeam team);
    }
}
