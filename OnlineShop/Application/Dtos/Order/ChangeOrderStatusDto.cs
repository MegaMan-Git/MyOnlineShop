namespace Application.Dtos.Order
{
    /// <summary>
    /// مدل تغییر وضعیت سفارش
    /// </summary>
    public class ChangeOrderStatusDto
    {
        /// <summary>
        /// شناسه سفارش
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// وضعیت سفارش
        /// </summary>
        public string Status { get; set; } = string.Empty;
    }
}
