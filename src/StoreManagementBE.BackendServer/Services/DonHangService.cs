using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Models;
using StoreManagementBE.BackendServer.Models.Entities;
using StoreManagementBE.BackendServer.Services.Interfaces;

namespace StoreManagementBE.BackendServer.Services
{
    public class DonHangService : IDonHangService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DonHangService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IQueryable<DonHang> ApplyFilter(OrderFilterDTO filter)
        {
            var q = _context.DonHangs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Keyword))
            {
                var kw = filter.Keyword.Trim();

                if (int.TryParse(kw, out var id))
                    q = q.Where(x => x.OrderId == id);
                else if (decimal.TryParse(kw, out var money))
                    q = q.Where(x => (x.TotalAmount ?? 0) == money);
                else
                    q = q.Where(x => x.OrderDate.HasValue &&
                                     x.OrderDate.Value.ToString("yyyy-MM-dd").Contains(kw));
            }

            if (filter.DateFrom.HasValue)
                q = q.Where(x => x.OrderDate >= filter.DateFrom);

            if (filter.DateTo.HasValue)
            {
                var end = filter.DateTo.Value.Date.AddDays(1).AddTicks(-1);
                q = q.Where(x => x.OrderDate <= end);
            }

            if (filter.MinTotal.HasValue)
                q = q.Where(x => (x.TotalAmount ?? 0) >= filter.MinTotal.Value);

            if (filter.MaxTotal.HasValue)
                q = q.Where(x => (x.TotalAmount ?? 0) <= filter.MaxTotal.Value);

            return q;
        }

        public async Task<PagedResult<DonHangDTO>> GetAll(int page, int pageSize, OrderFilterDTO filter)
        {
            var query = ApplyFilter(filter);

            var total = await query.CountAsync();

            var list = await query
                .OrderByDescending(x => x.OrderId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Items)
                .Include(x => x.Payments)
                .ToListAsync();

            return new PagedResult<DonHangDTO>
            {
                Data = _mapper.Map<List<DonHangDTO>>(list),
                Total = total,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<DonHangDTO?> GetById(int orderId)
        {
            var e = await _context.DonHangs
                .Include(x => x.Items)
                .Include(x => x.Payments)
                .FirstOrDefaultAsync(x => x.OrderId == orderId);

            // Luôn return ở mọi nhánh
            if (e == null) return null;
            return _mapper.Map<DonHangDTO>(e);
        }
    }
}
