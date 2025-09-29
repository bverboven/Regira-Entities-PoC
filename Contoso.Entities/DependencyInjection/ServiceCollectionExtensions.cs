using Contoso.Constants;
using Contoso.Data;
using Contoso.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Regira.DAL.EFcore.Services;
using Regira.Entities.DependencyInjection.Attachments;
using Regira.Entities.DependencyInjection.Json;
using Regira.Entities.DependencyInjection.Preppers;
using Regira.Entities.DependencyInjection.QueryBuilders;
using Regira.Entities.DependencyInjection.ServiceBuilders.Abstractions;
using Regira.Entities.DependencyInjection.ServiceBuilders.Extensions;
using Regira.Entities.EFcore.Primers;
using Regira.Entities.Mapping.AutoMapper;
using Regira.Entities.Models;
using Regira.IO.Storage.FileSystem;

namespace Contoso.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddContosoEntities(this IServiceCollection services, Action<DbContextOptionsBuilder> configure)
    {
        services
            .AddDbContext<ContosoContext>((sp, db) =>
            {
                configure.Invoke(db);
                db.AddPrimerInterceptors(sp);
                db.AddAutoTruncateInterceptors();
            });

        services.AddHttpContextAccessor();

        services
            .UseEntities<ContosoContext>(e =>
            {
                e.UseDefaults();
                e.ConfigureDefaultJsonOptions();
                e.UseAutoMapper();

                e.AddDefaultGlobalQueryFilters<Guid>();
                //e.AddPrimer<HasGuidKeyPrimer>();
                e.AddPrepper<HasGuidPrepper>();
            })
            .WithAttachments(_ => new BinaryFileService(new FileSystemOptions { RootFolder = AppSettings.DataFolder }))
            .AddEntities();

        return services;
    }

    public static IEntityServiceCollection<ContosoContext> AddEntities(this IEntityServiceCollection<ContosoContext> services)
    {
        return services
            .For<Person>(p =>
            {
                p.AddMapping<PersonDto, PersonInputDto>();
            })
            .For<Student>(student =>
            {
                student.AddMapping<StudentDto, StudentInputDto>();
                student.SortBy(query => query
                    .OrderBy(x => x.GivenName)
                    .ThenBy(x => x.LastName)
                );
                student.Includes((query, incl) =>
                {
                    if (incl?.HasFlag(EntityIncludes.All) == true)
                    {
                        query = query
                            .Include(x => x.Enrollments!)
                            .ThenInclude(x => x.Course);
                    }
                    return query;
                });
            })
            .For<Instructor>(instructor =>
            {
                instructor.AddMapping<InstructorDto, InstructorInputDto>();
                instructor.SortBy(query => query
                    .OrderBy(x => x.LastName)
                    .ThenBy(x => x.GivenName)
                );
                instructor.Includes((query, incl) =>
                {
                    if (incl?.HasFlag(EntityIncludes.All) == true)
                    {
                        query = query.Include(x => x.Courses!).ThenInclude(x => x.Course);
                        query = query.Include(x => x.OfficeAssignments);
                        query = query.Include(x => x.Attachments!)
                            .ThenInclude(x => x.Attachment);
                    }
                    return query;
                });
                instructor.HasAttachments(e => e.Attachments, a =>
                {
                    a.AddMapping<InstructorAttachmentDto, InstructorAttachmentInputDto>();
                });
            })
            .For<Department, Guid>(dep =>
            {
                dep.AddMapping<DepartmentDto, DepartmentInputDto>();
                dep.SortBy(query => query.OrderBy(x => x.Title));
                dep.Includes((query, incl) =>
                {
                    query = query.Include(d => d.Administrator);
                    if (incl?.HasFlag(EntityIncludes.All) == true)
                    {
                        query = query.Include(d => d.Courses);
                    }
                    return query;
                });
            })
            .For<Course, CourseSearchObject, CourseSortBy, CourseIncludes>(course =>
            {
                course.AddMapping<CourseDto, CourseInputDto>();
                course.Filter((query, so) =>
                {
                    // DepartmentId
                    if (so?.DepartmentId?.Any() == true)
                    {
                        query = query.Where(x => so.DepartmentId.Contains(x.DepartmentId));
                    }
                    // InstructorId
                    if (so?.InstructorId?.Any() == true)
                    {
                        query = query.Where(x => x.Instructors!.Any(instructor => so.InstructorId.Contains(instructor.Id)));
                    }
                    // Title
                    if (!string.IsNullOrWhiteSpace(so?.Q))
                    {
                        query = query.Where(x => EF.Functions.Like(x.Title, $"%{so.Q}%"));
                    }
                    // Credits
                    if (so?.MinCredits.HasValue == true)
                    {
                        query = query.Where(x => x.Credits >= so.MinCredits);
                    }
                    if (so?.MaxCredits.HasValue == true)
                    {
                        query = query.Where(x => x.Credits <= so.MaxCredits);
                    }
                    // EnrollmentDate
                    if (so?.MinEnrollmentDate.HasValue == true)
                    {
                        query = query.Where(x => x.Enrollments!.Any(e => e.StartDate >= so.MinEnrollmentDate));
                    }
                    if (so?.MaxEnrollmentDate.HasValue == true)
                    {
                        query = query.Where(x => x.Enrollments!.Any(e => e.StartDate < so.MaxEnrollmentDate.Value.Date.AddDays(1)));
                    }
                    return query;
                });
                course.SortBy((query, sortBy) =>
                {
                    switch (sortBy)
                    {
                        case CourseSortBy.IdDesc:
                            return query.OrderByDescending(x => x.Id);
                        case CourseSortBy.Title:
                            return query.OrderBy(x => x.Title);
                        case CourseSortBy.TitleDesc:
                            return query.OrderByDescending(x => x.Title);
                        case CourseSortBy.Department:
                            return query.OrderBy(x => x.Department!.Title);
                        case CourseSortBy.EnrollmentDate:
                            return query.OrderBy(x => x.Enrollments!.Min(e => e.StartDate));
                        case CourseSortBy.EnrollmentDateDesc:
                            return query.OrderByDescending(x => x.Enrollments!.Min(e => e.StartDate));
                        default:
                            return query.OrderBy(x => x.Id);
                    }
                });
                course.Includes((query, incl) =>
                {
                    if (incl == null)
                    {
                        return query;
                    }

                    if (incl.Value.HasFlag(CourseIncludes.Department))
                    {
                        query = query.Include(x => x.Department);
                    }
                    if (incl.Value.HasFlag(CourseIncludes.Instructors))
                    {
                        query = query.Include(x => x.Instructors!).ThenInclude(x => x.Instructor);
                    }
                    if (incl.Value.HasFlag(CourseIncludes.Enrollments) || incl.Value.HasFlag(CourseIncludes.Students))
                    {
                        query = incl.Value.HasFlag(CourseIncludes.Students)
                            ? query.Include(x => x.Enrollments!)
                                .ThenInclude(x => x.Student)
                            : query.Include(x => x.Enrollments);
                    }
                    if (incl.Value.HasFlag(CourseIncludes.Attachments))
                    {
                        query = query.Include(x => x.Attachments!)
                            .ThenInclude(x => x.Attachment);
                    }
                    return query;
                });
                course.Related(e => e.Enrollments);
                course.Related(e => e.Instructors);
                course.HasAttachments(e => e.Attachments);
            });
    }
}
