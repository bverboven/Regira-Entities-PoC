using Contoso.Entities;
using Microsoft.AspNetCore.Mvc;
using Regira.Entities.Web.Attachments.Abstractions;
using Regira.Entities.Web.Controllers.Abstractions;

namespace Contoso.API.Controllers;

// attribute [ApiController] is added automatically by EntityControllerBase<TEntity>

[Route("persons")]
public class PersonController : EntityControllerBase<Person>;

[Route("students")]
public class StudentController : EntityControllerBase<Student>;

[Route("instructors")]
public class InstructorController : EntityControllerBase<Instructor>;
[Route("instructors")]
public class InstructorAttachmentController : EntityAttachmentControllerBase<InstructorAttachment>;

[Route("courses")]
public class CourseController : EntityControllerBase<Course, int, CourseSearchObject, CourseSortBy, CourseIncludes, Course, Course>;
[Route("courses")]
public class CourseAttachmentController : EntityAttachmentControllerBase<CourseAttachment>;

[Route("departments")]
public class DepartmentController : EntityControllerBase<Department, Guid>;
