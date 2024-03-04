using HelloJob.App.ViewModels;
using HelloJob.Entities.DTOS;
using Microsoft.AspNetCore.Mvc;

namespace HelloJob.App.Controllers
{
    public class LayoutController : Controller
    {
        public IActionResult _Layout(RegisterDto register_dto,LoginDto login_dto)
        {
            LayoutVM vm = new LayoutVM
            {
                Registerdto = register_dto,
                Logindto = login_dto
            };
            return View(vm);
        }
    }
}
