using HelloJob.Entities.DTOS;
using HelloJob.Service.Responses;

namespace HelloJob.App.ViewModels
{
    public class HomeVM
    {
        public PagginatedResponse<BlogGetDto> blogs { get; set; }
    }
}
