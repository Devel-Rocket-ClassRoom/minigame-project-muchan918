public interface IAttacker
{
    int Damage { get; }
    void Attack(IDefender target);
}
