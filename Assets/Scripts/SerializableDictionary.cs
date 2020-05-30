using System;
using Characters;

[Serializable]
public class EnemyAmountDictionary : SerializableDictionary<Enemy, int>
{
}

[Serializable]
public class ConsumableAmountDictionary : SerializableDictionary<Consumable, int>
{
}