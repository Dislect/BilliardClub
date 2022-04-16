namespace BilliardClub.Models.AbstractFactory
{
    public class EnType : TypeTable
    {
        public EnType() : base()
        {
            this.name = "Американский пул";
            this.price = 300;
            this.picturePath = "/img/poolb.png";
        }
    }
}
