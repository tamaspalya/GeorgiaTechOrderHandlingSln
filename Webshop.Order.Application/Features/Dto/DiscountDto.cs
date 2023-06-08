using EnsureThat;

namespace Webshop.Order.Application.Features.Dto
{
    public class DiscountDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public int Value { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
