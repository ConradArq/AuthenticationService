using System.ComponentModel;

namespace AuthenticationService.Shared.Dtos
{
    public class PaginationRequestDto : RequestDto
    {
        [DefaultValue(1)]
        public int PageNumber { get; set; } = 1;

        [DefaultValue(10)]
        public int PageSize { get; set; } = 10;
    }
}
