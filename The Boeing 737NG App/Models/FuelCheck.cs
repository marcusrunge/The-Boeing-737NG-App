using SQLite;

namespace The_Boeing_737NG_App.Models
{
    public class FuelCheck
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Difference { get; set; }
        public string Time { get; set; }
    }
}