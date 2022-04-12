
public class SkeletonEnemy : EnemyAIController2D
{
   public override void  Start()
    {
        //Set enemy specific values
        _movementSpeed = 5.0f;
        _attackRange = .75f;
        _enemyPointValue = 50;
        _healthSystem = new HealthSystem(1);

        base.Start();
    }
}
