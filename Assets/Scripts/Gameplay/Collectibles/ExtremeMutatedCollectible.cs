
public class ExtremeMutatedCollectible : MutatedCollectible
{
    public ExtremeMutatedCollectible() : base()
    {
        UpdateMechanic();
    }

    private void UpdateMechanic()
    {
        _body.name = _name = "ExtremeMutatedCollectible";
        _collider.height = 7;
        _collider.direction = 0;
    }
}
