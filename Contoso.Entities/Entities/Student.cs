namespace Contoso.Entities;

public class Student : Person
{
    public ICollection<Enrollment>? Enrollments { get; set; }
}

public class StudentDto : PersonDto
{
    public ICollection<EnrollmentDto>? Enrollments { get; set; }
}

public class StudentInputDto : PersonInputDto
{
    public ICollection<EnrollmentInputDto>? Enrollments { get; set; }
}