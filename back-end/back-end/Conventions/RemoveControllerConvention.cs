using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace back_end.Conventions
{
    public class RemoveControllerConvention : IApplicationModelConvention
    {
        private readonly string _controllerName;

        public RemoveControllerConvention(string controllerName)
        {
            _controllerName = controllerName;
        }

        public void Apply(ApplicationModel app)
        {
            ControllerModel? controller = app.Controllers
                .FirstOrDefault(c => c.ControllerName == _controllerName);

            if (controller != null)
                app.Controllers.Remove(controller);
        }
    }
}