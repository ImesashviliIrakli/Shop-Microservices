
using Shop.Service.RewardAPI.Message;

namespace Shop.Services.RewardAPI.Service.IService
{
    public interface IRewardService
    {
        public Task UpdateRewards(RewardsMessage rewardsMessage);
    }
}
