interface IFuelConsumable
{
    void IntakeFuel(float amount);
    void ConsumeFuel(float amount);
    void FuelLimiter();
    void FullLoadFuel();
    void EmptyFuel();

}
