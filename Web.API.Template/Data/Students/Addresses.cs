namespace Web.API.Template.Data.Students
{
    public class Address
    {
        public int AddressId { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string City { get; set; }

        public int StudentId { get; set; }
        public virtual Student student { get; set; }
    }
}
