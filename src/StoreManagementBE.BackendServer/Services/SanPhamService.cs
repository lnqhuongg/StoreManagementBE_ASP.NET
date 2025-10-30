using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Enum;
using StoreManagementBE.BackendServer.Models;
using StoreManagementBE.BackendServer.Models.Entities;
using StoreManagementBE.BackendServer.Services.Interfaces;
using System.Threading.Tasks;

namespace StoreManagementBE.BackendServer.Services
{
    public class SanPhamService : ISanPhamService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SanPhamService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<SanPhamDTO>> GetAll()
        {
            var list = await _context.SanPhams.Include(x => x.Category).Include(x => x.Supplier).ToListAsync();
            return _mapper.Map<List<SanPhamDTO>>(list);
        }
        public async Task<SanPhamDTO> GetById(int id)
        {
            var sp = await _context.SanPhams.Include(x => x.Category).Include(x => x.Supplier).FirstOrDefaultAsync(x => x.ProductID == id);
            return _mapper.Map<SanPhamDTO>(sp);
        }


        public async Task<ApiResponse<SanPhamDTO>> Create(SanPhamDTO sp)
        {
            try
            {
                // Kiểm tra trùng ProductID
                bool exists = await _context.SanPhams
                    .AnyAsync(x => x.ProductID == sp.ProductID);

                Console.WriteLine(sp.ProductName);

                if (exists)
                {
                    return new ApiResponse<SanPhamDTO>
                    {
                        Success = false,
                        Message = "Sản phẩm đã tồn tại!"
                    };
                }

                // Kiểm tra trùng Barcode (nếu cần)
                bool barcodeExists = await _context.SanPhams
                    .AnyAsync(x => x.Barcode == sp.Barcode);

                if (barcodeExists)
                {
                    return new ApiResponse<SanPhamDTO>
                    {
                        Success = false,
                        Message = "Mã vạch đã tồn tại!"
                    };
                }

                // Tạo entity sản phẩm mới
                SanPham sanpham = new SanPham
                {
                    ProductName = sp.ProductName,
                    Barcode = sp.Barcode,
                    Price = sp.Price,
                    Unit = sp.Unit,
                    CreatedAt = DateTime.Now,
                    Status = sp.Status,

                    // QUAN TRỌNG: Chỉ set ID, KHÔNG set navigation properties
                    CategoryID = sp.Category?.Category_id,
                    SupplierID = sp.Supplier?.supplier_id,
                    //Category = sp.Category,
                    //Supplier = sp.Supplier
                };


                _context.SanPhams.Add(sanpham);
                await _context.SaveChangesAsync();

                // Load lại sản phẩm với đầy đủ thông tin liên quan
                var createdProduct = await _context.SanPhams
                    .Include(x => x.Category)
                    .Include(x => x.Supplier)
                    .FirstOrDefaultAsync(x => x.ProductID == sanpham.ProductID);

                var resultDTO = _mapper.Map<SanPhamDTO>(createdProduct);

                return new ApiResponse<SanPhamDTO>
                {
                    Message = "Thêm mới sản phẩm thành công!",
                    DataDTO = resultDTO, // Sửa thành Data thay vì DataDTO
                    Success = true
                };
            }
            catch (Exception e)
            {
                return new ApiResponse<SanPhamDTO>
                {
                    Message = "Lỗi khi thêm mới sản phẩm: " + e.Message,
                    Success = false
                };
            }
        }

