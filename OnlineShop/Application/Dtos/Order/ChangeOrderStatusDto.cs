namespace Application.Dtos.Order
{
    public class ChangeOrderStatusDto
    {
        public int OrderId { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
