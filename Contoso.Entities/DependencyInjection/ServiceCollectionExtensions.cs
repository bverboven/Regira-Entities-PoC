using Contoso.Data;
using Contoso.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Regira.DAL.EFcore.Services;
using Regira.Entities.DependencyInjection.Attachments;
using Regira.Entities.DependencyInjection.Mapping;
using Regira.Entities.DependencyInjection.Primers;
using Regira.Entities.DependencyInjection.QueryBuilders;
using Regira.Entities.DependencyInjection.ServiceBuilders.Abstractions;
using Regira.Entities.DependencyInjection.ServiceBuilders.Extensions;
using Regira.Entities.EFcore.Primers;
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
                e.UseAutoMapper();
                e.ConfigureDefaultJsonOptions();

                e.AddDefaultGlobalQueryFilters<Guid>();
                e.AddPrimer<HasGuidKeyPrimer>();
            })
            .WithAttachments(_ =>
            {
                var dir = new DirectoryInfo(Path.Combine(AppContext.BaseDirectory, "../../../../data")).FullName;
                return new BinaryFileService(new FileSystemOptions { RootFolder = Path.Combine(dir, "data") });
            })
            //.AddSimpleEntities()
            .AddEntities()
            ;

        return services;
    }

    public static IEntityServiceCollection<ContosoContext> AddSimpleEntities(this IEntityServiceCollection<ContosoContext> services)
    {
        return services
            .For<Person>()
            .For<Student>()
            .For<Instructor>(e => e.HasAttachments(item => item.Attachments))
            .For<Department, Guid>()
            .For<Course, int, CourseSearchObject, CourseSortBy, CourseIncludes>(e => e.HasAttachments(item => item.Attachments));
    }
    public static IEntityServiceCollection<ContosoContext> AddEntities(this IEntityServiceCollection<ContosoContext> services)
    {
        return services
            .For<Person>()
            .For<Student>(student =>
            {
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
                instructor.SortBy(query => query
                    .OrderBy(x => x.LastName)
                    .ThenBy(x => x.GivenName)
                );
                instructor.Includes((query, incl) =>
                {
                    if (incl?.HasFlag(EntityIncludes.All) == true)
                    {
                        query = query.Include(x => x.OfficeAssignments);
                        query = query.Include(x => x.Attachments!)
                            .ThenInclude(x => x.Attachment);
                    }
                    return query;
                });
                instructor.HasAttachments(e => e.Attachments);
            })
            .For<Department, Guid>(dep =>
            {
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
            .For<Course, int, CourseSearchObject, CourseSortBy, CourseIncludes>(course =>
            {
                course.Filter((query, so) =>
                {
                    if (!string.IsNullOrWhiteSpace(so?.Q))
                    {
                        query = query.Where(x => x.Title!.Contains(so.Q));
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
                    if (incl?.HasFlag(CourseIncludes.Instructors) == true)
                    {
                        query = query.Include(x => x.Instructors);
                    }
                    if (incl?.HasFlag(CourseIncludes.Enrollments) == true)
                    {
                        query = query.Include(x => x.Enrollments!)
                            .ThenInclude(x => x.Student);
                    }
                    if (incl?.HasFlag(CourseIncludes.Attachments) == true)
                    {
                        query = query.Include(x => x.Attachments!)
                            .ThenInclude(x => x.Attachment);
                    }
                    return query;
                });
                course.HasAttachments(e => e.Attachments);
            });
    }
}
