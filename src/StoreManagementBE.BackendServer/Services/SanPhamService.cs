using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using StoreManagementBE.BackendServer.DTOs.SanPhamDTO;
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
        private readonly IImageService _imageService;
        private static readonly char[] Base62Chars =
        "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();
        public SanPhamService(ApplicationDbContext context, IMapper mapper, IImageService imageService)
        {
            _context = context;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<List<SanPhamDTO>> GetAll()
        {
            try
            {
                var list = await _context.SanPhams.Include(x => x.Category).Include(x => x.Supplier).ToListAsync();
                return _mapper.Map<List<SanPhamDTO>>(list);
            } catch(Exception e)
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
            } catch(Exception e)
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

        public async Task<bool> checkBarcodeExistForOtherProducts(int id, string barcode)
        {
            var sanpham = await GetById(id);
            var exist = await _context.SanPhams.AnyAsync(x => x.Barcode == sanpham.Barcode && x.ProductID != sanpham.ProductID);
            return exist;
        }

        private static string ToBase62(ulong number)
        {
            var result = new char[8];
            int index = 7;

            do
            {
                result[index--] = Base62Chars[number % 62];
                number /= 62;
            } while (number > 0 && index >= 0);

            return new string(result, index + 1, 7 - index);
        }

        private string generateAutoBarcode()
        {
            var guid = Guid.NewGuid();
            var bytes = guid.ToByteArray();

            // Lấy 8 byte đầu từ GUID
            var number = BitConverter.ToUInt64(bytes, 0);

            return "SP" + ToBase62(number);
        }

        public async Task<SanPhamDTO> Create(SanPhamRequestDTO sp)
        {
            try
            {

                string imageUrl = null;

                // Xử lý upload ảnh nếu có
                if (sp.ImageUrl != null && sp.ImageUrl.Length > 0)
                {
                    var uploadResult = await _imageService.SaveImageAsync(sp.ImageUrl);
                    if (uploadResult.Success && uploadResult.Data != null)
                    {
                        imageUrl = uploadResult.Data.Url;
                    }
                }


                // Tạo entity sản phẩm mới
                SanPham sanpham = new SanPham
                {
                    ProductName = sp.ProductName,
                    Barcode = generateAutoBarcode(),
                    Price = sp.Price,
                    Unit = sp.Unit,
                    CreatedAt = DateTime.Now,
                    Status = sp.Status,
                    ImageUrl = imageUrl,
                    CategoryID = sp.CategoryID,
                    SupplierID = sp.SupplierID,
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

                
                if(sanpham!= null)
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
        public async Task<SanPhamDTO> Update(int id, SanPhamRequestDTO sp)
        {
            try
            {
                // SỬA: Thêm await
                var existingProduct = await _context.SanPhams
                    .FirstOrDefaultAsync(x => x.ProductID == id);

                if (existingProduct == null)
                    return null;

                

                // QUAN TRỌNG: Update từng property thay vì dùng AutoMapper
                existingProduct.ProductName = sp.ProductName;
                existingProduct.Barcode = sp.Barcode;
                existingProduct.Price = sp.Price;
                existingProduct.Unit = sp.Unit;
                existingProduct.Status = sp.Status;

                // CHỈ update ID, không update navigation objects
                existingProduct.CategoryID = sp.CategoryID;
                existingProduct.SupplierID = sp.SupplierID;
                if (sp.ImageUrl != null && sp.ImageUrl.Length > 0)
                {
                    // 1. Xóa ảnh cũ nếu có
                    if (!string.IsNullOrEmpty(existingProduct.ImageUrl))
                    {
                        var oldFileName = Path.GetFileName(existingProduct.ImageUrl);
                        await _imageService.DeleteImageAsync(oldFileName);
                    }

                    // 2. Upload ảnh mới và lấy URL
                    var uploadResult = await _imageService.SaveImageAsync(sp.ImageUrl);
                    if (uploadResult.Success && uploadResult.Data != null)
                    {
                        existingProduct.ImageUrl = uploadResult.Data.Url; // Lưu URL vào database
                        Console.WriteLine($"✅ Đã upload ảnh mới: {uploadResult.Data.Url}");
                    }
                    else
                    {
                        Console.WriteLine($"❌ Lỗi upload ảnh: {uploadResult.Message}");
                        // Có thể giữ nguyên ảnh cũ hoặc xử lý lỗi
                    }
                }

                // KHÔNG cần gọi Update() vì Entity Framework đang track entity
                await _context.SaveChangesAsync();

                // Load lại với đầy đủ thông tin để trả về
                var updatedProduct = await _context.SanPhams
                    .Include(x => x.Category)
                    .Include(x => x.Supplier)
                    .FirstOrDefaultAsync(x => x.ProductID == id);

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
                .Include(sp => sp.Supplier).Where(x => x.SupplierID == supplier_id).ToListAsync();
                    return _mapper.Map<List<SanPhamDTO>>(list);
            } catch (Exception e)
            {
                throw new Exception("Lỗi khi tìm sản phẩm theo nhà cung cấp: " + e.Message);
            }
        }
        public async Task<List<SanPhamDTO>> getByCategoryID(int? category_id)
        {
            try
            {
                var list = await _context.SanPhams.Include(sp => sp.Category)
                .Include(sp => sp.Supplier).Where(x => x.CategoryID == category_id).ToListAsync();
                return _mapper.Map<List<SanPhamDTO>>(list);
            } catch(Exception e)
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
            } catch (Exception e)
            {
                throw new Exception("Lỗi khi lấy danh sách sắp xếp theo yêu cầu: " + e.Message);
            }
        }
        public async Task<List<SanPhamDTO>> getProductsBySupplierIDAndCategoryID(int? supplier_id, int? category_id)
        {
            try
            {
                var ls = await _context.SanPhams.Include(sp => sp.Category)
            .Include(sp => sp.Supplier).Where(x => x.CategoryID == category_id && x.SupplierID == supplier_id).ToListAsync();
                return _mapper.Map<List<SanPhamDTO>>(ls);
            } catch (Exception e)
            {
                throw new Exception("Lỗi khi tìm sản phẩm theo nhà cung cấp và loại: " + e.Message);
            }
        }
        public async Task<List<SanPhamDTO>> getProductsBySupplierIDAndPrice(int? supplier_id, string? order)
        {
            try
            {
                var query = _context.SanPhams.Include(sp => sp.Category).Include(sp => sp.Supplier).Where(x => x.SupplierID == supplier_id);
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
            } catch (Exception e)
            {
                throw new Exception("Lỗi khi lấy danh sách sản phẩm sắp xếp theo nhà cung cấp: " + e.Message);
            }
        }
        public async Task<List<SanPhamDTO>> getProductsByCategoryIDAndPrice(int? category_id, string? order)
        {
            try
            {
                var query = _context.SanPhams.Include(sp => sp.Category).Include(sp => sp.Supplier).Where(x => x.CategoryID == category_id);
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
            } catch (Exception e)
            {
                throw new Exception("Lỗi khi lấy danh sách sản phẩm theo yêu cầu và loại: " + e.Message);
            }
        }

        
        public async Task<List<SanPhamDTO>> getProductsBysupplierIDAndCategoryIDAndPriceAndKeyword(int? supplier_id, int? category_id, string? order, string? keyword)
        {
            try
            {
                
                var query = _context.SanPhams.Include(sp => sp.Category).Include(sp => sp.Supplier).AsQueryable();

                // Filter by supplier
                if (supplier_id.HasValue)
                {
                    query = query.Where(p => p.SupplierID == supplier_id.Value);
                }

                // Filter by category
                if (category_id.HasValue)
                {
                    query = query.Where(p => p.CategoryID == category_id.Value);
                }

                // Search by keyword
                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(p =>
                        p.ProductName.Contains(keyword));
                }

                // Sort by price
                if (!string.IsNullOrEmpty(order))
                {
                    query = order.ToLower() == "desc"
                        ? query.OrderByDescending(p => p.Price)
                        : query.OrderBy(p => p.Price);
                }

                //return await query.Select(p => new SanPhamDTO
                //{
                //}).ToListAsync();
                var list = await query.ToListAsync();
                return _mapper.Map<List<SanPhamDTO>>(list);
            } catch (Exception e)
            {
                throw new Exception("Lỗi khi lấy danh sách sản phẩm theo yêu cầu, nhà cung cấp, loại, keyword: " + e.Message);
            }
        }
    }
}
