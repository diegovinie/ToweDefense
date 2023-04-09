public class BulletPool : GenericPool<Bullet>{}

public interface IBulledPooled
{
    public BulletPool Pool { get; set; }
}
