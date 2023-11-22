using Shop.Services.CouponAPI.Models;

namespace Shop.Services.CouponAPI.Repositories
{
    public interface ICouponRepository
    {
        public IEnumerable<Coupon> GetCoupons();
        public Coupon GetCouponById(int id);

        public Coupon GetCouponByCode(string code);

        public Coupon Add(Coupon coupon);
        public Coupon Update(Coupon coupon);
        public Coupon Delete(int id);
    }
}
