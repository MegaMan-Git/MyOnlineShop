using Domain.Enums;

namespace Application.Dtos.Order
{
    /// <summary>
    /// مدل نمایش سفارش برای مدیر
    /// </summary>
    public class AdminOrderDto
    {
        /// <summary>
        /// شناسه سفارش
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// نام کاربری
        /// </summary>
        public string? UserName { get; set; }
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
