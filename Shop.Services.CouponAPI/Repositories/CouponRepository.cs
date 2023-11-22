using Shop.Services.CouponAPI.Data;
using Shop.Services.CouponAPI.Models;

namespace Shop.Services.CouponAPI.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly AppDbContext _context;
        public CouponRepository(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Coupon> GetCoupons()
        {
            return _context.Coupons.ToList();
        }

        public Coupon GetCouponById(int id)
        {
            return _context.Coupons.FirstOrDefault(x => x.CouponId == id);
        }

        public Coupon GetCouponByCode(string code)
        {
            return _context.Coupons.FirstOrDefault(x => x.CouponCode.ToLower() == code.ToLower());
        }

        public Coupon Add(Coupon coupon)
        {
            _context.Coupons.Add(coupon);
            _context.SaveChanges();

            return coupon;
        }

        public Coupon Update(Coupon coupon)
        {
            _context.Coupons.Update(coupon);
            _context.SaveChanges();

            return coupon;
        }

        public Coupon Delete(int id)
        {
            Coupon coupon = GetCouponById(id);
            _context.Coupons.Remove(coupon);
            _context.SaveChanges();

            return coupon;
        }
    }
}
