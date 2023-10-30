using MsPointsPurchaseApi.Contracts.Repositories;

namespace MsPointsPurchaseApi.Contracts.Migrations
{
    public class Migration_1_0_0
    {
        public static async Task Apply(IDbVersionRepository dbVersionRepository,
            IPointsRepository pointsRepository)
        {
            var points = await pointsRepository.List();
            foreach (var x in points)
            {
                await pointsRepository.Delete(x.Id);
            }
            await pointsRepository.Create(new DTOs.Points()
            {
                MinPointsValue = 200,
                MaxPointsValue = 999,
                Fee = 0.03,
                FeeValue = 0,
                Price = 0,
                Active = true
            });
            await pointsRepository.Create(new DTOs.Points()
            {
                MinPointsValue = 1000,
                MaxPointsValue = 9999,
                Fee = 0.03,
                FeeValue = 0,
                Price = 0,
                Active = true
            });
            await pointsRepository.Create(new DTOs.Points()
            {
                MinPointsValue = 10000,
                MaxPointsValue = 49999,
                Fee = 0.03,
                FeeValue = 0,
                Price = 0,
                Active = true
            });
            await pointsRepository.Create(new DTOs.Points()
            {
                MinPointsValue = 50000,
                MaxPointsValue = 249999,
                Fee = 0.03,
                FeeValue = 0,
                Price = 0, 
                Active = true
            });
            await dbVersionRepository.SetVersion("1.0.1");
        }
    }
}
