using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ibear_test.Database
{
    internal class Contractor
    {
        [Key]
        public Guid ID { get; set; }

        [Required, DisplayName("Имя")]
        public string Name { get; set; }

        [Required, DisplayName("Номер телефона"), MinLength(11), MaxLength(11)]
        public long Phone { get; set; }

        [DisplayName("Фото")]
        public byte[] Photo { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }


        [DisplayName("e-mail")]
        public string Email { get; set; }
    }
}
