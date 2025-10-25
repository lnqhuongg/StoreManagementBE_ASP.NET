using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreManagementBE.BackendServer.Models;
using StoreManagementBE.BackendServer.Models.Entities;
using StoreManagementBE.BackendServer.Services.Interfaces;

namespace StoreManagementBE.BackendServer.Services
{
    public class NhaCungCapService : INhaCungCapService
    {
        private readonly ApplicationDbContext dbContext;
        public NhaCungCapService(ApplicationDbContext context)
        {
            dbContext = context;
        }

        //lay tat ca nha cung cap
        public List<NhaCungCap> GetAll()
        {
            return dbContext.NhaCungCaps.ToList();
        }

        public NhaCungCap GetById(int suplier_id)
        {
            return dbContext.NhaCungCaps.Find(suplier_id);
        }

        public List<NhaCungCap> SearchByKeyword(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return dbContext.NhaCungCaps.ToList();
            return dbContext.NhaCungCaps.Where(x => x.name.Contains(keyword)).ToList();
        }

        public bool Create(NhaCungCap ncc)
        {
            try
            {
                dbContext.NhaCungCaps.Add(ncc);
                dbContext.SaveChanges();
                return true; // thêm thành công
            }
            catch
            {
                return false; // thêm thất bại
            }
        }

        public bool Update(NhaCungCap ncc)
        {
            var Ncc = dbContext.NhaCungCaps.Find(ncc.supplier_id);
            if (Ncc == null) return false;

            if (isExist(ncc.supplier_id))
            {
                Ncc.name = Ncc.name;
                try
                {
                    dbContext.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else return false;
        }

        //public bool Delete(int category_id)
        //{
        //    LoaiSanPham loaiTMP = _context.LoaiSanPhams.Find(category_id);
        //    if (isExist(category_id))
        //    {
        //        _context.LoaiSanPhams.Remove(loaiTMP);

        //        try
        //        {
        //            _context.SaveChanges();
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }
        //    }
        //    else return false;
        //}

        public bool isExist(int supplier_id)
        {
            var existing = dbContext.NhaCungCaps.Find(supplier_id);
            if (existing == null)
                return false;
            else return true;
        }
    }
}
