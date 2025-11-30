using Bogus;
using Contoso.Constants;
using Contoso.Data;
using Contoso.Entities;
using Regira.Entities.Attachments.Models;
using Regira.Entities.Services.Abstractions;
using System.Text;

namespace Contoso.Console;

public class DataSeeder(ContosoContext dbContext, IEntityService<Instructor> instructorService, IEntityService<InstructorAttachment> instructorAttachmentService,
    IEntityService<Student> studentService, IEntityService<Department, Guid> departmentService, IEntityService<Course, int> courseService, IEntityService<CourseAttachment> courseAttachmentService)
{
    public async Task SeedDataAsync(int factor = 1)
    {
        var faker = new Faker();

        var instructors = SeedInstructors(20 * factor);
        var students = SeedStudents(100 * factor);
        var courses = SeedCourses(25 * factor);
        var departments = SeedDepartments(5 * factor);

        foreach (var item in instructors)
        {
            item.OfficeAssignments = new Faker<OfficeAssignment>()
                .RuleFor(x => x.Location, f => f.Address.FullAddress())
                .Generate(faker.Random.Number(0, 3));
            await instructorService.Add(item);
        }
        foreach (var item in students)
        {
            item.Enrollments = faker.PickRandom(courses, faker.Random.Number(1, 10))
                .Select(x => new Enrollment
                {
                    Course = x,
                    Grade = faker.PickRandom(Enum.GetValues<Grade>()),
                    StartDate = faker.Date.Between(new DateTime(DateTime.Today.Year - 3, 9, 1), new DateTime(DateTime.Today.Year + 1, 9, 1))
                }).ToList();
            await studentService.Add(item);
        }
        foreach (var item in departments)
        {
            item.Administrator = faker.PickRandom(instructors);
            await departmentService.Add(item);
        }
        foreach (var item in courses)
        {
            var department = faker.PickRandom(departments);
            item.DepartmentId = department.Id;
            item.Instructors = faker.PickRandom(instructors, faker.Random.Number(0, 2)).Concat([department.Administrator!])
                .Select(instructor =>
                {
                    var start = faker.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now);
                    return new CourseInstructor
                    {
                        Instructor = instructor,
                        Course = item,
                        StartDate = start,
                        EndDate = start.AddMonths(faker.Random.Int(1, 18))
                    };
                })
                .ToList();
            await courseService.Add(item);
        }

        await dbContext.SaveChangesAsync();

        foreach (var item in instructors)
        {
            var attachments = SeedAttachments(faker.Random.Number(0, 3));
            var itemAttachments = attachments.Select(x => new InstructorAttachment
            {
                Attachment = x,
                ObjectId = item.Id,
                AttachmentType = faker.Lorem.Word()
            }).ToList();

            foreach (var attachment in itemAttachments)
            {
                await instructorAttachmentService.Add(attachment);
            }
        }
        foreach (var item in courses)
        {
            var attachments = SeedAttachments(faker.Random.Number(0, 3));
            var itemAttachments = attachments.Select(x => new CourseAttachment
            {
                Attachment = x,
                ObjectId = item.Id
            }).ToList();

            foreach (var attachment in itemAttachments)
            {
                await courseAttachmentService.Add(attachment);
            }
        }

        await dbContext.SaveChangesAsync();
    }

    public IList<Attachment> SeedAttachments(int count)
    {
        return new Faker<Attachment>()
            .RuleFor(x => x.FileName, f => $"{f.Music.Genre()}.txt")
            .RuleFor(x => x.Bytes, (_, x) => Encoding.Default.GetBytes(x.FileName!))
            .Generate(count);
    }

    public IList<Instructor> SeedInstructors(int count)
    {
        return new Faker<Instructor>()
            .RuleFor(x => x.GivenName, (f) => f.Name.FirstName())
            .RuleFor(x => x.LastName, (f) => f.Name.LastName())
            .Generate(count);
    }
    public IList<Student> SeedStudents(int count)
    {
        return new Faker<Student>()
            .RuleFor(x => x.GivenName, (f) => f.Name.FirstName())
            .RuleFor(x => x.LastName, (f) => f.Name.LastName())
            .Generate(count);
    }
    public IList<Course> SeedCourses(int count)
    {
        return new Faker<Course>()
            .RuleFor(x => x.Title, (f) => f.Commerce.ProductName())
            .RuleFor(x => x.Credits, (f) => f.Random.Number(0, 5).OrNull(f, .2f))
            .Generate(count);
    }
    public IList<Department> SeedDepartments(int count)
    {
        var faker = new Faker();
        return faker.Commerce.Categories(count)
            .Select(c => new Department
            {
                Title = c,
                Budget = faker.Random.Number(0, 10_000).OrNull(faker, .2f),
                StartDate = faker.Date.Between(new DateTime(DateTime.Today.Year - 10, 1, 1), new DateTime(DateTime.Today.Year, 1, 1))
            })
            .ToArray();
    }
}