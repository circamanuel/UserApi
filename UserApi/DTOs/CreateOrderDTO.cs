namespace UserApi.DTOs
{
    public class CreateOrderDTO
    {
        public int UserId { get; set; }
        public int TotalPieces { get; set; }
        public List<int> ProductIds { get; set; } = new();
    }
}
