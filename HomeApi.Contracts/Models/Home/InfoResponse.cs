namespace HomeApi.Contracts.Home
{
    public class InfoResponse
    {
        public int FloorAmount { get; set; }
        public string Telephone { get; set; }
        public string Heating { get; set; }
        public int CurrentVolts { get; set; }
        public int Area { get; set; }
        public string Material { get; set; }
        public AddressInfo AddressInfo { get; set; }
    }
}
