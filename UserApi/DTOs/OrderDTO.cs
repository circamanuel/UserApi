namespace UserApi.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TotalPieces { get; set; }
        public decimal TotalPrice { get; set; }
        public List<ProductDTO> Products { get; set; } = new();
    }

}
