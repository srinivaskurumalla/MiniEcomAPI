namespace MiniEcom.Dtos
{
    public class AddressDto
    {
        public int Id { get; set; }
        public string? Label { get; set; }
        public string RecipientName { get; set; } = null!;
        public string Line1 { get; set; } = null!;
        public string? Line2 { get; set; }
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string? PostalCode { get; set; }
        public string? Phone { get; set; }
        public bool IsDefault { get; set; }
    }

}
