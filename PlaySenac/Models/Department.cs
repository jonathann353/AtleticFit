namespace PlaySenac.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Seller> Sellers { get; set; } 
            = new List<Seller>();
    }
}
