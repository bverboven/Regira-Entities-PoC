using Contoso.Entities;
using Microsoft.AspNetCore.Mvc;
using Regira.Entities.Web.Attachments.Abstractions;
using Regira.Entities.Web.Controllers.Abstractions;

namespace Contoso.API.Controllers;

// ApiControllerAttribute is added automatically by EntityControllerBase<TEntity>

// Person
[Route("persons")]
public class PersonController : EntityControllerBase<Person>;

// Student
[Route("students")]
public class StudentController : EntityControllerBase<Student>;

// Instructor
[Route("instructors")]
public class InstructorController : EntityControllerBase<Instructor>;
[Route("instructors")]
public class InstructorAttachmentController : EntityAttachmentControllerBase<InstructorAttachment>;

// Course
[Route("courses")]
public class CourseController : EntityControllerBase<Course, int, CourseSearchObject, CourseSortBy, CourseIncludes, Course, Course>;
[Route("courses")]
public class CourseAttachmentController : EntityAttachmentControllerBase<CourseAttachment>;

// Department
[Route("departments")]
public class DepartmentController : EntityControllerBase<Department, Guid>;
