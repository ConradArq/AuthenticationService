using MediatR;
using AuthenticationService.Shared.Dtos;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AuthenticationService.Application.Features.ApplicationMenu.Commands.Update
{
    public class UpdateApplicationMenuCommand : IRequest<ResponseDto<ApplicationMenuResponse>>
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string? Title { get; set; } = string.Empty;
        public string? Path { get; set; }
        public string? IconType { get; set; }
        public string? Icon { get; set; }
        public string? Class { get; set; }
        public bool? GroupTitle { get; set; }
        public string? Badge { get; set; }
        public string? BadgeClass { get; set; }
        public int? Order { get; set; }

        public int? ParentApplicationMenuId { get; set; }

        [DefaultValue((int)Domain.Enums.Status.Active)]
        public int StatusId { get; set; } = (int)Domain.Enums.Status.Active;
    }
}
