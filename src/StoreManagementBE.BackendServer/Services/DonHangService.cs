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

        // ==================== 1. LỌC DỮ LIỆU (Tương tự SearchByKeyword) ====================
        public IQueryable<DonHang> ApplyFilter(OrderFilterDTO filter)
        {
            // 1. Include Khách hàng và Nhân viên ngay từ đầu để tìm kiếm
            var q = _context.DonHangs
                .Include(x => x.Customer) // Để tìm theo tên khách
                .Include(x => x.User)     // Để tìm theo tên nhân viên (nếu có quan hệ)
                .AsQueryable();

            // 2. Logic tìm kiếm đa năng (Keyword)
            if (!string.IsNullOrWhiteSpace(filter.Keyword))
            {
                var kw = filter.Keyword.Trim().ToLower(); // Chuyển về chữ thường để tìm không phân biệt hoa thường

                // Tìm theo: Mã đơn OR Tên khách OR Tên nhân viên
                q = q.Where(x =>
                    x.OrderId.ToString().Contains(kw) ||
                    (x.Customer != null && x.Customer.Name.ToLower().Contains(kw)) ||
                    (x.User != null && x.User.FullName.ToLower().Contains(kw)) // Giả sử User có cột FullName
                );
            }

            // 3. Lọc theo ngày
            if (filter.DateFrom.HasValue)
                q = q.Where(x => x.OrderDate >= filter.DateFrom);

            if (filter.DateTo.HasValue)
            {
                var end = filter.DateTo.Value.Date.AddDays(1).AddTicks(-1);
                q = q.Where(x => x.OrderDate <= end);
            }

            // 4. Lọc theo tổng tiền
            if (filter.MinTotal.HasValue)
                q = q.Where(x => (x.TotalAmount ?? 0) >= filter.MinTotal.Value);

            if (filter.MaxTotal.HasValue)
                q = q.Where(x => (x.TotalAmount ?? 0) <= filter.MaxTotal.Value);

            return q;
        }

        // ==================== 2. LẤY DANH SÁCH (Phân trang) ====================
        public async Task<PagedResult<DonHangDTO>> GetAll(int page, int pageSize, OrderFilterDTO filter)
        {
            // Bước 1: Gọi hàm lọc ở trên
            var query = ApplyFilter(filter);

            // Bước 2: Đếm tổng số bản ghi thỏa mãn điều kiện lọc
            var total = await query.CountAsync();

            // Bước 3: Phân trang và lấy dữ liệu
            var list = await query
                .OrderByDescending(x => x.OrderDate) // Đơn mới nhất lên đầu
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Items)      // Quan trọng: Lấy kèm chi tiết đơn hàng
                .Include(x => x.Payments)   // Quan trọng: Lấy kèm lịch sử thanh toán
                .ToListAsync();

            // Bước 4: Map sang DTO và trả về
            return new PagedResult<DonHangDTO>
            {
                Data = _mapper.Map<List<DonHangDTO>>(list),
                Total = total,
                Page = page,
                PageSize = pageSize
            };
        }

        // ==================== 3. LẤY CHI TIẾT (GetById) ====================
        public async Task<DonHangDTO?> GetById(int orderId)
        {
            var e = await _context.DonHangs
                .Include(x => x.Items)
                .Include(x => x.Payments)
                .FirstOrDefaultAsync(x => x.OrderId == orderId);

            if (e == null) return null;

            return _mapper.Map<DonHangDTO>(e);
        }

        // ==================== 4. TẠO MỚI (Create) ====================
        public async Task<DonHangDTO> Create(DonHangDTO donHangDTO)
        {
            try
            {
                // Map từ DTO sang Entity
                // Entity Framework sẽ tự động thêm cả các dòng trong bảng OrderItems 
                // nếu DTO có chứa danh sách Items
                var orderEntity = _mapper.Map<DonHang>(donHangDTO);

                // Gán ngày tạo là hiện tại nếu người dùng không gửi lên
                if (orderEntity.OrderDate == null || orderEntity.OrderDate == DateTime.MinValue)
                {
                    orderEntity.OrderDate = DateTime.Now;
                }
                if (orderEntity.Items != null)
                {
                    foreach (var item in orderEntity.Items)
                    {
                        // QUAN TRỌNG: Set Product = null để EF không cố tạo ra sản phẩm mới
                        item.Product = null;

                        // Đảm bảo ProductId được giữ nguyên
                        // (item.ProductId đã có giá trị từ DTO gửi xuống)
                    }
                }

                _context.DonHangs.Add(orderEntity);
                await _context.SaveChangesAsync();

                // Trả về kết quả sau khi lưu (để lấy được OrderId vừa sinh ra)
                return _mapper.Map<DonHangDTO>(orderEntity);
            }
            catch (Exception ex)
            {
                // Lấy lỗi sâu nhất bên trong (InnerException) - nơi chứa thông báo SQL
                var rawError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                throw new Exception("Chi tiết lỗi SQL: " + rawError);
            }
        }
    }
}