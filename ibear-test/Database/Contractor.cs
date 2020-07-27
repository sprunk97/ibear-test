using System;
using System.ComponentModel.DataAnnotations;

namespace ibear_test.Database
{
    internal class Contractor
    {
        [Key]
        public Guid ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public long Phone { get; set; }
        public byte[] Photo { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Email { get; set; }
    }
}
