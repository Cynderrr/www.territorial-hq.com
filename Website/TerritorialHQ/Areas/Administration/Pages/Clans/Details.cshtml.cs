using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TerritorialHQ.Models;
using TerritorialHQ.Services;

namespace TerritorialHQ.Areas.Administration.Pages.Clans
{
    [Authorize(Roles = "Administrator, Staff")]
    public class DetailsModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly LoggerService _logger;
        private readonly ClanService _service;

        public DetailsModel(IMapper mapper, LoggerService logger, ClanService service)
        {
            _mapper = mapper;
            _logger = logger;
            _service = service;
        }

        public Clan Clan { get; set; }

        public async Task<IActionResult>
            OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Clan = await _service.FindAsync(id);

            if (Clan == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
