using System.Collections;
using System.Collections.Generic;

public static class ObjectPoolManager
{
    private static List<Bullet> _bulletPool = new List<Bullet>();
    private static List<EnemyAIController2D> _enemyAIPool = new List<EnemyAIController2D>();
}