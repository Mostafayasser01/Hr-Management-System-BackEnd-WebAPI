using System.ComponentModel.DataAnnotations;

namespace HrSystem.Models
{
    public class General_Settings
    {
        public int Id { get; set; }
        public double Discound { get; set; }
        public double Bonus { get; set; }
        public string UnitUsed { get; set; }
        public string OffDay1 { get; set; }
        public string OffDay2 { get; set; }
       

    }
}
