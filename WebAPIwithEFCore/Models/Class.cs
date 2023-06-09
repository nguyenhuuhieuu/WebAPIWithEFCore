namespace WebAPIwithEFCore.Models
{
    public class Class
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public virtual ICollection<Student> Students { get; set; }

        public ClassDTO ToDTO()
        {
            return new ClassDTO
            {
                ClassId = ClassId,
                ClassName = ClassName,
                //StudentNumber = (Students == null) ? 0 : Students.Count()
            };
        }
    }
}