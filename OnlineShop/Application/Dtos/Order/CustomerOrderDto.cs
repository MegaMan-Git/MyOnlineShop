using Domain.Enums;

namespace Application.Dtos.Order
{
    /// <summary>
    /// مدل نمایش سفارش برای مشتری
    /// </summary>
    public class CustomerOrderDto
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// وضعیت سفارش
        /// </summary>
        public OrderStatus Status { get; set; }
        /// <summary>
        /// تاریخ ایجاد سفارش
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
