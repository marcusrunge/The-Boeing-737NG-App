namespace The_Boeing_737NG_App.Services
{
    public interface IFuelCheckListService
    {
    }
    public class FuelCheckListService : IFuelCheckListService
    {
        public static FuelCheckListService Instance() => new FuelCheckListService();
    }
}