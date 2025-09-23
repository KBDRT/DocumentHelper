using Microsoft.EntityFrameworkCore;

namespace DocumentHelper.Model.Reference
{
    [Owned]
    public class Address
    {
        public string City { get; set; } = string.Empty;

        public string Street { get; set; } = string.Empty;

        public string House { get; set; } = string.Empty;
    }
}
