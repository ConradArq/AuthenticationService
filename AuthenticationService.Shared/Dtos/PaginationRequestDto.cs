using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Shared.Dtos
{
    public class PaginationRequestDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "The page number must be greater than 0.")]
        [DefaultValue(1)]
        public int PageNumber { get; set; } = 1;
        [Range(1, int.MaxValue, ErrorMessage = "The page number must be greater than 0.")]
        [DefaultValue(10)]
        public int PageSize { get; set; } = 10;
    }
}
