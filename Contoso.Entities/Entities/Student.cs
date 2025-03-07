namespace Contoso.Entities;

public class Student : Person
{
    public ICollection<Enrollment>? Enrollments { get; set; }
}