using Microsoft.EntityFrameworkCore;
using Shop.Service.RewardAPI.Message;
using Shop.Service.RewardAPI.Models;
using Shop.Services.RewardAPI.Data;
using Shop.Services.RewardAPI.Service.IService;

namespace Shop.Services.RewardAPI.Service
{
    public class RewardService : IRewardService
    {
        private DbContextOptions<AppDbContext> _dbOptions { get; }

        public RewardService(DbContextOptions<AppDbContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        public async Task UpdateRewards(RewardsMessage rewardsMessage)
        {
            try
            {
                Rewards rewards = new()
                {
                    OrderId = rewardsMessage.OrderId,
                    RewardsActivity = rewardsMessage.RewardsActivity,
                    UserId = rewardsMessage.UserId,
                    RewardsDate = DateTime.Now
                };

                await using var _db = new AppDbContext(_dbOptions);

                await _db.Rewards.AddAsync(rewards);
                await _db.SaveChangesAsync();
            }
            catch (Exception)
            {
            }
        }
    }
}
