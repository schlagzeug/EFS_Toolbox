namespace ToolBox.Objects.eFleetSuite
{
    public class ProvisioningHost
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string ConnectionString { get; set; }

        public ProvisioningHost(string name, string address)
        {
            Name = name;
            Address = address;
        }
    }
}
