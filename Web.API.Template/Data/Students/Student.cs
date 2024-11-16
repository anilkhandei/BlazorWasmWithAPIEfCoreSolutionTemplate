namespace Web.API.Template.Data.Students
{
    public class Student
    {
        public int StudentId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public virtual Address address { get; set; }
    }
}
