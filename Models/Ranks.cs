using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFPractica01.Models
{
    public class Ranks
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int SkillsId { get; set; }
        [ForeignKey("SkillsId")]
        public Skills Skills { get; set; }
        public int Level { get; set; }
        public string? Description { get; set; }
        public string? SkillName { get; set; }
    }
}
