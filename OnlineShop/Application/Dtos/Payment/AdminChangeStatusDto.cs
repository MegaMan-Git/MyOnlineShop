namespace Application.Dtos.Payment
{
    /// <summary>
    /// مدل تغییر وضعیت پرداخت
    /// </summary>
    public class ChangePaymentStatusDto
    {
        /// <summary>
        /// شناسه سفارش
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// وضعیت پرداخت
        /// </summary>
        public string Status { get; set; } = string.Empty;
    }
}
