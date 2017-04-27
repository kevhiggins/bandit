namespace App.Unit
{
    public abstract class AbstractUnit : AppMonoBehavior, IUnit
    {
        public void Despawn()
        {
            Destroy(gameObject);
        }
    }
}