        public async Task<ApiResponse<bool>> Delete(int id)
        {
            try
            {
                // SỬA: Tìm entity trực tiếp, không dùng GetById
                var sanpham = await _context.SanPhams
                    .FirstOrDefaultAsync(x => x.ProductID == id);

                if (sanpham != null)
                {
                    // QUAN TRỌNG: Xóa trực tiếp entity, không dùng AutoMapper
                    _context.SanPhams.Remove(sanpham);

                    // SỬA: Dùng async
                    await _context.SaveChangesAsync();

                    return new ApiResponse<bool>
                    {
                        Message = "Xóa sản phẩm thành công!",
                        DataDTO = true, 
                        Success = true
                    };
                }
                else
                {
                    return new ApiResponse<bool>
                    {
                        Message = "Không tìm thấy sản phẩm cần xóa!",
                        Success = false
                    };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Delete error: {e.Message}");

                return new ApiResponse<bool>
                {
                    Message = "Lỗi khi xóa sản phẩm: " + e.Message,
                    Success = false
                };
            }
        }
        public async Task<ApiResponse<SanPhamDTO>> Update(SanPhamDTO sp)
        {
            try
            {
                // SỬA: Thêm await
                var existingProduct = await _context.SanPhams
                    .FirstOrDefaultAsync(x => x.ProductID == sp.ProductID);

                if (existingProduct == null)
                    return new ApiResponse<SanPhamDTO>
                    {
                        Message = "Không tìm thấy sản phẩm cần cập nhật!",
                        Success = false
                    };

                // SỬA: Thêm await
                bool duplicateBarcode = await _context.SanPhams
                    .AnyAsync(x => x.Barcode == sp.Barcode && x.ProductID != sp.ProductID);

                if (duplicateBarcode)
                    return new ApiResponse<SanPhamDTO>
                    {
                        Message = "Mã vạch đã tồn tại, vui lòng nhập mã khác!",
                        Success = false
                    };

                // QUAN TRỌNG: Update từng property thay vì dùng AutoMapper
                existingProduct.ProductName = sp.ProductName;
                existingProduct.Barcode = sp.Barcode;
                existingProduct.Price = sp.Price;
                existingProduct.Unit = sp.Unit;
                existingProduct.Status = sp.Status;

                // CHỈ update ID, không update navigation objects
                existingProduct.CategoryID = sp.Category?.Category_id;
                existingProduct.SupplierID = sp.Supplier?.supplier_id;

                // KHÔNG cần gọi Update() vì Entity Framework đang track entity
                await _context.SaveChangesAsync();

                // Load lại với đầy đủ thông tin để trả về
                var updatedProduct = await _context.SanPhams
                    .Include(x => x.Category)
                    .Include(x => x.Supplier)
                    .FirstOrDefaultAsync(x => x.ProductID == sp.ProductID);

                var resultDTO = _mapper.Map<SanPhamDTO>(updatedProduct);

                return new ApiResponse<SanPhamDTO>
                {
                    Message = "Cập nhật sản phẩm thành công!",
                    DataDTO = resultDTO, // Sửa thành Data thay vì DataDTO
                    Success = true
                };
            }
            catch (Exception e)
            {
                return new ApiResponse<SanPhamDTO>
                {
                    Message = "Lỗi khi cập nhật sản phẩm: " + e.Message,
                    Success = false
                };
            }
        }

        //public bool UpdateStatus(int id)
        //{
        //    try
        //    {
        //        SanPham sanpham = _context.SanPhams.Find(id);
        //        if (sanpham != null)
        //        {
        //            Console.WriteLine("id sp update status: ", sanpham.product_id);
        //            sanpham.status = (sanpham.status == 1) ? 0 : 1;

        //            _context.SaveChanges();
        //            return true;
        //        }
        //        else
        //        {
        //            throw new Exception("Không tìm thấy sản phẩm cần cập nhật!");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Lỗi khi cập nhật: {ex.Message}");
        //    }
        //}
        public async Task<List<SanPhamDTO>> searchByKeyword(string keyword)
        {
            //return _context.SanPhams
            //    .Include(sp => sp.Category)
            //    .Include(sp => sp.Supplier)
            //    .Where(x => x.product_name.ToLower().Contains(keyword.ToLower()))
            //    .ToList();
            try
            {
                var list = await _context.SanPhams
                            .Include(sp => sp.Category)
                            .Include(sp => sp.Supplier)
                            .Where(x => x.ProductName.ToLower().Contains(keyword.ToLower()))
                            .ToListAsync();
                return _mapper.Map<List<SanPhamDTO>>(list);
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi tìm sản phẩm theo keyword: " + e.Message);
            }
        }
        public async Task<List<SanPhamDTO>> getBySupplierID(int? supplier_id)
        {
            //return _context.SanPhams.Include(sp => sp.Category)
            //.Include(sp => sp.Supplier).Where(x => x.supplier_id == supplier_id).ToList();
            var list = await _context.SanPhams.Include(sp => sp.Category)
            .Include(sp => sp.Supplier).Where(x => x.SupplierID == supplier_id).ToListAsync();
            return _mapper.Map<List<SanPhamDTO>>(list);
        }
        public async Task<List<SanPhamDTO>> getByCategoryID(int? category_id)
        {
            var list = await _context.SanPhams.Include(sp => sp.Category)
            .Include(sp => sp.Supplier).Where(x => x.CategoryID == category_id).ToListAsync();
            return _mapper.Map<List<SanPhamDTO>>(list);
        }
        public async Task<List<SanPhamDTO>> getProductsSortByPrice(string? order)
        {
            var query = _context.SanPhams.Include(sp => sp.Category).Include(sp => sp.Supplier);
            if(order.ToLower() == "desc")
            {
                var list = await query.OrderByDescending(sp => sp.Price).ToListAsync();
                return _mapper.Map<List<SanPhamDTO>>(list);
            }
            var ls = await query.OrderBy(sp => sp.Price).ToListAsync();
            return _mapper.Map<List<SanPhamDTO>>(ls);
        }
        public async Task<List<SanPhamDTO>> getProductsBySupplierIDAndCategoryID(int? supplier_id, int? category_id)
        {
            var ls = await _context.SanPhams.Include(sp => sp.Category)
            .Include(sp => sp.Supplier).Where(x => x.CategoryID == category_id && x.SupplierID == supplier_id).ToListAsync();
            return _mapper.Map<List<SanPhamDTO>>(ls);
        }
        public async Task<List<SanPhamDTO>> getProductsBySupplierIDAndPrice(int? supplier_id, string? order)
        {
            var query = _context.SanPhams.Include(sp => sp.Category).Include(sp => sp.Supplier).Where(x => x.SupplierID == supplier_id);
            if(order != "")
            {
                if (order.ToLower() == "desc")
                {
                    var ls = await query.OrderByDescending(sp => sp.Price).ToListAsync();
                    return _mapper.Map<List<SanPhamDTO>>(ls);
                }
            } else
            {
                var ls = await query.OrderBy(sp => sp.Price).ToListAsync();
                return _mapper.Map<List<SanPhamDTO>>(ls);
            }

            var list = await query.OrderBy(sp => sp.Price).ToListAsync();
            return _mapper.Map<List<SanPhamDTO>>(list);
        }
        public async Task<List<SanPhamDTO>> getProductsByCategoryIDAndPrice(int? category_id, string? order)
        {
            var query = _context.SanPhams.Include(sp => sp.Category).Include(sp => sp.Supplier).Where(x => x.CategoryID == category_id);
            if(order != "")
            {
                if (order.ToLower() == "desc")
                {
                    var ls = await query.OrderByDescending(sp => sp.Price).ToListAsync();
                    return _mapper.Map<List<SanPhamDTO>>(ls);
                }
            } else
            {
                var ls = await query.OrderBy(sp => sp.Price).ToListAsync();
                return _mapper.Map<List<SanPhamDTO>>(ls);
            }

            var list = await query.OrderBy(sp => sp.Price).ToListAsync();
            return _mapper.Map<List<SanPhamDTO>>(list);
        }
        public async Task<List<SanPhamDTO>> getProductsBysupplierIDAndCategoryIDAndPrice(int? supplier_id, int? category_id, string? order)
        {
            var query = _context.SanPhams.Include(sp => sp.Category).Include(sp => sp.Supplier).Where(x => x.CategoryID == category_id && x.SupplierID == supplier_id);
            if(order != "")
            {
                var ls = await query.OrderBy(sp => sp.Price).ToListAsync();
                return _mapper.Map<List<SanPhamDTO>>(ls);
            } else
            {
                if (order.ToLower() == "desc")
                {
                    var ls = await query.OrderByDescending(sp => sp.Price).ToListAsync();
                    return _mapper.Map<List<SanPhamDTO>>(ls);
                }
            }
            var list = await query.OrderBy(sp => sp.Price).ToListAsync();
            return _mapper.Map<List<SanPhamDTO>>(list);
        }
    }
}
