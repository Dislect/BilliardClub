namespace BilliardClub.Models.AbstractFactory
{
    public interface AbstractFactory
    {
        public TableRotation CreateRotation();
        public TypeTable CreateType();
    }
}