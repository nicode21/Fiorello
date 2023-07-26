using Fiorello_backend.ViewModels;

namespace Fiorello_backend.Services.Interfaces
{
    public interface ILayoutService
    {
        Task<LayoutVM> GetAllDatas();
    }
}
