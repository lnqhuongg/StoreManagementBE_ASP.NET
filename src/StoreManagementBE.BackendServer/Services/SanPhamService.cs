using Microsoft.EntityFrameworkCore;
using StoreManagementBE.BackendServer.DTOs;
using StoreManagementBE.BackendServer.Enum;
using StoreManagementBE.BackendServer.Models;
using StoreManagementBE.BackendServer.Models.Entities;
using StoreManagementBE.BackendServer.Services.Interfaces;

namespace StoreManagementBE.BackendServer.Services
{
    public class SanPhamService : ISanPhamService
    {
        private readonly ApplicationDbContext _context;

        public SanPhamService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<SanPham> GetAll()
        {
            //return _context.SanPhams.ToList();
            return _context.SanPhams
            .Include(sp => sp.Category)
            .Include(sp => sp.Supplier)
            .ToList();
            }
        public SanPham GetById(int id)
        {
            return _context.SanPhams
                .Include(sp => sp.Category)
                .Include(sp => sp.Supplier)
                .FirstOrDefault(sp => sp.product_id == id);
        }


        public bool Create(SanPhamDTO sp)
        {
            if (sp == null) return false;

            if (_context.SanPhams.Any(x => x.barcode == sp.barcode))
            {
                throw new Exception("Mã vạch đã tồn tại, vui lòng nhập mã khác!");
            }

            var sanpham = new SanPham
            {
                product_id = sp.product_id,
                category_id = sp.Category.category_id,
                supplier_id = sp.Supplier.supplier_id,
                product_name = sp.product_name,
                barcode = sp.barcode,
                price = sp.price,
                unit = ApplicationDbContext.ParseUnit(sp.unit),
                created_at = DateTime.Now,
                status = sp.status
            };

            _context.SanPhams.Add(sanpham);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            SanPham sp = _context.SanPhams.Find(id);
            if (sp!=null)
            {
                _context.SanPhams.Remove(sp);
                _context.SaveChanges();
                return true;
            } else
            {
                throw new Exception("Không tìm thấy sản phẩm cần cập nhật!");
                return false;
            }
        }
        public bool Update(SanPhamDTO sp)
        {
            var sanpham = _context.SanPhams.FirstOrDefault(x => x.product_id == sp.product_id);

            if (sanpham == null)
                throw new Exception("Không tìm thấy sản phẩm cần cập nhật!");

            if (_context.SanPhams.Any(x => x.barcode == sp.barcode && x.product_id != sp.product_id))
                throw new Exception("Mã vạch đã tồn tại, vui lòng nhập mã khác!");

            sanpham.category_id = sp.Category?.category_id;
            sanpham.supplier_id = sp.Supplier?.supplier_id;
            sanpham.product_name = sp.product_name;
            sanpham.barcode = sp.barcode;
            sanpham.price = sp.price;
            sanpham.unit = ApplicationDbContext.ParseUnit(sp.unit);
            sanpham.status = sp.status;

            _context.SanPhams.Update(sanpham);
            _context.SaveChanges();

            return true;
        }

        public bool UpdateStatus(int id)
        {
            try
            {
                SanPham sanpham = _context.SanPhams.Find(id);
                if (sanpham != null)
                {
                    Console.WriteLine("id sp update status: ", sanpham.product_id);
                    sanpham.status = (sanpham.status == 1) ? 0 : 1;

                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    throw new Exception("Không tìm thấy sản phẩm cần cập nhật!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật: {ex.Message}");
            }
        }
        public List<SanPham> searchByKeyword(string keyword)
        {
            return _context.SanPhams
                .Include(sp => sp.Category)
                .Include(sp => sp.Supplier)
                .Where(x => x.product_name.ToLower().Contains(keyword.ToLower()))
                .ToList();
        }
        public List<SanPham> getBySupplierID(int? supplier_id)
        {
            return _context.SanPhams.Include(sp => sp.Category)
            .Include(sp => sp.Supplier).Where(x => x.supplier_id == supplier_id).ToList();
        }
        public List<SanPham> getByCategoryID(int? category_id)
        {
            return _context.SanPhams.Include(sp => sp.Category)
            .Include(sp => sp.Supplier).Where(x => x.category_id == category_id).ToList();
        }
        public List<SanPham> getProductsSortByPrice(string? order)
        {
            var query = _context.SanPhams.Include(sp => sp.Category).Include(sp => sp.Supplier);
            if(order.ToLower() == "desc")
            {
                return query.OrderByDescending(sp => sp.price).ToList();
            }
            return query.OrderBy(sp => sp.price).ToList();
        }
        public List<SanPham> getProductsBySupplierIDAndCategoryID(int? supplier_id, int? category_id)
        {
            return _context.SanPhams.Include(sp => sp.Category)
            .Include(sp => sp.Supplier).Where(x => x.category_id == category_id && x.supplier_id == supplier_id).ToList();
        }
        public List<SanPham> getProductsBySupplierIDAndPrice(int? supplier_id, string? order)
        {
            var query = _context.SanPhams.Include(sp => sp.Category).Include(sp => sp.Supplier).Where(x => x.supplier_id == supplier_id);
            if(order != "")
            {
                if (order.ToLower() == "desc")
                {
                    return query.OrderByDescending(sp => sp.price).ToList();
                }
            } else
            {
                return query.OrderBy(sp => sp.price).ToList();
            }
            
            return query.OrderBy(sp => sp.price).ToList();
        }
        public List<SanPham> getProductsByCategoryIDAndPrice(int? category_id, string? order)
        {
            var query = _context.SanPhams.Include(sp => sp.Category).Include(sp => sp.Supplier).Where(x => x.category_id == category_id);
            if(order != "")
            {
                if (order.ToLower() == "desc")
                {
                    return query.OrderByDescending(sp => sp.price).ToList();
                }
            } else
            {
                return query.OrderBy(sp => sp.price).ToList();
            }
            
            return query.OrderBy(sp => sp.price).ToList();
        }
        public List<SanPham> getProductsBysupplierIDAndCategoryIDAndPrice(int? supplier_id, int? category_id, string? order)
        {
            var query = _context.SanPhams.Include(sp => sp.Category).Include(sp => sp.Supplier).Where(x => x.category_id == category_id && x.supplier_id == supplier_id);
            if(order != "")
            {
                return query.OrderBy(sp => sp.price).ToList();
            } else
            {
                if (order.ToLower() == "desc")
                {
                    return query.OrderByDescending(sp => sp.price).ToList();
                }
            }
            return query.OrderBy(sp => sp.price).ToList();
        }
    }
}
