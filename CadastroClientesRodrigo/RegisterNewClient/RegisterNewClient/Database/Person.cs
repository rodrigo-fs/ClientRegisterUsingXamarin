using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RegisterNewClient.Database
{
    public class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PersonId{ get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        [Phone]
        public string Telephone { get; set; }
        public string ImagePath { get; set; }
    }
}
