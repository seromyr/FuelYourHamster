interface IFuelConsumable
{
    void IsConsumingFuel(bool value);
    void IntakeFuel(float amount);
    void ConsumeFuel(float amount);
    void FuelLimiter();
    void FullLoadFuel();
    void EmptyFuel();
}
