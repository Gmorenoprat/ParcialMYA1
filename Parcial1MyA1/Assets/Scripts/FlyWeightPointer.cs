public static class FlyWeightPointer
{
    public static readonly FlyWeight Asteroid = new FlyWeight
    {
        speed = 2
    };
    public static readonly FlyWeight MiniAsteroid = new FlyWeight
    {
        speed = 1
    };

}
