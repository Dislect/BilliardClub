using System;

namespace BilliardClub.Models.AbstractFactory
{
    public class RusFactory : AbstractFactory
    {
        public TableRotation CreateRotation()
        {
            return new RusRotation();
        }

        public TypeTable CreateType()
        {
            return new RusType();
        }
    }
}
