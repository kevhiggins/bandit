namespace App.Unit
{
    public abstract class AbstractUnit : AppMonoBehavior, IUnit
    {
        public virtual void Despawn()
        {
            Destroy(gameObject);
        }
    }
}
