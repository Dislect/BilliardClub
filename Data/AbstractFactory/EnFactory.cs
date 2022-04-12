namespace BilliardClub.Models.AbstractFactory
{
    public class EnFactory : AbstractFactory
    {
        public TableRotation CreateRotation()
        {
            return new EnRotation();
        }

        public TypeTable CreateType()
        {
            return new EnType();
        }
    }
}