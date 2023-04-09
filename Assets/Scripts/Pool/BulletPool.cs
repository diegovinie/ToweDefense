public class BulletPool : GenericPool<Bullet>{}

// public interface IBulledPooled
// {
//     public BulletPool Pool { get; set; }
// }

public interface IBulletPooled : IGenericPooled<BulletPool>{}
