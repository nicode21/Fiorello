using Fiorello_backend.Data;
using Fiorello_backend.Models;
using Fiorello_backend.Services.Interfaces;
using Fiorello_backend.ViewModels;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Linq;
using System.Security.Claims;

namespace Fiorello_backend.Services
{
    public class LayoutService : ILayoutService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBasketService _basketService;
        private readonly UserManager<AppUser> _userManager;

        public LayoutService(AppDbContext context,
                             IBasketService basketService,
                             IHttpContextAccessor httpContextAccessor,
                             UserManager<AppUser> userManager)
        {
            _context = context;
            _basketService = basketService;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<LayoutVM> GetAllDatas()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            int count = _basketService.GetCount();
            var datas = _context.Settings.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
            return new LayoutVM { BasketCount = count, SettingDatas = datas, UserFullName = user?.FullName};
        }
    }
}
