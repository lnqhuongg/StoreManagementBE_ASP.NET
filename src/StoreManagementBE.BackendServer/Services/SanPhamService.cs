using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
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
            try
            {
                var list = await _context.SanPhams.Include(x => x.Category).Include(x => x.Supplier).ToListAsync();
                return _mapper.Map<List<SanPhamDTO>>(list);
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi lấy danh sách sản phẩm: " + e.Message);
            }
        }
        public async Task<SanPhamDTO> GetById(int id)
        {
            try
            {
                var sp = await _context.SanPhams.Include(x => x.Category).Include(x => x.Supplier).FirstOrDefaultAsync(x => x.ProductID == id);
                return _mapper.Map<SanPhamDTO>(sp);
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi lấy sản phẩm theo ID: " + e.Message);
            }
        }

        public async Task<bool> checkExistBarcode(string barcode)
        {
            bool barcodeExists = await _context.SanPhams
                .AnyAsync(x => x.Barcode == barcode);

            return barcodeExists;
        }

        public async Task<bool> checkExistID(int ID)
        {
            bool exists = await _context.SanPhams
                .AnyAsync(x => x.ProductID == ID);

            return exists;
        }

        public async Task<SanPhamDTO> Create(SanPhamDTO sp)
        {
            try
            {

                // Tạo entity sản phẩm mới
                SanPham sanpham = new SanPham
                {
                    ProductName = sp.ProductName,
                    Barcode = sp.Barcode,
                    Price = sp.Price,
                    Unit = sp.Unit,
                    CreatedAt = DateTime.Now,
                    Status = sp.Status,

                    CategoryId = sp.Category?.CategoryId,
                    SupplierId = sp.Supplier?.SupplierId,
                };


                _context.SanPhams.Add(sanpham);
                await _context.SaveChangesAsync();

                // Load lại sản phẩm với đầy đủ thông tin liên quan
                var createdProduct = await _context.SanPhams
                    .Include(x => x.Category)
                    .Include(x => x.Supplier)
                    .FirstOrDefaultAsync(x => x.ProductID == sanpham.ProductID);

                var resultDTO = _mapper.Map<SanPhamDTO>(createdProduct);

                return resultDTO;
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi thêm sản phẩm: " + e.Message);
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var sanpham = await _context.SanPhams
                    .FirstOrDefaultAsync(x => x.ProductID == id);


                if (sanpham != null)
                {
                    _context.SanPhams.Remove(sanpham);

                    // SỬA: Dùng async
                    await _context.SaveChangesAsync();

                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi xóa sản phẩm: " + e.Message);
            }
        }
        public async Task<SanPhamDTO> Update(SanPhamDTO sp)
        {
            try
            {
                // SỬA: Thêm await
                var existingProduct = await _context.SanPhams
                    .FirstOrDefaultAsync(x => x.ProductID == sp.ProductID);

                if (existingProduct == null)
                    return null;



                // QUAN TRỌNG: Update từng property thay vì dùng AutoMapper
                existingProduct.ProductName = sp.ProductName;
                existingProduct.Barcode = sp.Barcode;
                existingProduct.Price = sp.Price;
                existingProduct.Unit = sp.Unit;
                existingProduct.Status = sp.Status;

                // CHỈ update ID, không update navigation objects
                existingProduct.CategoryId = sp.Category?.CategoryId;
                existingProduct.SupplierId = sp.Supplier?.SupplierId;

                // KHÔNG cần gọi Update() vì Entity Framework đang track entity
                await _context.SaveChangesAsync();

                // Load lại với đầy đủ thông tin để trả về
                var updatedProduct = await _context.SanPhams
                    .Include(x => x.Category)
                    .Include(x => x.Supplier)
                    .FirstOrDefaultAsync(x => x.ProductID == sp.ProductID);

                var resultDTO = _mapper.Map<SanPhamDTO>(updatedProduct);

                return resultDTO;
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi cập nhật sản phẩm: " + e.Message);
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
            try
            {
                var list = await _context.SanPhams.Include(sp => sp.Category)
                .Include(sp => sp.Supplier).Where(x => x.SupplierId == supplier_id).ToListAsync();
                return _mapper.Map<List<SanPhamDTO>>(list);
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi tìm sản phẩm theo nhà cung cấp: " + e.Message);
            }
        }
        public async Task<List<SanPhamDTO>> getByCategoryID(int? category_id)
        {
            try
            {
                var list = await _context.SanPhams.Include(sp => sp.Category)
                .Include(sp => sp.Supplier).Where(x => x.CategoryId == category_id).ToListAsync();
                return _mapper.Map<List<SanPhamDTO>>(list);
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi tìm sản phẩm theo loại: " + e.Message);
            }
        }
        public async Task<List<SanPhamDTO>> getProductsSortByPrice(string? order)
        {
            try
            {
                var query = _context.SanPhams.Include(sp => sp.Category).Include(sp => sp.Supplier);
                if (order.ToLower() == "desc")
                {
                    var list = await query.OrderByDescending(sp => sp.Price).ToListAsync();
                    return _mapper.Map<List<SanPhamDTO>>(list);
                }
                var ls = await query.OrderBy(sp => sp.Price).ToListAsync();
                return _mapper.Map<List<SanPhamDTO>>(ls);
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi lấy danh sách sắp xếp theo yêu cầu: " + e.Message);
            }
        }
        public async Task<List<SanPhamDTO>> getProductsBySupplierIDAndCategoryID(int? supplier_id, int? category_id)
        {
            try
            {
                var ls = await _context.SanPhams.Include(sp => sp.Category)
            .Include(sp => sp.Supplier).Where(x => x.CategoryId == category_id && x.SupplierId == supplier_id).ToListAsync();
                return _mapper.Map<List<SanPhamDTO>>(ls);
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi tìm sản phẩm theo nhà cung cấp và loại: " + e.Message);
            }
        }
        public async Task<List<SanPhamDTO>> getProductsBySupplierIDAndPrice(int? supplier_id, string? order)
        {
            try
            {
                var query = _context.SanPhams.Include(sp => sp.Category).Include(sp => sp.Supplier).Where(x => x.SupplierId == supplier_id);
                if (order != "")
                {
                    if (order.ToLower() == "desc")
                    {
                        var ls = await query.OrderByDescending(sp => sp.Price).ToListAsync();
                        return _mapper.Map<List<SanPhamDTO>>(ls);
                    }
                }
                else
                {
                    var ls = await query.OrderBy(sp => sp.Price).ToListAsync();
                    return _mapper.Map<List<SanPhamDTO>>(ls);
                }

                var list = await query.OrderBy(sp => sp.Price).ToListAsync();
                return _mapper.Map<List<SanPhamDTO>>(list);
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi lấy danh sách sản phẩm sắp xếp theo nhà cung cấp: " + e.Message);
            }
        }
        public async Task<List<SanPhamDTO>> getProductsByCategoryIDAndPrice(int? category_id, string? order)
        {
            try
            {
                var query = _context.SanPhams.Include(sp => sp.Category).Include(sp => sp.Supplier).Where(x => x.CategoryId == category_id);
                if (order != "")
                {
                    if (order.ToLower() == "desc")
                    {
                        var ls = await query.OrderByDescending(sp => sp.Price).ToListAsync();
                        return _mapper.Map<List<SanPhamDTO>>(ls);
                    }
                }
                else
                {
                    var ls = await query.OrderBy(sp => sp.Price).ToListAsync();
                    return _mapper.Map<List<SanPhamDTO>>(ls);
                }

                var list = await query.OrderBy(sp => sp.Price).ToListAsync();
                return _mapper.Map<List<SanPhamDTO>>(list);
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi lấy danh sách sản phẩm theo yêu cầu và loại: " + e.Message);
            }
        }
        public async Task<List<SanPhamDTO>> getProductsBysupplierIDAndCategoryIDAndPrice(int? supplier_id, int? category_id, string? order)
        {
            try
            {
                var query = _context.SanPhams.Include(sp => sp.Category).Include(sp => sp.Supplier).Where(x => x.CategoryId == category_id && x.SupplierId == supplier_id);
                if (order != "")
                {
                    var ls = await query.OrderBy(sp => sp.Price).ToListAsync();
                    return _mapper.Map<List<SanPhamDTO>>(ls);
                }
                else
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
            catch (Exception e)
            {
                throw new Exception("Lỗi khi lấy danh sách sản phẩm theo yêu cầu, nhà cung cấp, loại: " + e.Message);
            }
        }
    }
}