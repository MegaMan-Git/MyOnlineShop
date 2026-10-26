using Domain.Enums;

namespace Domain.Common
{
    public class ServiceResult<T>
    {
        public ResultStatusCode StatusCode { get; set; }
        public string? Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public T? Data { get; set; }
    }
}
