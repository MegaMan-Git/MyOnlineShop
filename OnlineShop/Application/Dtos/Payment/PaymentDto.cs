using Domain.Enums;

namespace Application.Dtos.Payment
{
    /// <summary>
    /// مدل نمایش اطلاعات پرداخت
    /// </summary>
    public class PaymentDto
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// شناسه سفارش
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// مبلغ پرداخت
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// وضعیت پرداخت
        /// </summary>
        public PaymentStatus Status { get; set; }
        /// <summary>
        /// تاریخ ایجاد پرداخت
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// شناسه پرداخت درگاه
        /// </summary>
        public string? Authority { get; set; }
        /// <summary>
        /// شناسه پیگیری پرداخت
        /// </summary>
        public string? ReferenceId { get; set; }
    }
}
