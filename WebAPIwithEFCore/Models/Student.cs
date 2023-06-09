using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPIwithEFCore.Models
{
    public class Student
    {
        //data annotation
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentId { get; set; }

        [MaxLength(50)]
        public string StudentName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Gender? Gender { get; set; }
        public int? Friends { get; set; }

       
        public int ClassId { get; set; }
        [ForeignKey("ClassId")]
        public virtual Class Class { get; set; }

        public StudentDTO ToDTO()
        {
            return new StudentDTO
            {
                StudentId = StudentId,
                StudentName = StudentName,
                ClassId = ClassId,
                //ClassName = Class.ClassName       //chỉ có get student theo id, get all lỗi. tương tự với class
            };
        }

    }
    public enum Gender
    {
        MALE,
        FEMALE
    }


}
